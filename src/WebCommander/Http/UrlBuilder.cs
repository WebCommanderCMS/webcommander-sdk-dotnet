using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using WebCommander.Errors;

namespace WebCommander.Http;

/// <summary>URL assembly: a path value stays in its own segment, and a dots-only segment is refused.</summary>
public static partial class UrlBuilder
{
    [GeneratedRegex(@"\{([a-z_][a-z0-9_]*)\}")]
    private static partial Regex PlaceholderPattern();

    [GeneratedRegex("[?#]")]
    private static partial Regex QueryOrFragment();

    private static readonly Regex RedactedParameter =
        new("[?#&]([^?#&=]*)=" + Regex.Escape(Redaction.Redacted) + "(?=[&#]|$)");

    /// <summary>The <c>{name}</c> placeholders in a template, first-seen order, query part excluded.</summary>
    public static IReadOnlyList<string> FindPathParamNames(string pathTemplate)
    {
        var names = new List<string>();
        var seen = new HashSet<string>();
        foreach (Match match in PlaceholderPattern().Matches(SplitTemplate(pathTemplate).Template))
            if (seen.Add(match.Groups[1].Value))
                names.Add(match.Groups[1].Value);
        return names;
    }

    /// <summary>Fill <c>{name}</c> placeholders, reporting every problem in one exception before any I/O.</summary>
    public static string SubstitutePathParams(
        string pathTemplate, IReadOnlyDictionary<string, object?>? pathParams)
    {
        var (template, queryAndFragment) = SplitTemplate(pathTemplate);
        var expected = FindPathParamNames(pathTemplate);
        var supplied = pathParams ?? new Dictionary<string, object?>();

        var problems = new List<string>();
        if (FragmentAt(pathTemplate) is int fragmentAt)
            problems.Add(FragmentRefusal(fragmentAt));
        var missing = expected.Where(name => !supplied.ContainsKey(name)).OrderBy(name => name, StringComparer.Ordinal).ToList();
        if (missing.Count > 0)
            problems.Add("missing path params: " + string.Join(", ", missing));
        var unknown = supplied.Keys.Where(key => !expected.Contains(key)).OrderBy(key => key, StringComparer.Ordinal).ToList();
        if (unknown.Count > 0)
            problems.Add("unknown path params: " + string.Join(", ", unknown));

        var blank = new List<string>();
        var wrongTyped = new List<string>();
        var dotsOnly = new List<string>();
        foreach (var name in expected)
        {
            if (!supplied.ContainsKey(name)) continue;
            var value = supplied[name];
            if (value is null || (value is string text && text.Trim().Length == 0))
                blank.Add(name);
            else if (!UsablePathParam(value))
                wrongTyped.Add($"{name} ({value.GetType().Name})");
            // Judged on the encoded form, which is what reaches the wire.
            else if (IsDotsOnly(PercentEncode(EncodablePathText(value))))
                dotsOnly.Add(name);
        }
        if (blank.Count > 0)
            problems.Add("blank path params: " + string.Join(", ", blank.OrderBy(entry => entry, StringComparer.Ordinal)));
        if (wrongTyped.Count > 0)
            problems.Add("wrong-typed path params: " + string.Join(", ", wrongTyped.OrderBy(entry => entry, StringComparer.Ordinal)));
        if (dotsOnly.Count > 0)
            problems.Add("dot-only path params: "
                + string.Join(", ", dotsOnly.OrderBy(entry => entry, StringComparer.Ordinal))
                + " - a path parameter of only dots is removed by the URL layer, so the request "
                + "would address a different endpoint; pass the identifier the endpoint takes");
        if (problems.Count > 0)
            throw new SdkUsageError(Redaction.RedactMessageText($"{pathTemplate} -> " + string.Join("; ", problems)));

        var substituted = PlaceholderPattern().Replace(template, placeholder =>
            PercentEncode(EncodablePathText(supplied[placeholder.Groups[1].Value])));
        if (substituted.Contains('{') || substituted.Contains('}'))
            throw new SdkUsageError(Redaction.RedactMessageText($"{pathTemplate} -> malformed placeholder, unresolved: {substituted}"));
        return substituted + queryAndFragment;
    }

