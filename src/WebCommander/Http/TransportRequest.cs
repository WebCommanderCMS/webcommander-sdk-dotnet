using System.Collections;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;

using WebCommander.Client;
using WebCommander.Config;
using WebCommander.Errors;

using static WebCommander.Http.QueryParams;

namespace WebCommander.Http;

/// <summary>One request, snapshotted: one wire representation reused by every attempt and replay.</summary>
public sealed class TransportRequest
{
    public TransportRequest(string httpMethod, string url)
    {
        HttpMethod = httpMethod;
        Url = url;
    }

    private TransportRequest(
        TransportRequest source,
        IReadOnlyDictionary<string, string>? headers,
        IReadOnlyDictionary<string, string>? requestInfoHeaders)
    {
        HttpMethod = source.HttpMethod;
        Url = source.Url;
        _queryParams = source._queryParams;
        _urlWithQuery = source.UrlWithQuery;
        _jsonBody = source._jsonBody;
        _jsonBodyBytes = source._jsonBodyBytes;
        _formData = source._formData;
        _formFields = source._formFields;
        _files = source._files;
        _streamOrigins = source._streamOrigins;
        _multipartBoundary = source._multipartBoundary;
        _prepared = source._prepared;
        _replaySafe = source._replaySafe;
        _wireBody = source._wireBody;
        _wireContentType = source._wireContentType;
        _headers = headers;
        _requestInfoHeaders = requestInfoHeaders;
        _requestInfoBody = source._requestInfoBody;
        ResponseFormat = source.ResponseFormat;
        DtoType = source.DtoType;
    }

    public string HttpMethod { get; }

    /// <summary>The absolute URL without the query string; see <see cref="UrlWithQuery"/>.</summary>
    public string Url { get; }

    /// <summary>Query parameters, normalised to the text they go out as and held immutably.</summary>
    public IReadOnlyDictionary<string, object?>? QueryParams
    {
        get => _queryParams;
        init => _queryParams = Redaction.DetachMap(
            value is null ? null : (IReadOnlyDictionary<string, object?>)BuildQueryParams(value));
    }

    /// <summary>A JSON body, encoded to its wire bytes as it is set, so it is read exactly once.</summary>
    public object? JsonBody
    {
        get => _jsonBody;
        init
        {
            _jsonBody = value;
            _jsonBodyBytes = Transport.EncodeJsonBody(value);
        }
    }

    internal (object? Body, byte[]? Bytes) EncodedJsonBody
    {
        init
        {
            _jsonBody = value.Body;
            _jsonBodyBytes = value.Bytes;
        }
    }

    /// <summary>Form-encoded data, normalised once as it is set; immutable afterwards.</summary>
    public IReadOnlyDictionary<string, object?>? FormData
    {
        get => _formData;
        init
        {
            var (recorded, sent) = NormaliseFormData(value);
            _formData = recorded;
            _formFields = sent;
        }
    }

    /// <summary>Multipart files to send; immutable, with byte parts copied both ways.</summary>
    public IReadOnlyDictionary<string, object?>? Files
    {
        get => Redaction.CopyBuffersInMap(_files);
        init
        {
            _files = Redaction.DetachMap(value);
            _streamOrigins = RecordStreamOrigins(_files);
        }
    }

    internal string? MultipartBoundary => _multipartBoundary;

    /// <summary>The real headers to send; <see cref="RequestInfoHeaders"/> is the redacted copy.</summary>
    public IReadOnlyDictionary<string, string>? Headers
    {
        get => _headers;
        init => _headers = Redaction.DetachText(value);
    }

    public IReadOnlyDictionary<string, string>? RequestInfoHeaders
    {
        get => _requestInfoHeaders;
        init => _requestInfoHeaders = Redaction.DetachText(value);
    }

    /// <summary>An already-redacted body to record on the response's request info.</summary>
    public object? RequestInfoBody
    {
        get => Redaction.CopyBuffers(_requestInfoBody);
        init => _requestInfoBody = Redaction.Detach(value);
    }

    /// <summary>Which form <c>Data</c> takes: <c>"map"</c>, <c>"json"</c> or <c>"dto"</c>.</summary>
    public string? ResponseFormat { get; init; }

