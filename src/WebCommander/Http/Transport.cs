using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

using WebCommander.Client;
using WebCommander.Config;
using WebCommander.Errors;

namespace WebCommander.Http;

/// <summary>HTTP execution over <see cref="HttpClient"/>: timeouts, TLS, proxy and retries; no tokens.</summary>
public sealed class Transport : IDisposable
{
    internal const string JsonContentType = "application/json; charset=utf-8";

    // Methods the stale-echo workaround may resend: never anything with an effect.
    private static readonly IReadOnlySet<string> ResendableMethods = new HashSet<string> { "GET", "HEAD", "OPTIONS" };

    private const string UnknownParameterError = "unknown_parameter";
    private static readonly Regex ParameterName = new("[A-Za-z0-9_-]+", RegexOptions.Compiled);

    private readonly SdkConfig _config;
    private readonly RetryPolicy _retryPolicy;
    private readonly Action<string> _onProgress;
    private readonly HttpClient _client;

    public Transport(SdkConfig config, RetryPolicy? retryPolicy = null, Action<string>? onProgress = null)
    {
        _config = config;
        _retryPolicy = retryPolicy ?? RetryPolicy.FromConfig(config);
        _onProgress = onProgress ?? (_ => { });
        _client = new HttpClient(CreateHandler(config), disposeHandler: true)
        {
            // Timed per attempt instead: under ResponseHeadersRead this would leave the body read unbounded.
            Timeout = Timeout.InfiniteTimeSpan,
        };
    }

    internal static SocketsHttpHandler CreateHandler(SdkConfig config)
    {
        var handler = new SocketsHttpHandler
        {
            // A 3xx is returned, never followed with the token attached.
            AllowAutoRedirect = false,
            ConnectTimeout = FromSeconds(config.ConnectTimeoutSeconds),
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            AutomaticDecompression = DecompressionMethods.All,
        };
        if (!string.IsNullOrWhiteSpace(config.ProxyUrl))
        {
            handler.Proxy = new WebProxy(config.ProxyUrl);
            handler.UseProxy = true;
        }
        if (!string.IsNullOrWhiteSpace(config.CaBundle))
            ApplyCaBundle(handler, config.CaBundle!, config.MaxCaBundleBytes);
        else if (!config.VerifySsl)
            handler.SslOptions.RemoteCertificateValidationCallback = (_, _, _, _) => true;
        return handler;
    }