    /// <summary>Join a base URL and a path; an absolute URL passes only on the configured origin.</summary>
    public static string BuildUrl(string baseUrl, string path)
    {
        RejectARedactedValue(path);
        var lowered = path.ToLowerInvariant();
        if (lowered.StartsWith("http://", StringComparison.Ordinal) ||
            lowered.StartsWith("https://", StringComparison.Ordinal))
        {
            RequireSameOrigin(baseUrl, path);
            return path;
        }
        var trimmedBase = baseUrl.TrimEnd('/');
        return trimmedBase + (path.StartsWith('/') ? "" : "/") + path;
    }

    // Unreserved characters only; internal, as on a whole URL the delimiters must survive.
    internal static string PercentEncode(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        var builder = new StringBuilder(bytes.Length);
        foreach (var byteValue in bytes)
        {
            var character = (char)byteValue;
            var unreserved = character is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z')
                or (>= '0' and <= '9') or '-' or '.' or '_' or '~';
            if (unreserved) builder.Append(character);
            else builder.Append('%').Append(byteValue.ToString("X2"));
        }
        return builder.ToString();
    }

    private static int? FragmentAt(string url)
    {
        var at = url.IndexOf('#');
        return at < 0 ? null : at;
    }

    internal static void RejectAFragment(string url)
    {
        if (FragmentAt(url) is int at)
            throw new SdkUsageError(Redaction.RedactMessageText("path: " + FragmentRefusal(at)));
    }

    // The fragment is not quoted: an implicit-grant token rides in one.
    private static string FragmentRefusal(int at) =>
        $"a fragment begins at character {at.ToString(CultureInfo.InvariantCulture)} - "
        + "a '#' begins a fragment, and a fragment is never sent - RFC 3986 makes it client-side, "
        + "so the URL that reached the store would not be the URL written here, and any query "
        + "parameter appended after it would have been swallowed by it. Put the value in the path "
        + "or the query instead (the fragment's own text is not shown: it may be a credential)";

    internal static IEnumerable<(string Name, string Value)> QueryPairs(string query)
    {
        foreach (var chunk in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var equals = chunk.IndexOf('=');
            yield return equals < 0 ? (chunk, "") : (chunk[..equals], chunk[(equals + 1)..]);
        }
    }

    private static void RejectARedactedValue(string path)
    {
        var found = RedactedParameter.Match(path);
        if (found.Success)
            throw new SdkUsageError(Redaction.RedactMessageText(
                $"path: parameter '{found.Groups[1].Value}' holds only the redaction marker " +
                $"{Redaction.Redacted}, so this URL is a printed or logged copy and the real " +
                "value is not in it - read the value off the response itself"));
    }

    private static void RequireSameOrigin(string baseUrl, string url)
    {
        var parsed = Parse(url);
        if (!string.IsNullOrEmpty(parsed.UserInfo))
            throw new SdkUsageError(Redaction.RedactMessageText(
                $"absolute URL for host {parsed.Host} carries embedded credentials, which " +
                "would replace the bearer token; strip the userinfo before following it"));
        if (Origin(parsed) != Origin(Parse(baseUrl)))
            // Both redacted: a pagination link can carry a token in its query.
            throw new SdkUsageError(Redaction.RedactMessageText(
                $"{Redaction.RedactText(url)} is not on the configured origin " +
                $"{Redaction.RedactText(baseUrl)}; an absolute URL is " +
                "requested with the access token attached, so only the store's own links " +
                "may be passed back"));
    }

    private static Uri Parse(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            throw new SdkUsageError(Redaction.RedactMessageText($"{Redaction.RedactText(url)} -> not a parseable absolute URL"));
        return uri;
    }

    private static string Origin(Uri uri)
    {
        var scheme = uri.Scheme.ToLowerInvariant();
        var host = uri.Host.ToLowerInvariant();
        return $"{scheme}://{host}:{uri.Port}";
    }

    private static (string Template, string QueryAndFragment) SplitTemplate(string pathTemplate)
    {
        var boundary = QueryOrFragment().Match(pathTemplate);
        return boundary.Success
            ? (pathTemplate[..boundary.Index], pathTemplate[boundary.Index..])
            : (pathTemplate, "");
    }

    private static bool UsablePathParam(object value) =>
        value is string or int or long;

    // Shared by the dots check and the substitution, so the value judged is the value sent.
    private static string EncodablePathText(object? value) =>
        value is string text ? text.Trim() : Convert.ToString(value, CultureInfo.InvariantCulture)!;

    private static bool IsDotsOnly(string encoded)
    {
        if (encoded.Length == 0)
            return false;
        foreach (var character in encoded)
        {
            if (character != '.')
                return false;
        }
        return true;
    }
}