    public Type? DtoType { get; init; }

    internal TransportRequest WithHeaders(
        IReadOnlyDictionary<string, string>? headers,
        IReadOnlyDictionary<string, string>? requestInfoHeaders) =>
        new(this, Redaction.DetachText(headers), Redaction.DetachText(requestInfoHeaders));

    internal async Task PrepareAsync(SdkConfig config, string method, CancellationToken cancellationToken)
    {
        if (_prepared)
            return;
        JudgeFileParts();
        using var deadline = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        deadline.CancelAfter(TimeSpan.FromMilliseconds(config.TimeoutSeconds * 1000));
        try
        {
            JudgeSourceCeiling(config);
            await CopyStreamPartsAsync(config.MaxRequestSpoolBytes, deadline.Token).ConfigureAwait(false);
            await SettleWireBodyAsync(config, method, deadline.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException error) when (!cancellationToken.IsCancellationRequested)
        {
            throw new SdkError(
                Redaction.RedactMessageText(
                    "the request body could not be assembled within the timeoutSeconds ceiling of "
                    + config.TimeoutSeconds.ToString(CultureInfo.InvariantCulture)
                    + " second(s) - an upload stream did not yield its bytes in time. Nothing was "
                    + "sent"),
                null,
                ErrorType.Timeout,
                null,
                null,
                null,
                error);
        }
        _prepared = true;
    }

    internal bool ReplaySafe => _replaySafe;

    internal byte[]? WireBody => _wireBody;

    internal string? WireContentType => _wireContentType;

    private void JudgeFileParts()
    {
        if (_files is null)
            return;
        foreach (var (field, value) in _files)
            if (Transport.ProblemWithFilePart(field, value) is { } problem)
                throw problem;
    }

    private async Task CopyStreamPartsAsync(long ceiling, CancellationToken cancellationToken)
    {
        if (_files is null || _files.Count == 0)
            return;
        var carriesAStream = false;
        foreach (var (_, value) in _files)
            if (value is Stream)
            {
                carriesAStream = true;
                break;
            }
        if (!carriesAStream)
            return;

        var frozen = new List<KeyValuePair<string, object?>>(_files.Count);
        foreach (var (field, value) in _files)
        {
            if (value is not Stream stream)
            {
                frozen.Add(new KeyValuePair<string, object?>(field, value));
                continue;
            }
            var filename = Transport.StreamPartFilename(field, stream);
            var copied = await CopyOnePartAsync(stream, ceiling, cancellationToken).ConfigureAwait(false);
            if (copied is null)
            {
                _replaySafe = false;
                frozen.Add(new KeyValuePair<string, object?>(field, value));
                continue;
            }
            frozen.Add(new KeyValuePair<string, object?>(
                field, ImmutableArray.Create<object?>(filename, copied)));
        }
        _files = Redaction.DetachedMapOf(frozen);
    }

    private async Task<byte[]?> CopyOnePartAsync(
        Stream stream, long ceiling, CancellationToken cancellationToken)
    {
        var origin = StreamOriginOf(stream);
        stream.Position = origin;
        var offered = Math.Max(0L, stream.Length - origin);
        if (offered > ceiling || offered > Array.MaxLength)
            return null;

        var buffer = new byte[offered];
        var filled = 0;
        while (filled < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(filled), cancellationToken)
                .ConfigureAwait(false);
            if (read <= 0)
                break;
            filled += read;
        }
        var overflow = new byte[1];
        if (await stream.ReadAsync(overflow.AsMemory(0, 1), cancellationToken).ConfigureAwait(false) > 0)
        {
            stream.Position = origin;
            return null;
        }
        return filled == buffer.Length ? buffer : buffer[..filled];
    }

