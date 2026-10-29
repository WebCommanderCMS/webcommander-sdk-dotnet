using System.Globalization;
using System.Text;
using System.Text.Json;

using WebCommander.Config;
using WebCommander.Http;

namespace WebCommander.Client;

/// <summary>Renders one exchange for a terminal, redacted.</summary>
public static class ExchangeConsole
{
    private static readonly JsonSerializerOptions Pretty = new(Redaction.DisplayJson)
    {
        WriteIndented = true,
    };

    /// <summary>Stands in for a status on a request that got no response; text, not a number.</summary>
    internal const string NoResponseStatus = "no response";

    private const string MultipartPrefix = "<multipart: ";
    private const string PayloadFilesKey = "files";
    private const string PayloadFormKey = "form";

    /// <summary>The text to print for one exchange, per the config's print flags.</summary>
    public static string RenderExchange(
        ApiResponse response, object? reportedBody, SdkConfig config,
        IReadOnlyDictionary<string, object?>? files = null, bool formEncoded = false) =>
        Render(response.RequestInfo!, response.StatusCode.ToString(CultureInfo.InvariantCulture),
            // Map, never Data: a printed block shows the body the store sent.
            response.Succeeded, response.Map, response.Headers, reportedBody, config, files, formEncoded);

    internal static string RenderFailedExchange(
        RequestInfo info, object? reportedBody, SdkConfig config,
        IReadOnlyDictionary<string, object?>? files = null, bool formEncoded = false) =>
        Render(info, NoResponseStatus, false, null, new Dictionary<string, string>(),
            reportedBody, config, files, formEncoded);

    private static string Render(
        RequestInfo info, string statusText, bool succeeded, object? data,
        IReadOnlyDictionary<string, string> responseHeaders, object? body, SdkConfig config,
        IReadOnlyDictionary<string, object?>? files, bool formEncoded)
    {
        var colour = new Colour(config.PrintColor && StdoutSupportsColour());
        var glyphs = ResolveBoxGlyphs();
        var reportedBody = SummarisePayload(body, files);
        if (SdkConfig.PrintFormatCompact.Equals(config.PrintFormat))
        {
            var line = new StringBuilder("wc | ")
                .Append(info.ClientName).Append(" | ")
                .Append(info.HttpMethod).Append(" | ")
                .Append(info.Url).Append(" | ")
                .Append(statusText).Append(" | ")
                .Append(info.AttemptCount).Append('/').Append(info.MaxAttempts)
                .Append(" | ").Append(Display.Seconds(info.ElapsedSeconds)).Append('s');
            if (config.PrintRequestPayload)
                line.Append(" | payload=").Append(RenderJsonValue(Redaction.RedactValue(reportedBody), compact: true));
            if (config.PrintResponseBody)
                line.Append(" | response=").Append(RenderJsonValue(Redaction.RedactValue(data), compact: true));
            return Truncate(Neutralise(line.ToString()), config.PrintMaxChars, glyphs);
        }

        var output = new StringBuilder(colour.Dim(
            $"WebCommander  {info.HttpMethod}  {statusText}  {Display.Seconds(info.ElapsedSeconds)}s"));
        if (config.PrintApiInfo)
        {
            Row(output, "client", info.ClientName);
            Row(output, "api version", info.ApiVersion);
            Row(output, "method", info.HttpMethod);
            Row(output, "endpoint", info.Url);
            Row(output, "params", RenderJsonValue(info.QueryParams, compact: true));
            output.Append('\n').Append("status".PadRight(12)).Append(": ")
                .Append(colour.Status(Neutralise(statusText), succeeded));
            Row(output, "attempt", info.AttemptCount + " / " + info.MaxAttempts);
            Row(output, "elapsed", Display.Seconds(info.ElapsedSeconds) + "s");
        }
        if (config.PrintHeaders)
        {
            Section(output, colour, "request headers", glyphs);
            foreach (var (name, value) in Sorted(info.Headers))
                Row(output, name, value, 14);
            Section(output, colour, "response headers", glyphs);
            foreach (var (name, value) in Sorted(Redaction.RedactMapping(responseHeaders)))
                Row(output, name, value, 14);
        }
        if (config.PrintRequestPayload && reportedBody is not null)
        {
            Section(output, colour, "payload", glyphs);
            Block(output, RenderJsonValue(Redaction.RedactValue(reportedBody), compact: false));
        }
        if (config.PrintResponseBody)
        {
            Section(output, colour, "response", glyphs);
            Block(output, RenderJsonValue(Redaction.RedactValue(data), compact: false));
        }
        if (config.PrintCurl)
        {
            Section(output, colour, "curl (POSIX shell)", glyphs);
            Block(output, BuildCurlCommand(info, reportedBody, files, config, formEncoded));
        }
        return Truncate(output.ToString(), config.PrintMaxChars, glyphs);
    }

