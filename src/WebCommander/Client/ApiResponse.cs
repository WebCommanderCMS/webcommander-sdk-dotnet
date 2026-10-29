using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.Json;

using WebCommander.Errors;

namespace WebCommander.Client;

/// <summary>What every SDK call returns: one HTTP exchange, what was sent and what came back.</summary>
public sealed class ApiResponse
{
    private const int MaxReportedMessages = 50;

    private const int MaxMessageDepth = 4;

    /// <summary>Build a response from an already-redacted request and a normalised format.</summary>
    public ApiResponse(
        int statusCode,
        object? data,
        IReadOnlyDictionary<string, string>? headers,
        string? contentType,
        RequestInfo? requestInfo,
        ErrorType errorType = ErrorType.None,
        byte[]? rawBody = null,
        string? responseFormat = null,
        Type? dtoType = null)
    {
        StatusCode = statusCode;
        Map = data;
        Headers = Redaction.DetachText(headers) ?? Redaction.EmptyText;
        ContentType = contentType;
        _rawBody = rawBody;
        ResponseFormat = responseFormat ?? Config.SdkConfig.ResponseFormatMap;
        _dtoType = dtoType;
        // Gated on Succeeded, which reads StatusCode and Map, both assigned above.
        ErrorType = errorType == ErrorType.None && !Succeeded
            ? ErrorTypes.Classify(statusCode, BodyDeniesSuccess(Map))
            : errorType;
        RequestInfo = requestInfo;
        _json = new Lazy<string?>(RenderBodyText, LazyThreadSafetyMode.ExecutionAndPublication);
        _dto = new Lazy<object?>(BuildDto, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public int StatusCode { get; }

    /// <summary>The body in the resolved <see cref="ResponseFormat"/>, or <c>null</c> when it has none.</summary>
    public object? Data => ResponseFormat switch
    {
        Config.SdkConfig.ResponseFormatJson => Json,
        Config.SdkConfig.ResponseFormatDto => Dto,
        _ => Map,
    };

    /// <summary>The decoded body: map or list for JSON, string for <c>text/*</c>, bytes otherwise.</summary>
    public object? Map { get; }

    /// <summary>The server's own body text as it arrived; <c>null</c> for a binary or empty body.</summary>
    public string? Json => _json.Value;

    /// <summary>The typed view of the body; a view, never the contract - <see cref="Map"/> has everything.</summary>
    public object? Dto => _dto.Value;

    /// <summary>Which form <see cref="Data"/> took: <c>map</c>, <c>json</c> or <c>dto</c>.</summary>
    public string ResponseFormat { get; }

    /// <summary>The body's own bytes for <see cref="Json"/>; a binary body shares Map's array.</summary>
    private readonly byte[]? _rawBody;

    private readonly Type? _dtoType;
    private readonly Lazy<string?> _json;
    private readonly Lazy<object?> _dto;

    /// <summary>The body as text; a <c>text/*</c> body keeps its declared-charset decoding.</summary>
    private string? RenderBodyText()
    {
        if (Map is byte[])
            return null;
        if (Map is string decoded)
            return decoded;
        if (_rawBody is null || _rawBody.Length == 0)
            return null;
        return Encoding.UTF8.GetString(_rawBody);
    }

    /// <summary>The DTO the endpoint named, or <c>null</c> when the body is not a record.</summary>
    private object? BuildDto() =>
        _dtoType is null || Map is not IReadOnlyDictionary<string, object?> envelope
            ? null
            // Qualified: the simple name resolves to this class's own Dto property first.
            : WebCommander.Dto.Build(_dtoType, envelope);

    /// <summary>The response headers, immutable; names compare ordinally, as the store spelled them.</summary>
    public IReadOnlyDictionary<string, string> Headers { get; }

    public string? ContentType { get; }

    public ErrorType ErrorType { get; }

    public RequestInfo? RequestInfo { get; }

    /// <summary>A 2xx status and no <c>isSuccess: false</c> in the body, which a 200 can carry.</summary>
    public bool Succeeded => StatusCode is >= 200 and < 300 && !BodyDeniesSuccess(Map);

    /// <summary>The one body key that is allowed to contradict a 2xx status.</summary>
    private const string SuccessFlagKey = "isSuccess";

    /// <summary>An explicit <c>isSuccess</c> denial; <c>message</c> appears in success envelopes too.</summary>
    private static bool BodyDeniesSuccess(object? data)
    {
        // The second arm covers a mapping of another shape, such as a hand-built response.
        if (data is IReadOnlyDictionary<string, object?> body)
            return body.TryGetValue(SuccessFlagKey, out var flag) && !IsTruthy(flag);
        if (data is IDictionary raw)
            return raw.Contains(SuccessFlagKey) && !IsTruthy(raw[SuccessFlagKey]);
        return false;
    }

    /// <summary>JSON truthiness; anything unrecognised is true, so an unknown shape does not deny.</summary>
    private static bool IsTruthy(object? value)
    {
        switch (value)
        {
            case null:
                return false;
            case bool flag:
                return flag;
            case string text:
                return text.Length > 0;
            // Ahead of IConvertible, which BigInteger does not implement.
            case System.Numerics.BigInteger big:
                return !big.IsZero;
            case IConvertible number
                when number.GetTypeCode() is >= TypeCode.SByte and <= TypeCode.Decimal:
                return number.ToDouble(CultureInfo.InvariantCulture) != 0.0;
            // An arbitrary IEnumerable is not probed: advancing it could consume a one-shot sequence.
            case ICollection collection:
                return collection.Count > 0;
            default:
                return true;
        }
    }

    /// <summary>Human-readable problems from an error body, most specific tier first, capped.</summary>
    public IReadOnlyList<string> GetErrorMessages()
    {
        // Map, never Data: the caller's chosen format must not change what a failure says.
        var body = Map;
        if (body is not IReadOnlyDictionary<string, object?>)
            body = ParseJsonBody(body);
        if (body is not IReadOnlyDictionary<string, object?> envelope)
            return Array.Empty<string>();

        var messages = ReadErrorEntries(envelope.GetValueOrDefault("errors"));
        if (messages.Count == 0)
            messages = ReadMessageValue(envelope.GetValueOrDefault("message"), 0);
        if (messages.Count == 0)
        {
            messages = ReadMessageValue(envelope.GetValueOrDefault("error"), 0);
            if (messages.Count == 0)
                messages = ReadMessageValue(envelope.GetValueOrDefault("errorCode"), 0);
        }
        if (messages.Count == 0)
        {
            messages = ReadMessageValue(envelope.GetValueOrDefault("detail"), 0);
            if (messages.Count == 0)
                messages = ReadMessageValue(envelope.GetValueOrDefault("title"), 0);
        }
        return CapMessages(messages);
    }

    /// <summary>Throw when the call failed, otherwise return this response for chaining.</summary>
    public ApiResponse RaiseForStatus()
    {
        if (Succeeded)
            return this;
        var messages = GetErrorMessages();
        var summary = messages.Count == 0 ? $"HTTP {StatusCode}" : messages[0];
        var redactedBody = Redaction.RedactValue(Map);
        var redactedHeaders = Redaction.RedactMapping(Headers);
        SdkError error = StatusCode is 401 or 403
            ? new SdkAuthError(Redaction.RedactMessageText(summary), StatusCode, ErrorType, redactedBody, redactedHeaders, RequestInfo)
            : new SdkError(Redaction.RedactMessageText(summary), StatusCode, ErrorType, redactedBody, redactedHeaders, RequestInfo);
        foreach (var message in messages)
            error.AddError(Redaction.RedactMessageText(message));
        throw error;
    }

    private static object? ParseJsonBody(object? body)
    {
        string text = body switch
        {
            byte[] bytes => Encoding.UTF8.GetString(bytes),
            string decoded => decoded,
            _ => "",
        };
        if (text.Length == 0)
            return null;
        // The BOM goes before the guard too, or a BOM'd body fails the '{' test.
        text = text.Trim().TrimStart('\uFEFF').Trim();
        if (!text.StartsWith('{') && !text.StartsWith('['))
            return null;
        try
        {
            using var document = JsonDocument.Parse(text);
            return Redaction.Materialise(document.RootElement);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>Read the <c>errors</c> tier, whichever of its three shapes arrived.</summary>
    private static List<string> ReadErrorEntries(object? entries)
    {
        var messages = new List<string>();
        if (entries is IReadOnlyDictionary<string, object?> byField)
        {
            foreach (var (field, value) in byField)
                PrefixField(field, ReadMessageValue(value, 0), messages);
            return messages;
        }
        if (IsSequence(entries, out var list))
        {
            foreach (var entry in list)
            {
                if (entry is IReadOnlyDictionary<string, object?> item)
                {
                    var text = item.GetValueOrDefault("message") ?? item.GetValueOrDefault("code");
                    PrefixField(item.GetValueOrDefault("field"), ReadMessageValue(text, 0), messages);
                }
                else
                {
                    messages.AddRange(ReadMessageValue(entry, 0));
                }
            }
            return messages;
        }
        if (entries is string single)
            return ReadMessageValue(single, 0);
        return messages;
    }

    /// <summary>Turn any value from an error body into messages; blanks dropped, depth-guarded.</summary>
    private static List<string> ReadMessageValue(object? value, int depth)
    {
        if (value is null || depth > MaxMessageDepth)
            return new List<string>();
        if (value is string message)
            return message.Trim().Length == 0
                ? new List<string>()
                : new List<string> { message };
        if (value is IReadOnlyDictionary<string, object?> nested)
        {
            var text = nested.GetValueOrDefault("message") ?? nested.GetValueOrDefault("code");
            return ReadMessageValue(text, depth + 1);
        }
        if (IsSequence(value, out var list))
        {
            var messages = new List<string>();
            foreach (var item in list)
                messages.AddRange(ReadMessageValue(item, depth + 1));
            return messages;
        }
        var scalar = Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
        return scalar.Trim().Length == 0 ? new List<string>() : new List<string> { scalar };
    }

    /// <summary>Prefix each message with its field, tested for presence: a bulk import's row 0 is real.</summary>
    private static void PrefixField(object? fieldName, List<string> fieldMessages, List<string> output)
    {
        var name = fieldName is null
            ? ""
            : (Convert.ToString(fieldName, CultureInfo.InvariantCulture) ?? "").Trim();
        foreach (var message in fieldMessages)
            output.Add(name.Length == 0 ? message : $"{name}: {message}");
    }

    private static IReadOnlyList<string> CapMessages(List<string> messages)
    {
        if (messages.Count <= MaxReportedMessages)
            return messages;
        var capped = messages.GetRange(0, MaxReportedMessages);
        capped.Add($"+{messages.Count - MaxReportedMessages} more problems");
        return capped;
    }

    private static bool IsSequence(object? value, out IEnumerable<object?> sequence)
    {
        if (value is not string && value is IEnumerable raw)
        {
            sequence = raw.Cast<object?>();
            return true;
        }
        sequence = Array.Empty<object?>();
        return false;
    }
}