    private async Task SettleWireBodyAsync(
        SdkConfig config, string method, CancellationToken cancellationToken)
    {
        if (_jsonBodyBytes is { } encoded)
        {
            JudgeFinalCeiling(config, encoded.LongLength);
            _wireBody = encoded;
            _wireContentType = Transport.JsonContentType;
            return;
        }
        if (_files is { Count: > 0 })
        {
            _multipartBoundary = Transport.NewMultipartBoundary();
            using var multipart = Transport.BuildMultipartContent(this, _multipartBoundary);
            var declared = multipart.Headers.ContentLength;
            if (declared is not long framed)
            {
                throw new SdkUsageError(Redaction.RedactMessageText(
                    "files: the multipart body's length could not be computed before it was "
                    + "assembled, so neither maxMultipartOverheadBytes nor maxRequestBytes could "
                    + "be applied to it, and it was not sent"));
            }
            JudgeFramingCeiling(config, framed - _sourceBytes);
            JudgeFinalCeiling(config, framed);
            _wireContentType = multipart.Headers.ContentType?.ToString();
            if (!_replaySafe)
                return;
            if (framed > Array.MaxLength)
                throw RefuseCeiling("the request body", framed, Array.MaxLength,
                    "maxRequestBytes", exact: true);
            using var assembled = new MemoryStream((int)framed);
            await multipart.CopyToAsync(assembled, cancellationToken).ConfigureAwait(false);
            _wireBody = assembled.GetBuffer();
            return;
        }
        if (_formFields is not null)
        {
            _wireBody = EncodeFormBody(_formFields, config, out var written)
                ?? throw RefuseCeiling("the urlencoded body", written, config.MaxRequestBytes,
                    "maxRequestBytes", exact: false);
            _wireContentType = FormContentType;
            return;
        }
        if (Transport.CarriesABody(method, _jsonBody, _formData, _files))
            _wireBody = Array.Empty<byte>();
    }

    private const string FormContentType = "application/x-www-form-urlencoded";

    private void JudgeSourceCeiling(SdkConfig config)
    {
        var source = 0L;
        if (_jsonBodyBytes is { } encoded)
            source = Add(source, encoded.LongLength);
        if (_formFields is not null)
        {
            foreach (var (name, value) in _formFields)
                source = Add(source,
                    Encoding.UTF8.GetByteCount(name) + (long)Encoding.UTF8.GetByteCount(value));
        }
        if (_files is not null)
        {
            foreach (var (_, value) in _files)
                source = Add(source, PartSourceBytes(value));
        }
        if (source > config.MaxRequestSourceBytes)
            throw RefuseCeiling("the payload", source, config.MaxRequestSourceBytes,
                "maxRequestSourceBytes", exact: true);
        _sourceBytes = source;
    }

    private void JudgeFramingCeiling(SdkConfig config, long framing)
    {
        if (framing > config.MaxMultipartOverheadBytes)
            throw RefuseCeiling("the multipart framing", framing, config.MaxMultipartOverheadBytes,
                "maxMultipartOverheadBytes", exact: true);
    }

    private static void JudgeFinalCeiling(SdkConfig config, long body)
    {
        if (body > config.MaxRequestBytes)
            throw RefuseCeiling("the request body", body, config.MaxRequestBytes,
                "maxRequestBytes", exact: true);
    }

    private long PartSourceBytes(object? value) => value switch
    {
        byte[] bytes => bytes.LongLength,
        IReadOnlyList<object?> { Count: >= 2 } tuple => tuple[1] switch
        {
            byte[] bytes => bytes.LongLength,
            string text => Encoding.UTF8.GetByteCount(text),
            _ => 0L,
        },
        Stream { CanSeek: true } stream => Math.Max(0L, stream.Length - StreamOriginOf(stream)),
        _ => 0L,
    };

    // Saturating: an overflowed negative total would pass every ceiling.
    private static long Add(long total, long more)
    {
        var sum = unchecked(total + more);
        return sum < total ? long.MaxValue : sum;
    }

    // Sizes and setting names only: the payload is caller data, and this message is logged.
    private static SdkUsageError RefuseCeiling(
        string what, long measured, long ceiling, string setting, bool exact) =>
        new(Redaction.RedactMessageText(
            what + (exact ? " is " : " had already reached ")
            + measured.ToString(CultureInfo.InvariantCulture)
            + " bytes, past the " + ceiling.ToString(CultureInfo.InvariantCulture)
            + "-byte " + setting + " ceiling, so nothing was sent - send less, or raise "
            + setting + " on the client"));