    private static IEnumerable<KeyValuePair<string, string>> Sorted(IReadOnlyDictionary<string, string> headers) =>
        headers.OrderBy(entry => entry.Key, StringComparer.Ordinal);

    /// <summary>The curl line; a JSON body gets a <c>Content-Type</c> only when the caller set none.</summary>
    private static string BuildCurlCommand(
        RequestInfo info, object? payload, IReadOnlyDictionary<string, object?>? files,
        SdkConfig config, bool formEncoded)
    {
        var curl = new StringBuilder("curl -X ").Append(ShellQuote(info.HttpMethod ?? ""))
            .Append(' ').Append(ShellQuote(Neutralise(WithQuery(info))));
        if (!config.VerifySsl)
            curl.Append(" \\\n  -k");
        foreach (var (name, value) in Sorted(info.Headers))
            curl.Append(" \\\n  -H ").Append(ShellQuote(Neutralise(name + ": " + value)));

        var entries = MultipartEntries(files);
        var body = FormPart(payload);
        foreach (var (field, filename) in entries)
            curl.Append(" \\\n  -F ")
                .Append(ShellQuote(Neutralise(field) + "=@" + Neutralise(filename)));

        if (entries.Count > 0)
        {
            foreach (var (name, value) in FormFields(body))
                curl.Append(" \\\n  -F ").Append(ShellQuote(Neutralise(name) + "=" + Neutralise(value)));
        }
        else if (body is not null)
        {
            var fields = formEncoded ? FormFields(body) : new List<(string Name, string Value)>();
            if (fields.Count > 0)
            {
                foreach (var (name, value) in fields)
                    curl.Append(" \\\n  --data-urlencode ")
                        .Append(ShellQuote(Neutralise(name) + "=" + Neutralise(value)));
            }
            else
            {
                if (!formEncoded && !HasContentType(info.Headers))
                    curl.Append(" \\\n  -H ").Append(ShellQuote("Content-Type: application/json"));
                curl.Append(" \\\n  --data-raw ")
                    .Append(ShellQuote(RenderJson(Redaction.RedactValue(body), JsonLayout.Collapsed)));
            }
        }
        return curl.ToString();
    }

    /// <summary>Describe what was sent without reading a byte of any upload.</summary>
    public static object? SummarisePayload(object? body, IReadOnlyDictionary<string, object?>? files)
    {
        var entries = MultipartEntries(files);
        if (entries.Count == 0)
            return body;
        var summary = MultipartPrefix
            + string.Join(", ", entries.Select(entry => entry.Field + "=" + entry.Filename)) + ">";
        if (body is null)
            return summary;
        return new Dictionary<string, object?> { [PayloadFilesKey] = summary, [PayloadFormKey] = body };
    }

    /// <summary>Field name and filename for every file; a label, not a transcript of the wire.</summary>
    private static List<(string Field, string Filename)> MultipartEntries(
        IReadOnlyDictionary<string, object?>? files)
    {
        var entries = new List<(string, string)>();
        if (files is null)
            return entries;
        foreach (var (field, value) in files)
            entries.Add((field, FilenameFor(field, value)));
        return entries;
    }

