namespace WebCommander.Client;

/// <summary>The request behind a response, redacted at construction and safe to log while held.</summary>
public sealed class RequestInfo
{
    private readonly IReadOnlyDictionary<string, object?> _queryParams;
    private readonly object? _body;

    public RequestInfo(
        string? clientName,
        string? apiVersion,
        string? httpMethod,
        string? url,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        object? body = null,
        IReadOnlyDictionary<string, string>? headers = null,
        int attemptCount = 1,
        int maxAttempts = 1,
        double elapsedSeconds = 0.0)
    {
        ClientName = clientName;
        ApiVersion = apiVersion;
        HttpMethod = httpMethod;
        Url = url is null ? null : Redaction.RedactText(url);
        _queryParams = RedactQueryParams(queryParams);
        _body = Redaction.Detach(Redaction.RedactValue(body));
        Headers = Redaction.DetachText(Redaction.RedactMapping(headers)) ?? Redaction.EmptyText;
        AttemptCount = attemptCount;
        MaxAttempts = maxAttempts;
        ElapsedSeconds = elapsedSeconds;
    }

    public string? ClientName { get; }

    public string? ApiVersion { get; }

    public string? HttpMethod { get; }

    public string? Url { get; }

    /// <summary>The query parameters, redacted and recursively immutable.</summary>
    public IReadOnlyDictionary<string, object?> QueryParams =>
        Redaction.CopyBuffersInMap(_queryParams)!;

    /// <summary>The request body, redacted at construction and recursively immutable.</summary>
    public object? Body => Redaction.CopyBuffers(_body);

    public IReadOnlyDictionary<string, string> Headers { get; }

    /// <summary>Which attempt produced the response (1-based).</summary>
    public int AttemptCount { get; }

    public int MaxAttempts { get; }

    public double ElapsedSeconds { get; }

    private static IReadOnlyDictionary<string, object?> RedactQueryParams(
        IReadOnlyDictionary<string, object?>? queryParams)
    {
        if (queryParams is null)
            return Redaction.EmptyMap;
        var shown = new Dictionary<string, object?>();
        foreach (var (key, value) in queryParams)
            shown[key] = Redaction.ShouldRedact(key) ? Redaction.Redacted : Redaction.RedactValue(value);
        return Redaction.DetachMap(shown)!;
    }
}
