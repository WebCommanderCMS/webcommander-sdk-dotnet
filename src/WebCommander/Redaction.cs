using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace WebCommander;

/// <summary>Decides what counts as a secret, by key or parameter name, for every display path.</summary>
public static partial class Redaction
{
    internal static readonly JsonSerializerOptions DisplayJson = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        // Must track DeepParse, or a body past 64 levels fails to render.
        MaxDepth = 512,
        Converters = { new BigIntegerJsonConverter() },
    };

    private sealed class BigIntegerJsonConverter
        : System.Text.Json.Serialization.JsonConverter<System.Numerics.BigInteger>
    {
        public override System.Numerics.BigInteger Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            throw new NotSupportedException();

        public override void Write(
            Utf8JsonWriter writer, System.Numerics.BigInteger value, JsonSerializerOptions options) =>
            writer.WriteRawValue(value.ToString(CultureInfo.InvariantCulture), skipInputValidation: true);
    }

    internal static readonly JsonDocumentOptions DeepParse = new() { MaxDepth = 512 };

    public const string Redacted = "***";

    public const string TooDeep = "<...>";

    public const string Circular = "<circular>";

    internal const string RedactionFailed = "<redaction failed>";

    internal const string UnparsedJson = "<unparsed JSON";

    public const int MaxDepth = 64;

    /// <summary>Name fragments marking a secret key, matched ignoring case and separators.</summary>
    public static readonly IReadOnlyList<string> SensitiveKeyMarkers =
        System.Collections.Immutable.ImmutableArray.Create(
            "secret", "password", "passphrase", "passwd", "pwd", "token", "bearer",
            "jwt", "authorization", "authstring", "apikey", "assertion", "credential",
            "verifier", "cookie", "session", "signature", "privatekey");

    private static readonly HashSet<string> NonSecretTailWords = new(StringComparer.Ordinal)
    {
        "amount", "at", "consent", "count", "duration", "expired", "level",
        "path", "policy", "remaining", "status", "type",
    };

    private static readonly HashSet<string> NonSecretLeadWords = new(StringComparer.Ordinal)
    {
        "has", "is", "was",
    };

    [GeneratedRegex("[^A-Za-z0-9]+|(?<=[a-z0-9])(?=[A-Z])")]
    private static partial Regex KeyWordBoundary();

    [GeneratedRegex("[A-Za-z][A-Za-z0-9+.\\-]{0,31}://[^\\s<>\"']*")]
    private static partial Regex UrlRun();

    private static readonly Regex PathQueryRun =
        new("/[^\\s<>\"'?#]*[?#][^\\s<>\"']+", RegexOptions.NonBacktracking);

    private static readonly Regex JsonName =
        new("\"([^\"\\\\]*(?:\\\\.[^\"\\\\]*)*)\"(\\s*:\\s*)", RegexOptions.NonBacktracking);

    [GeneratedRegex("\\\\\"([^\"\\\\]*(?:\\\\[^\"][^\"\\\\]*)*)\\\\\"(\\s*:\\s*)")]
    private static partial Regex JsonNameEscaped();

    [GeneratedRegex("[,}\\]\\s]")]
    private static partial Regex ScalarEnd();

    private static string ScrubJsonPairs(string text) =>
        text.IndexOf('"') < 0
            ? text
            : ScrubPairs(JsonNameEscaped(), ScrubPairs(JsonName, text, "\""), "\\\"");

    private static string ScrubPairs(Regex pattern, string text, string quote)
    {
        StringBuilder? rewritten = null;
        var cursor = 0;
        var position = 0;
        while (true)
        {
            var match = pattern.Match(text, position);
            if (!match.Success)
                break;
            if (!ShouldRedact(match.Groups[1].Value))
            {
                // Resume inside the match: an escaped quote in the name can start another pair.
                position = match.Index + 1;
                continue;
            }
            var valueAt = match.Index + match.Length;
            var end = EndOfJsonValue(text, valueAt, quote);
            (rewritten ??= new StringBuilder(text.Length))
                .Append(text, cursor, valueAt - cursor)
                .Append(quote).Append(Redacted).Append(quote);
            cursor = end;
            position = end;
        }
        return rewritten is null ? text : rewritten.Append(text, cursor, text.Length - cursor).ToString();
    }

    private static int EndOfJsonValue(string text, int start, string quote)
    {
        if (start >= text.Length)
            return start;
        if (StartsAt(text, start, quote))
            return EndOfJsonString(text, start, quote);
        if (text[start] is '{' or '[')
        {
            var depth = 0;
            var index = start;
            while (index < text.Length)
            {
                if (StartsAt(text, index, quote))
                {
                    index = EndOfJsonString(text, index, quote);
                    continue;
                }
                switch (text[index])
                {
                    case '{' or '[':
                        depth++;
                        break;
                    case '}' or ']':
                        if (--depth == 0) return index + 1;
                        break;
                }
                index++;
            }
            return text.Length;
        }
        var scalarEnd = ScalarEnd().Match(text, start);
        return scalarEnd.Success ? scalarEnd.Index : text.Length;
    }

    private static int EndOfJsonString(string text, int start, string quote)
    {
        var index = start + quote.Length;
        while (index < text.Length)
        {
            // Delimiter first: \" begins with a backslash, so escape-first would skip the closing quote.
            if (StartsAt(text, index, quote))
                return index + quote.Length;
            if (text[index] == '\\')
            {
                index += 2;
                continue;
            }
            index++;
        }
        return text.Length;
    }

    public static bool ShouldRedact(string key)
    {
        var words = KeyWords(key);
        if (words.Count > 0 &&
            (NonSecretLeadWords.Contains(words[0]) || NonSecretTailWords.Contains(words[^1])))
            return false;
        var normalised = string.Concat(words);
        foreach (var marker in SensitiveKeyMarkers)
            if (normalised.Contains(marker, StringComparison.Ordinal))
                return true;
        return false;
    }

    /// <summary>Deep-copy a value with secret-named entries replaced by <see cref="Redacted"/>.</summary>
    public static object? RedactValue(object? value) =>
        Redact(value, 0, new HashSet<object>(ReferenceEqualityComparer.Instance));

    private static object? Redact(object? value, int depth, HashSet<object> seen)
    {
        switch (value)
        {
            case IReadOnlyDictionary<string, object?> map:
                if (depth >= MaxDepth) return TooDeep;
                if (!seen.Add(map)) return Circular;
                try
                {
                    var copy = new Dictionary<string, object?>();
                    foreach (var (key, item) in map)
                        copy[key] = ShouldRedact(key) ? Redacted : Redact(item, depth + 1, seen);
                    return copy;
                }
                finally { seen.Remove(map); }

            case System.Collections.IDictionary raw:
                // A mapping typed other than object would fall to the sequence arm and print in clear.
                if (depth >= MaxDepth) return TooDeep;
                if (!seen.Add(raw)) return Circular;
                try
                {
                    var copy = new Dictionary<string, object?>();
                    foreach (System.Collections.DictionaryEntry entry in raw)
                    {
                        // Invariant: a non-string key must spell itself the same on every host.
                        var key = Convert.ToString(entry.Key, CultureInfo.InvariantCulture) ?? "";
                        copy[key] = ShouldRedact(key) ? Redacted : Redact(entry.Value, depth + 1, seen);
                    }
                    return copy;
                }
                finally { seen.Remove(raw); }

            case byte[]:
                // Ahead of the sequence arm, or an export body is walked byte by byte.
                return value;

            case System.Collections.IEnumerable list when value is not string:
                if (depth >= MaxDepth) return TooDeep;
                if (!seen.Add(list)) return Circular;
                try
                {
                    var mapping = RedactGenericMapping(list, depth, seen);
                    if (mapping is not null) return mapping;
                    var copy = new List<object?>();
                    foreach (var entry in list)
                        copy.Add(RedactEntry(entry, depth + 1, seen));
                    return copy;
                }
                finally { seen.Remove(list); }

            case string text:
                return RedactText(RedactJsonText(text, depth, seen));

            case Uri uri:
                return RedactText(uri.ToString());

            default:
                return RedactUnwalkable(value, depth, seen);
        }
    }

    // A generic-interface-only mapping, such as ExpandoObject, would otherwise print in clear.
    private static Dictionary<string, object?>? RedactGenericMapping(
        System.Collections.IEnumerable value, int depth, HashSet<object> seen)
    {
        var pairType = MappingPairTypes.GetOrAdd(value.GetType(), FindMappingPairType);
        if (pairType is null) return null;
        var keyProperty = pairType.GetProperty("Key")!;
        var valueProperty = pairType.GetProperty("Value")!;
        var copy = new Dictionary<string, object?>();
        foreach (var entry in value)
        {
            if (!pairType.IsInstanceOfType(entry)) return null;
            var key = Convert.ToString(keyProperty.GetValue(entry), CultureInfo.InvariantCulture) ?? "";
            copy[key] = ShouldRedact(key)
                ? Redacted
                : Redact(valueProperty.GetValue(entry), depth + 1, seen);
        }
        return copy;
    }

    private static readonly System.Collections.Concurrent.ConcurrentDictionary<Type, Type?>
        MappingPairTypes = new();

    private static Type? FindMappingPairType(Type type)
    {
        foreach (var contract in type.GetInterfaces())
        {
            if (!contract.IsGenericType) continue;
            var definition = contract.GetGenericTypeDefinition();
            if (definition != typeof(IDictionary<,>) && definition != typeof(IReadOnlyDictionary<,>))
                continue;
            return typeof(KeyValuePair<,>).MakeGenericType(contract.GetGenericArguments());
        }
        return null;
    }

    private static object? RedactUnwalkable(object? value, int depth, HashSet<object> seen)
    {
        if (value is null || IsImmutableScalar(value))
            return value;
        if (value is Stream)
            return DescribeOpaque(value);
        if (depth >= MaxDepth) return TooDeep;
        try
        {
            var probe = JsonSerializer.SerializeToUtf8Bytes(value, DisplayJson);
            var head = 0;
            while (head < probe.Length && probe[head] is 0x20 or 0x09 or 0x0A or 0x0D)
                head++;
            if (head >= probe.Length)
                return DescribeOpaque(value);
            if (probe[head] == (byte)'"')
            {
                using var literal = JsonDocument.Parse(probe, DeepParse);
                var text = literal.RootElement.GetString();
                return text is null ? DescribeOpaque(value) : Redact(text, depth, seen);
            }
            if (probe[head] != (byte)'{' && probe[head] != (byte)'[')
            {
                using var scalar = JsonDocument.Parse(probe, DeepParse);
                return Materialise(scalar.RootElement);
            }
            using var document = JsonDocument.Parse(probe, DeepParse);
            return Redact(Materialise(document.RootElement), depth, seen);
        }
        catch (Exception)
        {
            return RedactionFailed;
        }
    }

    private static string DescribeOpaque(object value) =>
        value is Stream { CanSeek: true } stream
            ? "<" + value.GetType().Name + ", "
                + stream.Length.ToString(CultureInfo.InvariantCulture) + " bytes>"
            : "<" + value.GetType().Name + ">";

    private static string DescribeUnparsedJson(string text) =>
        UnparsedJson + " " + (text.TrimStart().StartsWith('[') ? "array" : "object") + ", "
        + text.Length.ToString(CultureInfo.InvariantCulture) + " chars>";

    private static bool IsImmutableScalar(object value)
    {
        var type = value.GetType();
        return type.IsPrimitive || type.IsEnum
            || value is decimal or DateTime or DateTimeOffset or DateOnly or TimeOnly
                     or TimeSpan or Guid or System.Numerics.BigInteger;
    }

    private static string RedactJsonText(string text, int depth, HashSet<object> seen)
    {
        var trimmed = text.TrimStart();
        if (!trimmed.StartsWith('{') && !trimmed.StartsWith('['))
            return text;
        object? parsed;
        try
        {
            var fold = FoldNonFiniteLiterals(text, out var candidate, out var sentinel);
            if (fold == NonFiniteFold.NoSentinel)
                return RedactionFailed;
            using var document = JsonDocument.Parse(candidate ?? text, DeepParse);
            parsed = Materialise(document.RootElement);
            if (fold == NonFiniteFold.Folded)
                parsed = RestoreNonFinite(parsed, sentinel);
        }
        catch (JsonException)
        {
            return DescribeUnparsedJson(text);
        }
        var redacted = Redact(parsed, depth + 1, seen);
        try
        {
            var before = SerialiseCanonical(parsed);
            var after = SerialiseCanonical(redacted);
            return before == after ? text : after;
        }
        catch (Exception error) when (error is JsonException or NotSupportedException)
        {
            return RedactionFailed;
        }
    }

    private const string NanLiteral = "NaN";
    private const string InfinityLiteral = "Infinity";

    private enum NonFiniteFold
    {
        None,

        Folded,

        NoSentinel,
    }

    private static NonFiniteFold FoldNonFiniteLiterals(string text, out string? folded, out char sentinel)
    {
        folded = null;
        sentinel = '\0';
        if (!text.Contains(NanLiteral, StringComparison.Ordinal)
            && !text.Contains(InfinityLiteral, StringComparison.Ordinal))
            return NonFiniteFold.None;

        List<(int At, int Width, char Marker)>? found = null;
        var inString = false;
        var escaped = false;
        for (var index = 0; index < text.Length; index++)
        {
            var character = text[index];
            if (inString)
            {
                if (escaped) escaped = false;
                else if (character == '\\') escaped = true;
                else if (character == '"') inString = false;
                continue;
            }
            if (character == '"')
            {
                inString = true;
                continue;
            }
            char marker;
            int width;
            if (StartsAt(text, index, NanLiteral)) { marker = 'N'; width = NanLiteral.Length; }
            else if (StartsAt(text, index, InfinityLiteral)) { marker = 'P'; width = InfinityLiteral.Length; }
            else if (character == '-' && StartsAt(text, index + 1, InfinityLiteral))
            { marker = 'M'; width = InfinityLiteral.Length + 1; }
            else continue;
            (found ??= new List<(int, int, char)>()).Add((index, width, marker));
            index += width - 1;
        }
        if (found is null)
            return NonFiniteFold.None;

        sentinel = FreeSentinel(text);
        if (sentinel == '\0')
            return NonFiniteFold.NoSentinel;

        var rewritten = new StringBuilder(text.Length);
        var copied = 0;
        foreach (var (at, width, marker) in found)
        {
            rewritten.Append(text, copied, at - copied)
                .Append('"').Append(sentinel).Append(marker).Append('"');
            copied = at + width;
        }
        folded = rewritten.Append(text, copied, text.Length - copied).ToString();
        return NonFiniteFold.Folded;
    }

    private static bool StartsAt(string text, int at, string literal) =>
        at >= 0 && at + literal.Length <= text.Length
        && string.CompareOrdinal(text, at, literal, 0, literal.Length) == 0;

    private static char FreeSentinel(string text)
    {
        for (var candidate = '\uE000'; candidate <= '\uF8FF'; candidate++)
        {
            if (text.IndexOf(candidate) >= 0)
                continue;
            var escape = "\\u" + ((int)candidate).ToString("x4", CultureInfo.InvariantCulture);
            if (text.Contains(escape, StringComparison.OrdinalIgnoreCase))
                continue;
            return candidate;
        }
        return '\0';
    }

    private static object? RestoreNonFinite(object? value, char sentinel)
    {
        switch (value)
        {
            case string text when text.Length == 2 && text[0] == sentinel:
                return text[1] switch
                {
                    'N' => double.NaN,
                    'P' => double.PositiveInfinity,
                    _ => double.NegativeInfinity,
                };

            case Dictionary<string, object?> map:
                foreach (var key in map.Keys.ToList())
                    map[key] = RestoreNonFinite(map[key], sentinel);
                return map;

            case List<object?> list:
                for (var index = 0; index < list.Count; index++)
                    list[index] = RestoreNonFinite(list[index], sentinel);
                return list;

            default:
                return value;
        }
    }

    private static string SerialiseCanonical(object? value)
    {
        var builder = new StringBuilder();
        WriteCanonical(builder, value);
        return builder.ToString();
    }

    private static void WriteCanonical(StringBuilder builder, object? value)
    {
        switch (value)
        {
            case IReadOnlyDictionary<string, object?> map:
                builder.Append('{');
                var firstEntry = true;
                foreach (var (key, item) in map)
                {
                    if (!firstEntry) builder.Append(", ");
                    firstEntry = false;
                    WriteCanonical(builder, key);
                    builder.Append(": ");
                    WriteCanonical(builder, item);
                }
                builder.Append('}');
                return;

            case string text:
                // Not the display serializer: built-in encoders escape characters outside the BMP.
                WriteCanonicalStringLiteral(builder, text);
                return;

            case double number:
                // Not "R", which renders 100.0 as "100" and 1e308 as "1E+308".
                builder.Append(CanonicalReprDouble(number));
                return;

            case System.Numerics.BigInteger big:
                builder.Append(big.ToString(CultureInfo.InvariantCulture));
                return;

            case System.Collections.IEnumerable list:
                builder.Append('[');
                var firstItem = true;
                foreach (var entry in list)
                {
                    if (!firstItem) builder.Append(", ");
                    firstItem = false;
                    WriteCanonical(builder, entry);
                }
                builder.Append(']');
                return;

            default:
                builder.Append(JsonSerializer.Serialize(value, DisplayJson));
                return;
        }
    }

    private static void WriteCanonicalStringLiteral(StringBuilder builder, string text)
    {
        builder.Append('"');
        foreach (var character in text)
        {
            switch (character)
            {
                case '"': builder.Append("\\\""); break;
                case '\\': builder.Append("\\\\"); break;
                case '\n': builder.Append("\\n"); break;
                case '\r': builder.Append("\\r"); break;
                case '\t': builder.Append("\\t"); break;
                case '\b': builder.Append("\\b"); break;
                case '\f': builder.Append("\\f"); break;
                default:
                    if (character < 0x20)
                        builder.Append("\\u").Append(((int)character).ToString("x4", CultureInfo.InvariantCulture));
                    else
                        builder.Append(character);
                    break;
            }
        }
        builder.Append('"');
    }

    private static string CanonicalReprDouble(double value)
    {
        // First: the fixed-notation branch would turn "Infinity" into Infinity.0.
        if (double.IsNaN(value)) return "NaN";
        if (double.IsPositiveInfinity(value)) return "Infinity";
        if (double.IsNegativeInfinity(value)) return "-Infinity";
        var shortest = value.ToString("R", CultureInfo.InvariantCulture);
        var negative = shortest.StartsWith('-');
        if (negative)
            shortest = shortest[1..];
        var marker = shortest.IndexOfAny(new[] { 'e', 'E' });
        var exponent = 0;
        if (marker >= 0)
        {
            exponent = int.Parse(shortest[(marker + 1)..], CultureInfo.InvariantCulture);
            shortest = shortest[..marker];
        }
        var dot = shortest.IndexOf('.');
        var digits = dot < 0 ? shortest : shortest.Remove(dot, 1);
        var pointAt = (dot < 0 ? shortest.Length : dot) + exponent;
        var lead = 0;
        while (lead < digits.Length - 1 && digits[lead] == '0')
        {
            lead++;
            pointAt--;
        }
        digits = digits[lead..].TrimEnd('0');
        if (digits.Length == 0 || digits == "0")
        {
            digits = "0";
            pointAt = 1;
        }

        var sign = negative ? "-" : "";
        if (pointAt <= -4 || pointAt > 16)
        {
            var mantissa = digits.Length > 1 ? digits[..1] + "." + digits[1..] : digits;
            var outputExponent = pointAt - 1;
            return sign + mantissa + "e" + (outputExponent < 0 ? "-" : "+")
                + Math.Abs(outputExponent).ToString("00", CultureInfo.InvariantCulture);
        }
        if (pointAt <= 0)
            return sign + "0." + new string('0', -pointAt) + digits;
        if (pointAt >= digits.Length)
            return sign + digits + new string('0', pointAt - digits.Length) + ".0";
        return sign + digits[..pointAt] + "." + digits[pointAt..];
    }

    private static object? RedactEntry(object? entry, int depth, HashSet<object> seen)
    {
        if (entry is IReadOnlyList<object?> { Count: 2 } pair &&
            pair[0] is string name && ShouldRedact(name))
            return new List<object?> { name, Redacted };
        return Redact(entry, depth, seen);
    }

    public static Dictionary<string, string> RedactMapping(IReadOnlyDictionary<string, string>? headers)
    {
        var shown = new Dictionary<string, string>();
        if (headers is null) return shown;
        foreach (var (key, value) in headers)
            shown[key] = ShouldRedact(key) ? Redacted : RedactText(value);
        return shown;
    }

    internal static object? Detach(object? value) =>
        DetachValue(value, 0, new HashSet<object>(ReferenceEqualityComparer.Instance));

    internal static IReadOnlyDictionary<string, object?>? DetachMap(
        IReadOnlyDictionary<string, object?>? source)
    {
        if (source is null)
            return null;
        var entries = new List<KeyValuePair<string, object?>>();
        foreach (var (key, value) in source)
            entries.Add(new KeyValuePair<string, object?>(key, Detach(value)));
        return DetachedMap<object?>.Of(entries);
    }

    internal static IReadOnlyDictionary<string, object?> DetachedMapOf(
        List<KeyValuePair<string, object?>> entries) => DetachedMap<object?>.Of(entries);

    internal static IReadOnlyDictionary<string, string> DetachedTextOf(
        List<KeyValuePair<string, string>> entries) => DetachedMap<string>.Of(entries);

    internal static IReadOnlyDictionary<string, string>? DetachText(
        IReadOnlyDictionary<string, string>? source)
    {
        if (source is null)
            return null;
        var entries = new List<KeyValuePair<string, string>>();
        foreach (var (key, value) in source)
            entries.Add(new KeyValuePair<string, string>(key, value));
        return DetachedMap<string>.Of(entries);
    }

    internal static object? CopyBuffers(object? value) =>
        HoldsBuffer(value, 0) ? Detach(value) : value;

    internal static IReadOnlyDictionary<string, object?>? CopyBuffersInMap(
        IReadOnlyDictionary<string, object?>? snapshot) =>
        snapshot is not null && HoldsBuffer(snapshot, 0) ? DetachMap(snapshot) : snapshot;

    private static bool HoldsBuffer(object? value, int depth)
    {
        if (depth >= MaxDepth)
            return false;
        switch (value)
        {
            case byte[]:
                return true;

            case null:
            case string:
                return false;

            case IReadOnlyDictionary<string, object?> map:
                foreach (var (_, item) in map)
                    if (HoldsBuffer(item, depth + 1))
                        return true;
                return false;

            case System.Collections.IDictionary raw:
                foreach (System.Collections.DictionaryEntry entry in raw)
                    if (HoldsBuffer(entry.Value, depth + 1))
                        return true;
                return false;

            case System.Collections.IEnumerable list:
                foreach (var entry in list)
                    if (HoldsBuffer(entry, depth + 1))
                        return true;
                return false;

            default:
                return false;
        }
    }

    internal static IReadOnlyDictionary<string, object?> EmptyMap { get; } =
        DetachedMap<object?>.Of(new List<KeyValuePair<string, object?>>());

    internal static IReadOnlyDictionary<string, string> EmptyText { get; } =
        DetachedMap<string>.Of(new List<KeyValuePair<string, string>>());

    private static object? DetachValue(object? value, int depth, HashSet<object> seen)
    {
        switch (value)
        {
            case null:
            case string:
                return value;

            case byte[] bytes:
                return bytes.Length == 0 ? Array.Empty<byte>() : bytes.AsSpan().ToArray();

            case IReadOnlyDictionary<string, object?> map:
                if (depth >= MaxDepth) return TooDeep;
                if (!seen.Add(map)) return Circular;
                try
                {
                    var entries = new List<KeyValuePair<string, object?>>();
                    foreach (var (key, item) in map)
                        entries.Add(new KeyValuePair<string, object?>(
                            key, DetachValue(item, depth + 1, seen)));
                    return DetachedMap<object?>.Of(entries);
                }
                finally { seen.Remove(map); }

            case System.Collections.IDictionary raw:
                if (depth >= MaxDepth) return TooDeep;
                if (!seen.Add(raw)) return Circular;
                try
                {
                    var entries = new List<KeyValuePair<string, object?>>();
                    foreach (System.Collections.DictionaryEntry entry in raw)
                    {
                        var key = Convert.ToString(entry.Key, CultureInfo.InvariantCulture) ?? "";
                        entries.Add(new KeyValuePair<string, object?>(
                            key, DetachValue(entry.Value, depth + 1, seen)));
                    }
                    return DetachedMap<object?>.Of(entries);
                }
                finally { seen.Remove(raw); }

            case System.Collections.IEnumerable list:
                if (depth >= MaxDepth) return TooDeep;
                if (!seen.Add(list)) return Circular;
                try
                {
                    var builder = System.Collections.Immutable.ImmutableArray.CreateBuilder<object?>();
                    foreach (var entry in list)
                        builder.Add(DetachValue(entry, depth + 1, seen));
                    return builder.ToImmutable();
                }
                finally { seen.Remove(list); }

            default:
                return value;
        }
    }

    private sealed class DetachedMap<TValue> : IReadOnlyDictionary<string, TValue>
    {
        private readonly System.Collections.Immutable.ImmutableArray<KeyValuePair<string, TValue>> _entries;
        private readonly System.Collections.Immutable.ImmutableDictionary<string, TValue> _byKey;

        private DetachedMap(
            System.Collections.Immutable.ImmutableArray<KeyValuePair<string, TValue>> entries,
            System.Collections.Immutable.ImmutableDictionary<string, TValue> byKey)
        {
            _entries = entries;
            _byKey = byKey;
        }

        internal static DetachedMap<TValue> Of(List<KeyValuePair<string, TValue>> entries)
        {
            var positions = new Dictionary<string, int>(entries.Count, StringComparer.Ordinal);
            var ordered = new List<KeyValuePair<string, TValue>>(entries.Count);
            foreach (var entry in entries)
            {
                if (positions.TryGetValue(entry.Key, out var at))
                    ordered[at] = entry;
                else
                {
                    positions[entry.Key] = ordered.Count;
                    ordered.Add(entry);
                }
            }
            var byKey = System.Collections.Immutable.ImmutableDictionary
                .CreateBuilder<string, TValue>(StringComparer.Ordinal);
            foreach (var entry in ordered)
                byKey[entry.Key] = entry.Value;
            return new DetachedMap<TValue>(
                System.Collections.Immutable.ImmutableArray.CreateRange(ordered), byKey.ToImmutable());
        }

        public TValue this[string key] => _byKey[key];

        public IEnumerable<string> Keys
        {
            get
            {
                foreach (var entry in _entries)
                    yield return entry.Key;
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (var entry in _entries)
                    yield return entry.Value;
            }
        }

        public int Count => _entries.Length;

        public bool ContainsKey(string key) => _byKey.ContainsKey(key);

        public bool TryGetValue(string key, out TValue value) => _byKey.TryGetValue(key, out value!);

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            foreach (var entry in _entries)
                yield return entry;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>Redact every URL and schemeless <c>path?query</c> inside free text.</summary>
    public static string RedactText(string value)
    {
        if (value is null) return null!;
        var cleaned = value;
        if (cleaned.Contains("://", StringComparison.Ordinal))
            cleaned = UrlRun().Replace(cleaned, match => RedactOneUrl(match.Value));
        if (cleaned.IndexOf('?') >= 0 || cleaned.IndexOf('#') >= 0)
            cleaned = PathQueryRun.Replace(cleaned, match => RedactOneUrl(match.Value));
        return cleaned;
    }

    /// <summary>A URL with every query value withheld and every name kept, for always-on logs.</summary>
    public static string? UrlForLog(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return url;
        var mark = url.IndexOf('?');
        if (mark < 0)
            return RedactText(url);
        return RedactText(url[..mark]) + "?" + RedactQueryValues(url[(mark + 1)..], _ => true);
    }

    /// <summary>Render <c>name=value</c> pairs for a <c>ToString</c>, redacting secret-named ones.</summary>
    public static IReadOnlyList<string> RedactFields(
        object instance, IEnumerable<string> fieldNames)
    {
        var shown = new List<string>();
        foreach (var name in fieldNames)
        {
            var property = instance.GetType().GetProperty(name);
            var value = property?.GetValue(instance);
            var rendered = value is not null && ShouldRedact(name)
                ? Redacted
                : RedactValue(value) switch
                {
                    null => "null",
                    string text => RedactText(text),
                    var other => other.ToString() ?? "null",
                };
            shown.Add($"{ToCamelCase(name)}='{rendered}'");
        }
        return shown;
    }

    private static string ToCamelCase(string name) =>
        name.Length == 0 ? name : char.ToLowerInvariant(name[0]) + name[1..];

    internal static string RedactMessageText(string message) =>
        string.IsNullOrEmpty(message) ? message : RedactText(ScrubJsonPairs(message));

    private static string RedactOneUrl(string url)
    {
        var rest = url;
        var prefix = "";
        var authorityChanged = false;
        var schemeEnd = rest.IndexOf("://", StringComparison.Ordinal);
        if (schemeEnd >= 0)
        {
            var authorityEnd = rest.IndexOfAny(AuthorityEnd, schemeEnd + 3);
            if (authorityEnd < 0)
                authorityEnd = rest.Length;
            var authority = rest[(schemeEnd + 3)..authorityEnd];
            var shownAuthority = RedactUserinfo(authority);
            authorityChanged = shownAuthority != authority;
            prefix = rest[..(schemeEnd + 3)] + shownAuthority;
            rest = rest[authorityEnd..];
        }
        else if (rest.StartsWith("//", StringComparison.Ordinal))
        {
            var authorityEnd = rest.IndexOfAny(AuthorityEnd, 2);
            if (authorityEnd < 0)
                authorityEnd = rest.Length;
            var authority = rest[2..authorityEnd];
            var shownAuthority = RedactUserinfo(authority);
            authorityChanged = shownAuthority != authority;
            prefix = "//" + shownAuthority;
            rest = rest[authorityEnd..];
        }
        var hash = rest.IndexOf('#');
        var fragment = hash >= 0 ? rest[(hash + 1)..] : null;
        var head = hash >= 0 ? rest[..hash] : rest;
        var mark = head.IndexOf('?');
        var query = mark >= 0 ? head[(mark + 1)..] : null;
        var path = mark >= 0 ? head[..mark] : head;
        var shownQuery = query is null ? null : RedactQueryString(query);
        var shownFragment = fragment is null ? null : RedactQueryString(fragment);
        if (!authorityChanged && shownQuery == query && shownFragment == fragment)
            return url;
        var rebuilt = new StringBuilder(prefix).Append(path);
        if (!string.IsNullOrEmpty(shownQuery))
            rebuilt.Append('?').Append(shownQuery);
        if (!string.IsNullOrEmpty(shownFragment))
            rebuilt.Append('#').Append(shownFragment);
        return rebuilt.ToString();
    }

    private static readonly char[] AuthorityEnd = { '/', '?', '#' };

    private static string RedactUserinfo(string authority)
    {
        var at = authority.LastIndexOf('@');
        if (at < 0) return authority;
        var userinfo = authority[..at];
        var colon = userinfo.IndexOf(':');
        if (colon < 0) return authority;
        return userinfo[..colon] + ":" + Redacted + authority[at..];
    }

    internal static string RedactQueryString(string query) =>
        string.IsNullOrEmpty(query)
            ? query
            : RedactQueryValues(query, name => ShouldRedact(HttpUtility.UrlDecode(name)));

    private static string RedactQueryValues(string query, Func<string, bool> isSecret)
    {
        var chunks = query.Split('&');
        for (var index = 0; index < chunks.Length; index++)
        {
            var equals = chunks[index].IndexOf('=');
            if (equals >= 0 && isSecret(chunks[index][..equals]))
                chunks[index] = chunks[index][..equals] + "=" + Redacted;
        }
        return string.Join("&", chunks);
    }

    private static List<string> KeyWords(string key)
    {
        var words = new List<string>();
        foreach (var word in KeyWordBoundary().Split(key ?? ""))
            if (word.Length > 0)
                words.Add(word.ToLowerInvariant());
        return words;
    }

    internal static object? Materialise(JsonElement element) => element.ValueKind switch
    {
        JsonValueKind.Object => MaterialiseObject(element),
        JsonValueKind.Array => MaterialiseArray(element),
        JsonValueKind.String => element.GetString(),
        JsonValueKind.Number => ReadNumber(element),
        JsonValueKind.True => true,
        JsonValueKind.False => false,
        _ => null,
    };

    // Boxed separately: a long/double ternary would turn every integer into a double.
    private static object ReadNumber(JsonElement element)
    {
        if (element.TryGetInt64(out var longValue))
            return longValue;
        var raw = element.GetRawText();
        // An integer wider than Int64 stays exact; GetDouble() would drop the low digits.
        if (raw.AsSpan().IndexOfAny('.', 'e', 'E') < 0
            && System.Numerics.BigInteger.TryParse(
                raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var big))
            return big;
        return element.GetDouble();
    }

    private static Dictionary<string, object?> MaterialiseObject(JsonElement element)
    {
        var map = new Dictionary<string, object?>();
        foreach (var property in element.EnumerateObject())
            map[property.Name] = Materialise(property.Value);
        return map;
    }

    private static List<object?> MaterialiseArray(JsonElement element)
    {
        var list = new List<object?>();
        foreach (var item in element.EnumerateArray())
            list.Add(Materialise(item));
        return list;
    }
}