    private static string FilenameFor(string field, object? value) => value switch
    {
        IReadOnlyList<object?> tuple when tuple.Count > 0 && tuple[0] is string declared => declared,
        _ => field,
    };

    /// <summary>The form body inside a <see cref="SummarisePayload"/> result, or the payload itself.</summary>
    private static object? FormPart(object? payload)
    {
        if (payload is IReadOnlyDictionary<string, object?> map
            && map.Count == 2
            && map.ContainsKey(PayloadFilesKey) && map.ContainsKey(PayloadFormKey)
            && map[PayloadFilesKey] is string summary
            && summary.StartsWith(MultipartPrefix, StringComparison.Ordinal))
        {
            return map[PayloadFormKey];
        }
        return payload;
    }

    /// <summary>Redacted <c>name=value</c> pairs for a form body; anything else renders as JSON.</summary>
    private static List<(string Name, string Value)> FormFields(object? body)
    {
        var fields = new List<(string, string)>();
        switch (Redaction.RedactValue(body))
        {
            case IReadOnlyDictionary<string, object?> map:
                foreach (var (name, value) in map)
                    fields.Add((name, Transport.Stringify(value)));
                break;
            case System.Collections.IEnumerable list and not string:
                foreach (var entry in list)
                {
                    if (entry is not IReadOnlyList<object?> { Count: 2 } pair || pair[0] is not string name)
                        return new List<(string, string)>();
                    fields.Add((name, Transport.Stringify(pair[1])));
                }
                break;
        }
        return fields;
    }

    /// <summary>The URL to reproduce, each query name once, as the recorded URL already carries some.</summary>
    private static string WithQuery(RequestInfo info)
    {
        if (info.QueryParams.Count == 0)
            return info.Url ?? "";
        var alreadyOnTheUrl = QueryNamesIn(info.Url);
        var query = new StringBuilder(info.Url);
        var separator = (info.Url ?? "").Contains('?') ? '&' : '?';
        foreach (var (key, value) in info.QueryParams)
        {
            if (alreadyOnTheUrl.Contains(key))
                continue;
            var values = value is System.Collections.IEnumerable sequence and not string
                ? sequence.Cast<object?>()
                : new[] { value };
            foreach (var single in values)
            {
                query.Append(separator).Append(QueryEncode(key)).Append('=')
                    .Append(QueryEncode(Transport.Stringify(single)));
                separator = '&';
            }
        }
        return query.ToString();
    }

    /// <summary>Percent-encoded as the transport sends it, with <c>*</c> restored so <c>***</c> reads.</summary>
    private static string QueryEncode(string text) =>
        Uri.EscapeDataString(text).Replace("%2A", "*", StringComparison.Ordinal);

    private static HashSet<string> QueryNamesIn(string? url)
    {
        var names = new HashSet<string>(StringComparer.Ordinal);
        var marker = url?.IndexOf('?') ?? -1;
        if (url is null || marker < 0)
            return names;
        var query = url[(marker + 1)..];
        var fragment = query.IndexOf('#');
        if (fragment >= 0)
            query = query[..fragment];
        foreach (var (name, _) in UrlBuilder.QueryPairs(query))
            names.Add(Uri.UnescapeDataString(name));
        return names;
    }

    private static bool HasContentType(IReadOnlyDictionary<string, string> headers) =>
        headers.Keys.Any(name => name.Equals("Content-Type", StringComparison.OrdinalIgnoreCase));

    private static void Row(StringBuilder output, string label, object? value, int width = 12) =>
        output.Append('\n').Append(label.PadRight(width)).Append(": ")
            .Append(Neutralise(Convert.ToString(value, CultureInfo.InvariantCulture) ?? "null"));

    private static void Section(StringBuilder output, Colour colour, string label, BoxGlyphs glyphs)
    {
        var rule = glyphs.Rule;
        // new string(rule, 2): rule + rule would add two chars as an int.
        var prefix = new string(rule, 2) + " " + label + " ";
        output.Append('\n')
            .Append(colour.Dim(prefix + new string(rule, Math.Max(4, 62 - prefix.Length))));
    }

    private readonly record struct BoxGlyphs(char Rule, string Ellipsis);