    private static byte[]? EncodeFormBody(
        IReadOnlyDictionary<string, string> fields, SdkConfig config, out long written)
    {
        var ceiling = config.MaxRequestBytes;
        var buffer = new MemoryStream();
        var first = true;
        foreach (var (name, value) in fields)
        {
            if (!first)
                buffer.WriteByte((byte)'&');
            first = false;
            WriteFormToken(buffer, name);
            if (buffer.Length <= ceiling)
            {
                buffer.WriteByte((byte)'=');
                WriteFormToken(buffer, value);
            }
            if (buffer.Length > ceiling)
            {
                written = buffer.Length;
                return null;
            }
        }
        written = buffer.Length;
        return buffer.ToArray();
    }

    private static void WriteFormToken(MemoryStream into, string text)
    {
        if (text.Length == 0)
            return;
        var escaped = Uri.EscapeDataString(text).Replace("%20", "+", StringComparison.Ordinal);
        foreach (var character in escaped)
            into.WriteByte((byte)character);
    }

    internal string UrlWithQuery => _urlWithQuery ??= Transport.BuildUrl(Url, _queryParams);

    internal IReadOnlyDictionary<string, object?>? QueryParamsAsSent => _queryParams;

    internal IReadOnlyDictionary<string, string>? FormFieldsAsSent => _formFields;

    internal IReadOnlyDictionary<string, object?>? FilesAsSent => _files;

    internal object? RequestInfoBodyAsRecorded => _requestInfoBody;

    internal long StreamOriginOf(Stream stream) =>
        _streamOrigins is not null && _streamOrigins.TryGetValue(stream, out var origin) ? origin : 0L;

    private static IReadOnlyDictionary<object, long>? RecordStreamOrigins(
        IReadOnlyDictionary<string, object?>? files)
    {
        if (files is null)
            return null;
        Dictionary<object, long>? origins = null;
        foreach (var (_, value) in files)
            if (value is Stream { CanSeek: true } stream)
                (origins ??= new Dictionary<object, long>(ReferenceEqualityComparer.Instance))[stream] =
                    stream.Position;
        return origins;
    }

    private static (IReadOnlyDictionary<string, object?>? Recorded, IReadOnlyDictionary<string, string>? Sent)
        NormaliseFormData(IReadOnlyDictionary<string, object?>? source)
    {
        if (source is null)
            return (null, null);
        var recorded = new List<KeyValuePair<string, object?>>();
        var sent = new List<KeyValuePair<string, string>>();
        foreach (var (name, value) in source)
        {
            var text = FormFieldText(name, value);
            recorded.Add(new KeyValuePair<string, object?>(name, text));
            sent.Add(new KeyValuePair<string, string>(name, text));
        }
        return (Redaction.DetachedMapOf(recorded), Redaction.DetachedTextOf(sent));
    }

    private static string FormFieldText(string name, object? value)
    {
        if (value is null)
            return "";
        if (WireText(value) is { } text)
            return text;
        if (value is IEnumerable)
            throw new SdkUsageError(Redaction.RedactMessageText(
                $"formData '{name}': a form field holds one value, and this map can only name "
                + "each field once, so a list has no form-encoded spelling here. Send the one "
                + "value the field takes, or join the list yourself into the separated string "
                + "the endpoint documents"));
        throw new SdkUsageError(Redaction.RedactMessageText(RefuseValue("formData", name, value)));
    }

    private readonly IReadOnlyDictionary<string, object?>? _queryParams;
    private string? _urlWithQuery;
    private readonly object? _jsonBody;
    private readonly byte[]? _jsonBodyBytes;
    private readonly IReadOnlyDictionary<string, object?>? _formData;
    private readonly IReadOnlyDictionary<string, string>? _formFields;
    private IReadOnlyDictionary<string, object?>? _files;
    private string? _multipartBoundary;
    private bool _prepared;
    private bool _replaySafe = true;
    private byte[]? _wireBody;
    private long _sourceBytes;
    private string? _wireContentType;
    private readonly IReadOnlyDictionary<object, long>? _streamOrigins;
    private readonly IReadOnlyDictionary<string, string>? _headers;
    private readonly IReadOnlyDictionary<string, string>? _requestInfoHeaders;
    private readonly object? _requestInfoBody;
}