    /// <summary>Send one request with retries; every status comes back as an <see cref="ApiResponse"/>.</summary>
    public async Task<ApiResponse> SendHttpRequestAsync(
        TransportRequest request, CancellationToken cancellationToken = default)
    {
        RefuseIfDisposed();
        var method = NormaliseMethod(request.HttpMethod);
        CheckRequestHeaders(request.Headers);
        var url = request.UrlWithQuery;
        await request.PrepareAsync(_config, method, cancellationToken).ConfigureAwait(false);
        var maxAttempts = request.ReplaySafe ? _retryPolicy.MaxAttempts : 1;
        var started = Stopwatch.GetTimestamp();
        var attempt = 0;
        var requestsSent = 0;
        var staleEchoResent = false;
        Exception? lastException = null;

        while (attempt < maxAttempts)
        {
            attempt++;
            requestsSent++;
            // UrlForLog, not RedactText: progress names the call and withholds every query value.
            _onProgress($"{method} {Redaction.UrlForLog(url)} sending (attempt {attempt}/{maxAttempts})");

            using var ceiling = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            ceiling.CancelAfter(FromSeconds(_config.TimeoutSeconds));

            HttpResponseMessage raw;
            try
            {
                using var message = BuildRequestMessage(method, url, request);
                // ResponseHeadersRead, so no body is buffered before MaxResponseBytes is checked.
                raw = await _client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ceiling.Token)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (ObjectDisposedException error)
            {
                throw new SdkUsageError(Redaction.RedactMessageText("this Transport was disposed while a "
                    + $"{method} was in flight - a transport's lifetime is the lifetime of its "
                    + "connection pool, so close it only once nothing is using it"), error);
            }
            catch (Exception exception) when (exception is HttpRequestException or OperationCanceledException)
            {
                lastException = AsAttemptFailure(exception, ceiling);
                if (!await BackOffAfterExceptionAsync(lastException, method, attempt, maxAttempts,
                        requestFullySent: false, cancellationToken).ConfigureAwait(false))
                {
                    break;
                }
                continue;
            }

            using (raw)
            {
                var status = (int)raw.StatusCode;
                byte[] bodyBytes;
                try
                {
                    bodyBytes = await ReadBodyWithinCapAsync(raw, ceiling.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (ResponseOverCap refusal)
                {
                    throw TooLarge(refusal, method, url,
                        RequestInfoFor(method, url, request, requestsSent,
                            GrantedAttempts(maxAttempts, staleEchoResent), started));
                }
                catch (Exception exception) when (exception is HttpRequestException or IOException
                                                     or OperationCanceledException)
                {
                    lastException = AsAttemptFailure(exception, ceiling);
                    if (!await BackOffAfterExceptionAsync(lastException, method, attempt, maxAttempts,
                            requestFullySent: true, cancellationToken).ConfigureAwait(false))
                    {
                        break;
                    }
                    continue;
                }
                var contentType = raw.Content.Headers.ContentType?.ToString();

                if (attempt < maxAttempts && _retryPolicy.ShouldRetryStatusCode(status, method))
                {
                    await DelaySecondsAsync(
                        _retryPolicy.ComputeBackoffSeconds(attempt, RetryAfter(raw)), cancellationToken)
                        .ConfigureAwait(false);
                    continue;
                }

                var decoded = DecodeResponseBody(status, bodyBytes, contentType);
                var response = BuildResponse(raw, decoded, bodyBytes, contentType, method, url, request,
                    requestsSent, GrantedAttempts(maxAttempts, staleEchoResent), started);

                if (!staleEchoResent
                    && request.ReplaySafe
                    && ResendableMethods.Contains(method)
                    && DetectStaleParameterEcho(response, request.QueryParamsAsSent))
                {
                    staleEchoResent = true;
                    attempt--;
                    continue;
                }
                return response;
            }
        }

        var timedOut = IsTimeout(lastException);
        throw new SdkError(Redaction.RedactMessageText(
            $"{method} {Redaction.UrlForLog(url)}{(timedOut ? " timed out" : " failed")} after "
                + $"{requestsSent} attempt(s): {Redaction.RedactText(Convert.ToString(lastException, CultureInfo.InvariantCulture) ?? "unknown")}"),
            null,
            timedOut ? ErrorType.Timeout : ErrorType.Network,
            null,
            null,
            RequestInfoFor(method, url, request, requestsSent, GrantedAttempts(maxAttempts, staleEchoResent), started));
    }

    private async Task<bool> BackOffAfterExceptionAsync(
        Exception failure, string method, int attempt, int maxAttempts, bool requestFullySent,
        CancellationToken cancellationToken)
    {
        if (attempt >= maxAttempts
            || !_retryPolicy.ShouldRetryAfterException(failure, method, requestFullySent))
        {
            return false;
        }
        await DelaySecondsAsync(_retryPolicy.ComputeBackoffSeconds(attempt, null), cancellationToken)
            .ConfigureAwait(false);
        return true;
    }

    /// <summary>Release the connection pool; idempotent, and releases only what this transport owns.</summary>
    public void Dispose()
    {
        // Set first, so a racing send sees the closure rather than a half-torn-down client.
        _disposed = true;
        _client.Dispose();
    }

    private volatile bool _disposed;

    private void RefuseIfDisposed()
    {
        if (_disposed)
            throw new SdkUsageError("this Transport has been disposed and cannot "
                + "send - a transport's lifetime is the lifetime of its connection pool, so build "
                + "a new one rather than reusing a closed one");
    }

    internal static string NormaliseMethod(string? httpMethod)
    {
        if (string.IsNullOrWhiteSpace(httpMethod))
            throw new SdkUsageError("httpMethod must name a verb");
        var method = httpMethod.Trim().ToUpperInvariant();
        // Checked here: HttpMethod's constructor throws a bare FormatException outside SdkError.
        if (!IsHttpToken(method))
            // Neutralised, not echoed raw: the verb may carry a CR/LF, and this message is logged.
            throw new SdkUsageError(Redaction.RedactMessageText(
                "httpMethod must be a single verb with no spaces or control characters, got "
                + $"'{ExchangeConsole.NeutraliseOneLine(method)}'"));
        return method;
    }

    private static bool IsHttpToken(string value)
    {
        foreach (var character in value)
        {
            var allowed = character is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z')
                or (>= '0' and <= '9')
                or '!' or '#' or '$' or '%' or '&' or '\'' or '*'
                or '+' or '-' or '.' or '^' or '_' or '`' or '|' or '~';
            if (!allowed)
                return false;
        }
        return true;
    }

    internal static void CheckAssembledUrl(TransportRequest request)
    {
        var assembled = request.UrlWithQuery;
        if (Uri.TryCreate(assembled, UriKind.Absolute, out _))
            return;
        throw UrlTooLong(assembled);
    }

    private static SdkUsageError UrlTooLong(string url, Exception? inner = null) => new(
        Redaction.RedactMessageText(
            $"the assembled URL is {url.Length.ToString(CultureInfo.InvariantCulture)} "
            + "characters, which is more than a URL may hold - shorten the query values, "
            + "or send the filter in a body instead"),
        inner);

    internal static string BuildUrl(string url, IReadOnlyDictionary<string, object?>? queryParams)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var parsed)
            || (parsed.Scheme != Uri.UriSchemeHttp && parsed.Scheme != Uri.UriSchemeHttps))
        {
            throw new SdkUsageError(Redaction.RedactMessageText("not a valid http(s) URL: " + Redaction.RedactText(url)));
        }
        UrlBuilder.RejectAFragment(url);
        if (queryParams is null || queryParams.Count == 0)
            return url;