    /// <summary>The box-drawing glyphs probed as one string to choose between the Unicode and ASCII sets.</summary>
    private const string BoxGlyphSample = "┌└├│─…";

    private static readonly BoxGlyphs UnicodeGlyphs = new('─', "…");
    private static readonly BoxGlyphs AsciiGlyphs = new('-', "...");

    /// <summary>Box-drawing characters the console can print, probed with an exception fallback.</summary>
    private static BoxGlyphs ResolveBoxGlyphs()
    {
        try
        {
            var console = Console.OutputEncoding;
            var strict = Encoding.GetEncoding(
                console.CodePage, EncoderFallback.ExceptionFallback, DecoderFallback.ReplacementFallback);
            strict.GetBytes(BoxGlyphSample);
            return UnicodeGlyphs;
        }
        catch (Exception error) when (error is EncoderFallbackException or ArgumentException
                                          or NotSupportedException or IOException)
        {
            return AsciiGlyphs;
        }
    }

    /// <summary>ANSI helpers that no-op when colour is off or stdout cannot carry it.</summary>
    private readonly struct Colour
    {
        /// <summary>The ANSI introducer, as an escape so no raw control byte sits in the source.</summary>
        internal static readonly string Escape = ((char)0x1b).ToString();

        internal static readonly string Reset = Escape + "[0m";

        private readonly bool _enabled;

        internal Colour(bool enabled) => _enabled = enabled;

        internal string Dim(string text) => _enabled ? Escape + "[2m" + text + Reset : text;

        internal string Status(string text, bool succeeded) =>
            _enabled ? Escape + "[" + (succeeded ? "32" : "31") + "m" + text + Reset : text;
    }

