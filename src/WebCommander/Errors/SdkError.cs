using System.Globalization;
using System.Text;

namespace WebCommander.Errors;

/// <summary>Base class for every SDK error, and thrown directly for a non-2xx response.</summary>
public class SdkError : Exception
{
    private readonly List<string> _errors = new();

    // Redacted at the throw site: base.ToString() embeds the raw Message and cannot be told not to.
    public SdkError(string message) : base(message) { }

    public SdkError(
        string message,
        int? statusCode = null,
        Client.ErrorType? errorType = null,
        object? rawResponse = null,
        IReadOnlyDictionary<string, string>? responseHeaders = null,
        Client.RequestInfo? requestInfo = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorType = errorType;
        RawResponse = rawResponse;
        ResponseHeaders = Redaction.DetachText(responseHeaders) ?? Redaction.EmptyText;
        RequestInfo = requestInfo;
    }

    public int? StatusCode { get; }

    /// <summary>The failure classification, or <c>null</c> for a refusal that never reached the wire.</summary>
    public Client.ErrorType? ErrorType { get; }

    /// <summary>The decoded response body as handed over; <see cref="Explain"/> redacts it as it renders.</summary>
    public object? RawResponse { get; }

    /// <summary>Response headers, immutable; carried because an unfollowed 3xx has no body.</summary>
    public IReadOnlyDictionary<string, string> ResponseHeaders { get; } = Redaction.EmptyText;

    /// <summary>The call that failed, or <c>null</c> when raised before a request was assembled.</summary>
    public Client.RequestInfo? RequestInfo { get; }

    /// <summary>Append one field-level message, already redacted by the caller; chainable.</summary>
    public SdkError AddError(string errorMessage)
    {
        _errors.Add(errorMessage);
        return this;
    }

    /// <summary>A snapshot of the field-level messages the API supplied; empty when there were none.</summary>
    public IReadOnlyList<string> GetErrorMessages() => _errors.ToList();

    public string? GetHint() =>
        StatusCode is int code && HintsByStatus.TryGetValue(code, out var hint) ? hint : null;

    /// <summary>The full diagnostic as one block; it carries personal data, so print it per case only.</summary>
    public string Explain()
    {
        var lines = new StringBuilder(Summary());

        var info = RequestInfo;
        if (info is not null)
        {
            var shownParams = info.QueryParams;
            if (shownParams.Count > 0)
                lines.Append("\n  params  : ").Append(Flatten(RenderParams(shownParams)));
            if (info.AttemptCount > 0 && info.MaxAttempts > 0)
                lines.Append("\n  attempt : ").Append(info.AttemptCount).Append('/').Append(info.MaxAttempts);
            lines.Append("\n  elapsed : ").Append(Client.Display.Seconds(info.ElapsedSeconds)).Append('s');
        }

        var location = RedirectTarget(ResponseHeaders);
        if (location is not null)
            lines.Append("\n  location: ").Append(Flatten(location));

        var reported = GetErrorMessages();
        if (reported.Count > 1 || (reported.Count > 0 && !Summary().Contains(reported[0])))
        {
            lines.Append("\n  reported:");
            foreach (var message in reported)
                lines.Append("\n    - ").Append(Flatten(message));
        }

        var hint = GetHint();
        if (hint is not null)
            lines.Append("\n  hint    : ").Append(hint);

        if (RawResponse is not null)
        {
            var body = Flatten(Client.Display.RenderBody(Redaction.RedactValue(RawResponse)));
            if (body.Length > 500)
                body = body[..500] + $"... +{body.Length - 500} chars";
            lines.Append("\n  body    : ").Append(body);
        }

        return lines.ToString();
    }

    /// <summary>One greppable line, e.g. <c>404 GET /api/v4/admin/customers/0 - Customer Not Found</c>.</summary>
    public string Summary()
    {
        var parts = new List<string>();
        if (StatusCode is int code)
            parts.Add(code.ToString(CultureInfo.InvariantCulture));
        var info = RequestInfo;
        if (info is not null
            && !string.IsNullOrEmpty(info.HttpMethod) && !string.IsNullOrEmpty(info.Url))
        {
            parts.Add(Flatten(info.HttpMethod) + " "
                + Flatten(ShortenUrl(Redaction.UrlForLog(info.Url) ?? "")));
        }
        var reason = Flatten(Message);
        return parts.Count > 0 ? string.Join(" ", parts) + " - " + reason : reason;
    }