        var builder = new StringBuilder(url);
        var first = !url.Contains('?', StringComparison.Ordinal);
        foreach (var (key, value) in queryParams)
        {
            foreach (var single in ExpandValue(value))
            {
                builder.Append(first ? '?' : '&');
                first = false;
                builder.Append(Uri.EscapeDataString(key)).Append('=').Append(Uri.EscapeDataString(single));
            }
        }
        return builder.ToString();
    }

    private static IEnumerable<string> ExpandValue(object? value)
    {
        if (value is string text)
        {
            yield return text;
            yield break;
        }
        if (value is System.Collections.IEnumerable sequence)
        {
            foreach (var item in sequence)
                yield return Stringify(item);
            yield break;
        }
        yield return Stringify(value);
    }

    internal static string Stringify(object? value) =>
        value switch
        {
            null => "",
            string text => text,
            IFormattable formattable =>
                formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? "",
        };

    private HttpRequestMessage BuildRequestMessage(string method, string url, TransportRequest request)
    {
        HttpRequestMessage message;
        try
        {
            message = new HttpRequestMessage(new HttpMethod(method), url)
            {
                Content = BuildContent(request),
            };
        }
        catch (UriFormatException error)
        {
            throw UrlTooLong(url, error);
        }
        if (request.Headers is not null)
        {
            foreach (var (name, value) in request.Headers)
                AddHeader(message, name, value);
        }
        return message;
    }

    private static void CheckRequestHeaders(IReadOnlyDictionary<string, string>? headers)
    {
        if (headers is null)
            return;
        foreach (var (name, value) in headers)
        {
            if (!IsSendableHeader(name, value))
                RefuseHeader(name);
        }
    }

    internal static bool IsSendableHeader(string name, string? value)
    {
        if (string.IsNullOrEmpty(name) || !IsHttpToken(name))
            return false;
        if (value is null)
            return false;
        if (value.Length == 0)
            return true;
        if (!IsFieldVisible(value[0]) || !IsFieldVisible(value[^1]))
            return false;
        foreach (var character in value)
            if (character is not ' ' and not '\t' && !IsFieldVisible(character))
                return false;
        return true;
    }

    private static bool IsFieldVisible(char character) => character is >= '!' and <= '~';

    private static void RefuseHeader(string name) =>
        throw new SdkUsageError(Redaction.RedactMessageText(
            $"header '{ExchangeConsole.NeutraliseOneLine(name)}': not something this transport "
            + "can send - " + HeaderGrammar + ". The value is not shown: it may be a credential"));

    internal const string HeaderGrammar =
        "a blank name, a name carrying anything outside an HTTP token, or a value carrying a "
        + "byte outside printable ASCII (a carriage return, a newline, a NUL, any other "
        + "control, DEL or a non-ASCII byte) or a leading or trailing space or tab";

    private static void AddHeader(HttpRequestMessage message, string name, string value)
    {
        if (IsContentHeader(name))
        {
            if (message.Content is null)
                return;
            if (string.Equals(name, "Content-Type", StringComparison.OrdinalIgnoreCase))
            {
                if (IsMultipartBody(message.Content))
                    return;
                message.Content.Headers.Remove("Content-Type");
            }
            message.Content.Headers.TryAddWithoutValidation(name, value);
        }
        else if (!message.Headers.TryAddWithoutValidation(name, value))
        {
            RefuseHeader(name);
        }
    }

    private static bool IsMultipartBody(HttpContent content) =>
        content.Headers.TryGetValues("Content-Type", out var declared)
        && declared.FirstOrDefault() is { } value
        && value.StartsWith("multipart/", StringComparison.OrdinalIgnoreCase);

    internal static bool IsContentHeader(string name) =>
        name.StartsWith("Content-", StringComparison.OrdinalIgnoreCase);

    internal static bool CarriesABody(
        string method,
        object? jsonBody,
        IReadOnlyDictionary<string, object?>? formData,
        IReadOnlyDictionary<string, object?>? files) =>
        jsonBody is not null
        || files is { Count: > 0 }
        || formData is not null
        || method is "POST" or "PUT" or "PATCH";

    internal static byte[]? EncodeJsonBody(object? jsonBody)
    {
        switch (jsonBody)
        {
            case null:
                return null;
            case Stream:
                throw new SdkUsageError(Redaction.RedactMessageText("jsonBody: a raw "
                    + jsonBody.GetType().Name + " cannot be replayed on a retry, "
                    + "so read it and pass the value it holds"));
            case string jsonText:
                return Encoding.UTF8.GetBytes(jsonText);
        }
        try
        {
            return JsonSerializer.SerializeToUtf8Bytes(jsonBody);
        }
        // InvalidOperationException too: the serialiser raises it for some refusals.
        catch (Exception error) when (error is JsonException
                                         or NotSupportedException
                                         or InvalidOperationException)
        {
            throw new SdkUsageError(
                Redaction.RedactMessageText("jsonBody is not JSON-serialisable: " + error.Message), error);
        }
    }

    // 128 random bits, not a Guid: a predictable boundary can be planted inside a part.
    internal static string NewMultipartBoundary() =>
        "wc-boundary-" + Convert.ToHexString(
            System.Security.Cryptography.RandomNumberGenerator.GetBytes(16)).ToLowerInvariant();

    private static HttpContent? BuildContent(TransportRequest request)
    {
        if (request.WireBody is { } settled)
        {
            var content = new ByteArrayContent(settled);
            if (request.WireContentType is { } declared)
                content.Headers.TryAddWithoutValidation("Content-Type", declared);
            return content;
        }
        if (request.FilesAsSent is { Count: > 0 })
            return BuildMultipartContent(request, request.MultipartBoundary!);
        return null;
    }

    // Not MultipartFormDataContent: it RFC 2047-encodes a non-ASCII filename, losing its extension.
    internal static MultipartContent BuildMultipartContent(TransportRequest request, string boundary)
    {
        var multipart = new MultipartContent("form-data", boundary);
        if (request.FilesAsSent is not null)
        {
            foreach (var (field, value) in request.FilesAsSent)
                AddFilePart(multipart, field, value, request);
        }
        if (request.FormFieldsAsSent is not null)
        {
            foreach (var (name, value) in request.FormFieldsAsSent)
            {
                // The name reaches a part header, so it is held to the file part's rule.
                if (ProblemWithMultipartFieldName(name) is { } refusal)
                    throw refusal;
                var field = new StringContent(value);
                AttachPartDisposition(field, name, null);
                multipart.Add(field);
            }
        }
        return multipart;
    }

    private static void AddFilePart(
        MultipartContent multipart, string field, object? value, TransportRequest request)
    {
        switch (value)
        {
            case byte[] bytes:
                AddPart(multipart, new ByteArrayContent(bytes), field, field);
                break;
            case IReadOnlyList<object?> tuple when tuple.Count >= 2
                    && tuple[0] is string filename && TuplePartBody(tuple[1]) is { } tupleBytes:
                var part = new ByteArrayContent(tupleBytes);
                if (DeclaredPartContentType(tuple) is { } declared)
                    part.Headers.ContentType = MediaTypeHeaderValue.Parse(declared);
                AddPart(multipart, part, field, filename);
                break;
            case Stream stream when stream.CanSeek:
                stream.Position = request.StreamOriginOf(stream);
                AddPart(multipart, new CallerOwnedStreamContent(stream), field,
                    StreamPartFilename(field, stream));
                break;
        }
    }

    internal static string StreamPartFilename(string field, Stream stream) =>
        stream is FileStream fileStream ? Path.GetFileName(fileStream.Name) : field;

    private static void AddPart(MultipartContent multipart, HttpContent part, string field, string? filename)
    {
        AttachPartDisposition(part, field, filename);
        multipart.Add(part);
    }

    // Written unvalidated: the typed header throws on a quote and RFC 2047-encodes the filename.
    private static void AttachPartDisposition(HttpContent part, string field, string? filename)
    {
        var disposition = "form-data; name=\"" + EscapePartParameter(field) + "\"";
        if (filename is not null)
            disposition += "; filename=\"" + EscapePartParameter(filename) + "\"";
        part.Headers.TryAddWithoutValidation("Content-Disposition", Utf8OnTheWire(disposition));
    }

    // Percent form, not backslash: servers following WHATWG do not honour a backslash escape.
    private static string EscapePartParameter(string value) =>
        value.Replace("\\", "%5C", StringComparison.Ordinal)
            .Replace("\"", "%22", StringComparison.Ordinal);

    // Part headers are written as Latin-1; this puts raw UTF-8 on the wire instead of '?'.
    private static string Utf8OnTheWire(string value) =>
        Encoding.Latin1.GetString(Encoding.UTF8.GetBytes(value));

    private static SdkUsageError? ProblemWithPartLabel(string what, string value)
    {
        for (var index = 0; index < value.Length; index++)
        {
            var found = value[index] switch
            {
                '\r' => "a carriage return",
                '\n' => "a newline",
                '\0' => "a NUL",
                _ => null,
            };
            if (found is null)
                continue;
            return new SdkUsageError(Redaction.RedactMessageText(
                $"files: {what} carries {found} at character "
                + index.ToString(CultureInfo.InvariantCulture)
                + ", which would forge a header on the multipart part this SDK sends. The value "
                + "is not shown: it may be attacker-chosen. Strip CR, LF and NUL from an upload "
                + "filename before passing it on"));
        }
        return null;
    }

    private static byte[]? TuplePartBody(object? contents) => contents switch
    {
        byte[] bytes => bytes,
        string text => Encoding.UTF8.GetBytes(text),
        _ => null,
    };

    internal static bool IsSendableFilePart(object? value)
    {
        if (value is byte[])
            return true;
        if (value is IReadOnlyList<object?> tuple && tuple.Count >= 2
            && tuple[0] is string && tuple[1] is byte[] or string)
        {
            return true;
        }
        return value is Stream stream && stream.CanSeek;
    }

    internal static SdkUsageError? ProblemWithFilePart(string field, object? value)
    {
        if (!IsSendableFilePart(value))
            return RefuseFilePart(field, value);
        if (ProblemWithPartLabel(
                "a part name of " + field.Length.ToString(CultureInfo.InvariantCulture)
                + " character(s)", field) is { } badName)
        {
            return badName;
        }
        if (PartFilename(field, value) is { } filename
            && ProblemWithPartLabel($"the filename on part '{field}'", filename) is { } badFilename)
        {
            return badFilename;
        }
        if (value is IReadOnlyList<object?> tuple && DeclaredPartContentType(tuple) is { } declared)
        {
            if (ProblemWithPartLabel($"the content type on part '{field}'", declared) is { } badType)
                return badType;
            if (!MediaTypeHeaderValue.TryParse(declared, out _))
                return RefuseFilePartContentType(field, declared);
        }
        return null;
    }

    internal static SdkUsageError? ProblemWithMultipartFieldName(string name) =>
        ProblemWithPartLabel(
            "a formData name of " + name.Length.ToString(CultureInfo.InvariantCulture)
            + " character(s)", name);

    private static string? PartFilename(string field, object? value) => value switch
    {
        byte[] => field,
        IReadOnlyList<object?> tuple when tuple.Count >= 2 && tuple[0] is string filename => filename,
        Stream stream => StreamPartFilename(field, stream),
        _ => null,
    };

    private static string? DeclaredPartContentType(IReadOnlyList<object?> tuple) =>
        tuple.Count > 2 && tuple[2] is string declared ? declared : null;

    internal static SdkUsageError RefuseFilePartContentType(string field, string declared) =>
        new(Redaction.RedactMessageText(
            $"files: '{field}' declares the content type "
            + $"'{ExchangeConsole.NeutraliseOneLine(declared)}', which is not a media type this "
            + "transport can put on the part - it needs the `type/subtype` form, optionally with "
            + "parameters, as in \"text/csv\" or \"text/csv; charset=utf-8\". Leave the third "
            + "element off to let the multipart encoder choose"));

    internal static SdkUsageError RefuseFilePart(string field, object? value) =>
        new(Redaction.RedactMessageText(value is string
            ? $"files: '{field}' is a bare string. A string IS the part's content here, never a "
            + "path to open - but it carries no filename, so the part goes out named after the "
            + "field and reaches the store with no file extension, which the import refuses with "
            + "400 unsupported_file_type ('file must be .xlsx, .xls or .csv'). Naming the field "
            + "for the file instead breaks the other rule: the part name must be exactly 'file'. "
            + "Send the same text with a name: [\"customers.csv\", text, \"text/csv\"]"
            : $"files: '{field}' must be byte[], a seekable Stream, or "
            + "[filename, byte[] or string, contentType?] - a non-seekable stream cannot be "
            + "rewound and replayed on a retry, so buffer it first"));

    private sealed class CallerOwnedStreamContent : StreamContent
    {
        public CallerOwnedStreamContent(Stream stream) : base(stream) { }

        protected override void Dispose(bool disposing)
        {
        }
    }

    private ApiResponse BuildResponse(
        HttpResponseMessage raw, object? decoded, byte[] bodyBytes, string? contentType, string method,
        string url, TransportRequest request, int attempt, int maxAttempts, long started)
    {
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (name, values) in raw.Headers)
            headers[name] = string.Join(", ", values);
        foreach (var (name, values) in raw.Content.Headers)
            headers[name] = string.Join(", ", values);
        return new ApiResponse((int)raw.StatusCode, decoded, headers, contentType,
            RequestInfoFor(method, url, request, attempt, maxAttempts, started),
            rawBody: bodyBytes, responseFormat: request.ResponseFormat, dtoType: request.DtoType);
    }

    private RequestInfo RequestInfoFor(
        string method, string url, TransportRequest request, int attempt, int maxAttempts, long started) =>
        new(
            _config.ClientName,
            _config.ApiVersion,
            method,
            url,
            request.QueryParamsAsSent,
            request.RequestInfoBodyAsRecorded,
            request.RequestInfoHeaders ?? new Dictionary<string, string>(),
            attempt,
            maxAttempts,
            Math.Round(Stopwatch.GetElapsedTime(started).TotalSeconds, 4));

    private static int GrantedAttempts(int maxAttempts, bool staleEchoResent) =>
        maxAttempts + (staleEchoResent ? 1 : 0);

    /// <summary>Decode a body by Content-Type: JSON, <c>text/*</c> or bytes; a 204 is <c>null</c>.</summary>
    public static object? DecodeResponseBody(int status, byte[] body, string? contentType)
    {
        if (status == 204 || body.Length == 0)
            return null;
        var lowered = (contentType ?? "").ToLowerInvariant();
        var mediaType = lowered.Split(';', 2)[0].Trim();
        if (IsJsonMediaType(mediaType))
        {
            try
            {
                var start = HasUtf8Bom(body) ? 3 : 0;
                using var document = JsonDocument.Parse(body.AsMemory(start), Redaction.DeepParse);
                return Redaction.Materialise(document.RootElement);
            }
            catch (JsonException)
            {
                return Encoding.UTF8.GetString(body);
            }
        }
        if (mediaType.StartsWith("text/", StringComparison.Ordinal))
        {
            var declared = DeclaredCharset(lowered);
            if (declared is not null)
                return declared.GetString(body);
            return DecodeUtf8OrLatin1(body);
        }
        return body;
    }

    // Exact, not a prefix: application/json-seq is a stream of documents, not one.
    internal static bool IsJsonMediaType(string mediaType) =>
        mediaType == MediaType.JsonMediaType
        || mediaType.EndsWith("+json", StringComparison.Ordinal);

    // Strict UTF-8 first: a lenient decode would turn Latin-1 bytes into replacement characters.
    private static string DecodeUtf8OrLatin1(byte[] body)
    {
        try
        {
            return StrictUtf8.GetString(body);
        }
        catch (DecoderFallbackException)
        {
            return Encoding.Latin1.GetString(body);
        }
    }

    private static readonly Encoding StrictUtf8 = new UTF8Encoding(
        encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    private static bool HasUtf8Bom(byte[] body) =>
        body.Length >= 3 && body[0] == 0xEF && body[1] == 0xBB && body[2] == 0xBF;

    private static Encoding? DeclaredCharset(string loweredContentType)
    {
        var marker = loweredContentType.IndexOf("charset=", StringComparison.Ordinal);
        if (marker < 0)
            return null;
        var name = loweredContentType[(marker + "charset=".Length)..].Split(';', ' ', '\t')[0].Replace("\"", "");
        try
        {
            return Encoding.GetEncoding(name);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    // A 400 naming only fields this request never sent belongs to an earlier request on the same keep-alive connection.
    internal static bool DetectStaleParameterEcho(ApiResponse response, IReadOnlyDictionary<string, object?>? sentParams)
    {
        if (response.StatusCode != 400 || response.Map is not IReadOnlyDictionary<string, object?> body)
            return false;
        var namedFields = ExtractErrorFields(body);
        if (namedFields.Count == 0 && Equals(body.GetValueOrDefault("error"), UnknownParameterError))
            namedFields = ParseUnknownParameterMessage(body);
        if (namedFields.Count == 0)
            return false;
        var sent = new HashSet<string>();
        if (sentParams is not null)
            foreach (var name in sentParams.Keys)
                sent.Add(name.ToLowerInvariant());
        if (sent.Count == 0)
            return false;
        return !namedFields.Overlaps(sent);
    }

    private static HashSet<string> ExtractErrorFields(IReadOnlyDictionary<string, object?> body)
    {
        var fields = new HashSet<string>();
        var entries = body.GetValueOrDefault("errors");
        if (entries is IReadOnlyDictionary<string, object?> byField)
        {
            // Checked before the sequence branch, because a mapping is also IEnumerable.
            foreach (var fieldName in byField.Keys)
            {
                var name = fieldName.Trim().ToLowerInvariant();
                if (name.Length > 0)
                    fields.Add(name);
            }
        }
        else if (entries is System.Collections.IEnumerable list and not string)
        {
            foreach (var entry in list)
                if (entry is IReadOnlyDictionary<string, object?> item
                    && item.GetValueOrDefault("field") is { } field)
                {
                    fields.Add(Stringify(field).Trim().ToLowerInvariant());
                }
        }
        return fields;
    }

    private static HashSet<string> ParseUnknownParameterMessage(IReadOnlyDictionary<string, object?> body)
    {
        var fields = new HashSet<string>();
        var message = Stringify(body.GetValueOrDefault("message"));
        const string marker = "unknown query parameter(s):";
        var at = message.IndexOf(marker, StringComparison.Ordinal);
        if (at >= 0)
        {
            foreach (var listed in message[(at + marker.Length)..].Split(','))
            {
                var named = ParameterName.Match(listed);
                if (named.Success)
                    fields.Add(named.Value.ToLowerInvariant());
            }
        }
        return fields;
    }

    private static void ApplyCaBundle(SocketsHttpHandler handler, string pemPath, long maxBytes)
    {
        RefuseAnOversizedCaBundle(pemPath, maxBytes);
        X509Certificate2Collection roots = new();
        try
        {
            roots.ImportFromPemFile(pemPath);
        }
        catch (Exception error) when (error is IOException or System.Security.Cryptography.CryptographicException
            or UnauthorizedAccessException or ArgumentException)
        {
            throw new SdkConfigError(Redaction.RedactMessageText($"caBundle: {pemPath} could not be loaded: {error.Message}"), error);
        }
        if (roots.Count == 0)
            throw new SdkConfigError(Redaction.RedactMessageText($"caBundle: {pemPath} contains no PEM certificates, so the "
                + "trust store would be empty and every TLS handshake would fail - point it at a "
                + "PEM file holding at least one CA certificate"));
        handler.SslOptions.RemoteCertificateValidationCallback = (_, certificate, presentedChain, errors) =>
        {
            if ((errors & ~SslPolicyErrors.RemoteCertificateChainErrors) != 0)
                return false;
            if (certificate is null)
                return false;
            using var chain = new X509Chain();
            chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
            chain.ChainPolicy.CustomTrustStore.AddRange(roots);
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            if (presentedChain is not null)
                for (var index = 1; index < presentedChain.ChainElements.Count; index++)
                    chain.ChainPolicy.ExtraStore.Add(presentedChain.ChainElements[index].Certificate);
            using var leaf = new X509Certificate2(certificate);
            return chain.Build(leaf);
        };
    }

    internal static void RefuseAnOversizedCaBundle(string pemPath, long maxBytes)
    {
        long size;
        try
        {
            var entry = new FileInfo(pemPath);
            if (!entry.Exists)
                return;
            size = entry.Length;
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException
                                          or ArgumentException or NotSupportedException)
        {
            return;
        }
        if (size <= maxBytes)
            return;
        throw new SdkConfigError(Redaction.RedactMessageText(
            $"caBundle: {pemPath} is {size.ToString(CultureInfo.InvariantCulture)} bytes, past the "
            + maxBytes.ToString(CultureInfo.InvariantCulture)
            + "-byte maxCaBundleBytes ceiling - the whole file is read and parsed into a trust "
            + "store, so it is refused rather than imported. Point caBundle at the roots this "
            + "client needs, or raise maxCaBundleBytes"));
    }

    private static string? RetryAfter(HttpResponseMessage response) =>
        response.Headers.TryGetValues("Retry-After", out var values) ? values.FirstOrDefault() : null;

    private Exception AsAttemptFailure(Exception exception, CancellationTokenSource ceiling) =>
        exception is OperationCanceledException && ceiling.IsCancellationRequested
            ? new TimeoutException(
                "the attempt exceeded the timeoutSeconds ceiling of "
                + _config.TimeoutSeconds.ToString(CultureInfo.InvariantCulture) + " second(s)",
                exception)
            : exception;

    private static bool IsTimeout(Exception? exception)
    {
        for (var link = exception; link is not null; link = link.InnerException)
            if (link is TimeoutException or TaskCanceledException)
                return true;
        return false;
    }

    private static async Task DelaySecondsAsync(double seconds, CancellationToken cancellationToken)
    {
        if (!(seconds > 0))
            return;
        await Task.Delay(FromSeconds(seconds), cancellationToken).ConfigureAwait(false);
    }

    private static TimeSpan FromSeconds(double seconds) => TimeSpan.FromMilliseconds(seconds * 1000);

    private async Task<byte[]> ReadBodyWithinCapAsync(
        HttpResponseMessage raw,
        CancellationToken cancellationToken)
    {
        var cap = _config.MaxResponseBytes;
        var declared = raw.Content.Headers.ContentLength;
        if (declared > cap)
        {
            throw new ResponseOverCap(cap, declared);
        }

        await using var stream = await raw.Content
            .ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);
        using var buffer = new MemoryStream();
        var block = new byte[81920];
        long total = 0;
        while (true)
        {
            var wanted = (int)Math.Min(block.Length, cap - total + 1);
            var read = await stream
                .ReadAsync(block.AsMemory(0, wanted), cancellationToken)
                .ConfigureAwait(false);
            if (read <= 0)
                break;
            total += read;
            if (total > cap)
            {
                throw new ResponseOverCap(cap, declared);
            }

            buffer.Write(block, 0, read);
        }

        return buffer.ToArray();
    }

    private sealed class ResponseOverCap : Exception
    {
        internal ResponseOverCap(long cap, long? declared)
            : base("the response body is over the cap")
        {
            Cap = cap;
            Declared = declared;
        }

        internal long Cap { get; }

        internal long? Declared { get; }
    }

    private static SdkError TooLarge(
        ResponseOverCap refusal, string method, string url, RequestInfo requestInfo) => new(
        Redaction.RedactMessageText(
            $"{method} {Redaction.UrlForLog(url)} answered with a body over the "
            + refusal.Cap.ToString(CultureInfo.InvariantCulture) + "-byte MaxResponseBytes cap"
            + (refusal.Declared is { } length
                ? " (Content-Length " + length.ToString(CultureInfo.InvariantCulture) + ")"
                : "")
            + ", so it was not buffered - raise MaxResponseBytes on the client if a body this "
            + "large is expected"),
        null,
        ErrorType.Unknown,
        null,
        null,
        requestInfo);
}