    /// <summary>Whether colour escapes may be emitted: <c>NO_COLOR</c>, <c>TERM=dumb</c>, redirection.</summary>
    private static bool StdoutSupportsColour()
    {
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("NO_COLOR")))
            return false;
        if (string.Equals(Environment.GetEnvironmentVariable("TERM"), "dumb", StringComparison.Ordinal))
            return false;
        try
        {
            return !Console.IsOutputRedirected;
        }
        catch (IOException)
        {
            return false;
        }
    }

    private static void Block(StringBuilder output, string text)
    {
        foreach (var line in text.Split('\n'))
            output.Append('\n').Append(line);
    }

    /// <summary>Render an already-redacted value for display; it redacts nothing itself.</summary>
    public static string RenderJsonValue(object? value, bool compact = false) =>
        RenderJson(value, compact ? JsonLayout.OneLine : JsonLayout.AsWritten);

    /// <summary>How much of a rendered JSON document's own whitespace survives.</summary>
    private enum JsonLayout
    {
        /// <summary>The document as the caller wrote it, line breaks and indentation kept.</summary>
        AsWritten,

        /// <summary>Every line break folded to a space, for a one-line-per-call format.</summary>
        OneLine,

        /// <summary>Whitespace between tokens gone, for a <c>--data-raw</c>.</summary>
        Collapsed,
    }

    private static string RenderJson(object? value, JsonLayout layout)
    {
        if (value is null)
            return "<empty>";
        if (value is byte[] bytes)
            return $"<{bytes.Length} bytes>";
        if (value is string text && JsonDocumentText(text, layout) is { } document)
            return layout == JsonLayout.Collapsed ? NeutraliseOneLine(document) : Neutralise(document);
        try
        {
            return JsonSerializer.Serialize(value, layout == JsonLayout.AsWritten ? Pretty : Redaction.DisplayJson);
        }
        catch (Exception error) when (error is JsonException or NotSupportedException)
        {
            return Neutralise(Convert.ToString(value, CultureInfo.InvariantCulture) ?? "null");
        }
    }

    /// <summary>A JSON document laid out for display, or <c>null</c> for any other string.</summary>
    private static string? JsonDocumentText(string text, JsonLayout layout)
    {
        var lead = 0;
        while (lead < text.Length && char.IsWhiteSpace(text[lead]))
            lead++;
        if (lead >= text.Length || (text[lead] != '{' && text[lead] != '['))
            return null;
        try
        {
            using var document = JsonDocument.Parse(text, Redaction.DeepParse);
            if (document.RootElement.ValueKind is not (JsonValueKind.Object or JsonValueKind.Array))
                return null;
            if (layout == JsonLayout.Collapsed)
                // The element, not a Materialise: it keeps the document's own digits, such as 1.50.
                return JsonSerializer.Serialize(document.RootElement, Redaction.DisplayJson);
        }
        catch (Exception error) when (error is JsonException or NotSupportedException)
        {
            return null;
        }
        var folded = new StringBuilder(text.Length);
        foreach (var character in text)
            folded.Append(character switch
            {
                '\t' or '\r' => ' ',
                '\n' => layout == JsonLayout.OneLine ? ' ' : '\n',
                _ => character,
            });
        return folded.ToString();
    }

    /// <summary>Whether a fragment needs quoting; safe characters are left bare, as in <c>curl -X GET</c>.</summary>
    private static bool NeedsQuoting(string fragment)
    {
        if (fragment.Length == 0)
            return true;
        foreach (var character in fragment)
        {
            var safe = character is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9'
                || character is '_' or '@' or '%' or '+' or '=' or ':' or ',' or '.' or '/' or '-';
            if (!safe)
                return true;
        }
        return false;
    }

    /// <summary>POSIX shell quoting; an embedded apostrophe becomes <c>'"'"'</c>.</summary>
    internal static string ShellQuote(string fragment) =>
        NeedsQuoting(fragment) ? "'" + fragment.Replace("'", "'\"'\"'", StringComparison.Ordinal) + "'" : fragment;

    /// <summary>Escape control characters for a terminal, except <c>\n</c>.</summary>
    internal static string Neutralise(string text) => NeutraliseControls(text, keepNewlines: true);

    /// <summary><see cref="Neutralise"/> with <c>\n</c> escaped too, for a single-line message.</summary>
    internal static string NeutraliseOneLine(string text) => NeutraliseControls(text, keepNewlines: false);

    private static string NeutraliseControls(string text, bool keepNewlines)
    {
        var output = new StringBuilder(text.Length);
        foreach (var character in text)
        {
            // U+2028/U+2029 stay as escapes: C# reads the literals as newlines.
            if ((character <= 0x1f && !(keepNewlines && character == '\n'))
                || (character >= 0x7f && character <= 0x9f)
                || character == '\u2028' || character == '\u2029')
                output.Append("\\u").Append(((int)character).ToString("x4", CultureInfo.InvariantCulture));
            else
                output.Append(character);
        }
        return output.ToString();
    }

    /// <summary>Cut to <c>printMaxChars</c>, notice included; a ceiling too small for the notice loses.</summary>
    private static string Truncate(string text, int maxChars, BoxGlyphs glyphs)
    {
        if (maxChars <= 0 || text.Length <= maxChars)
            return text;
        var ellipsis = glyphs.Ellipsis;
        var reserved = $"{ellipsis} +{text.Length} chars".Length
            + (text.Contains(Colour.Escape, StringComparison.Ordinal) ? Colour.Reset.Length : 0);
        var cut = Math.Min(text.Length, Math.Max(0, maxChars - reserved));
        if (cut >= text.Length)
            return text;
        return CloseColourRun(text[..cut]) + $"{ellipsis} +{text.Length - cut} chars";
    }

    /// <summary>Repair an ANSI sequence a character-wise cut landed inside.</summary>
    private static string CloseColourRun(string text)
    {
        var marker = text.LastIndexOf(Colour.Escape, StringComparison.Ordinal);
        if (marker < 0)
            return text;
        if (text.IndexOf('m', marker) < 0)
        {
            text = text[..marker];
            marker = text.LastIndexOf(Colour.Escape, StringComparison.Ordinal);
            if (marker < 0)
                return text;
        }
        return text.AsSpan(marker).StartsWith(Colour.Reset) ? text : text + Colour.Reset;
    }
}