    /// <summary><see cref="Summary"/> prefixed onto the base rendering, keeping the stack trace.</summary>
    public override string ToString() => Summary() + Environment.NewLine + base.ToString();

    private static string ShortenUrl(string url)
    {
        var marker = url.IndexOf("://", StringComparison.Ordinal);
        if (marker < 0)
            return url;
        var remainder = url[(marker + 3)..];
        var slash = remainder.IndexOf('/');
        return slash >= 0 ? remainder[slash..] : url;
    }

    private static string RenderParams(IReadOnlyDictionary<string, object?> queryParams)
    {
        var rendered = new StringBuilder("{");
        var first = true;
        foreach (var (name, value) in queryParams)
        {
            if (!first)
                rendered.Append(", ");
            first = false;
            rendered.Append(QuoteCanonical(name)).Append(": ").Append(RenderParamValue(value));
        }
        return rendered.Append('}').ToString();
    }

    private static string RenderParamValue(object? value)
    {
        switch (value)
        {
            case null:
                return "None";
            case bool flag:
                return flag ? "True" : "False";
            case string text:
                return QuoteCanonical(text);
            case System.Collections.IEnumerable items:
                {
                    var rendered = new StringBuilder("[");
                    var first = true;
                    foreach (var item in items)
                    {
                        if (!first)
                            rendered.Append(", ");
                        first = false;
                        rendered.Append(RenderParamValue(item));
                    }
                    return rendered.Append(']').ToString();
                }
            default:
                return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "None";
        }
    }

    private static string QuoteCanonical(string text)
    {
        if (text.Contains('\'') && !text.Contains('"'))
            return "\"" + text + "\"";
        return "'" + text.Replace("\\", "\\\\").Replace("'", "\\'") + "'";
    }

    internal static string Flatten(object? value)
    {
        var text = value?.ToString() ?? "null";
        var builder = new StringBuilder(text.Length);
        foreach (var character in text)
        {
            if (character <= 0x1f || (character >= 0x7f && character <= 0x9f) || character == '\u2028' || character == '\u2029')
            {
                builder.Append(character switch
                {
                    '\n' => "\\n",
                    '\r' => "\\r",
                    '\t' => "\\t",
                    _ => "\\u" + ((int)character).ToString("x4", CultureInfo.InvariantCulture),
                });
            }
            else
            {
                builder.Append(character);
            }
        }
        return builder.ToString();
    }

    private static string? RedirectTarget(IReadOnlyDictionary<string, string> headers)
    {
        foreach (var (name, value) in headers)
            if (string.Equals(name, "location", StringComparison.OrdinalIgnoreCase))
                return Redaction.RedactText(value);
        return null;
    }

    private const string RedirectHint =
        "the store redirected and the SDK does not follow one - read the Location " +
        "header off the response and decide whether to re-issue the call against it. " +
        "The copy on this exception's ResponseHeaders has its secret query parameters " +
        "redacted, so it is for reading and not for re-issuing: call again with " +
        "raiseOnError off if you need the response itself";

    private static readonly IReadOnlyDictionary<int, string> HintsByStatus = BuildHints();

    private static Dictionary<int, string> BuildHints()
    {
        var hints = new Dictionary<int, string>();
        foreach (var status in new[] { 301, 302, 303, 307, 308 })
            hints[status] = RedirectHint;
        hints[400] = "the API rejected the request itself - check the payload shape and " +
                     "the query parameter names against the endpoint's documentation";
        hints[401] = "credentials were rejected, or the token expired and could not be " +
                     "renewed - check ClientId, ClientSecret and AuthString";
        hints[403] = "authenticated, but this client is not allowed to do that";
        hints[404] = "no such record - check the id you passed actually exists";
        hints[405] = "wrong HTTP method for this path";
        hints[409] = "conflicts with existing data, e.g. an email already in use";
        hints[429] = "rate limited - slow down, or raise MaxRetries so the SDK waits " +
                     "and retries for you";
        hints[500] = "the server failed. Retrying may help on a read; the SDK will not " +
                     "retry a write on a 500, because it cannot tell whether the server committed";
        return hints;
    }
}
