using System.Collections;
using System.Globalization;

using WebCommander.Errors;

namespace WebCommander.Http;

/// <summary>Query normalisation: every value leaves as its wire text; names pass through untouched.</summary>
public static class QueryParams
{
    /// <summary>Normalise values to their wire text: <c>null</c> dropped, an enumerable repeats the key.</summary>
    public static Dictionary<string, object?> BuildQueryParams(
        IReadOnlyDictionary<string, object?>? parameters)
    {
        var normalised = new Dictionary<string, object?>();
        if (parameters is null) return normalised;
        foreach (var (key, value) in parameters)
        {
            if (value is null) continue;
            normalised[key] = Normalise(value, key, 0);
        }
        return normalised;
    }

    private static object? Normalise(object value, string key, int depth)
    {
        if (value is IReadOnlyDictionary<string, object?> or IDictionary)
            throw new SdkUsageError(Redaction.RedactMessageText(
                $"query param '{key}': nested objects cannot be encoded in a query " +
                "string; flatten it or send it in the body"));

        // Ahead of the sequence arm, because a string is one: a scalar is never exploded.
        if (WireText(value) is { } text)
            return text;

        if (value is IEnumerable enumerable)
        {
            if (depth >= 1)
                throw new SdkUsageError(Redaction.RedactMessageText(
                    $"query param '{key}': a sequence inside a sequence has no " +
                    "query-string encoding; flatten it to one list of scalars"));
            var entries = new List<object?>();
            foreach (var entry in enumerable)
                if (entry is not null)
                    entries.Add(Normalise(entry, key, depth + 1));
            return entries;
        }

        throw new SdkUsageError(Redaction.RedactMessageText(RefuseValue("query param", key, value)));
    }

    /// <summary>The one refusal for an unspellable value: the type is named, never the value.</summary>
    internal static string RefuseValue(string what, string key, object value) =>
        $"{what} '{key}': a {value.GetType().Name} has no wire spelling this SDK will guess "
        + "at. Its ToString() is the calling program's own code, so the text sent and the text "
        + "recorded could disagree, and could differ again between two attempts at the same "
        + "request. Pass a string, a number, a bool, an enum, a date or time, a TimeSpan, a "
        + "Guid, a Uri - or, for a query parameter, a list of those";

    /// <summary>The one scalar-to-wire-text table, invariant; <c>null</c> for a shape it cannot spell.</summary>
    internal static string? WireText(object value) => value switch
    {
        string text => text,
        bool flag => flag ? "true" : "false",
        // Ahead of the enum arm, pinning the wire form if a member is renamed.
        SortDirection direction => direction == SortDirection.Asc ? "asc" : "desc",
        Enum enumValue => enumValue.ToString().ToLowerInvariant(),
        DateTime dateTime => IsoTime(dateTime),
        DateTimeOffset dateTimeOffset => dateTimeOffset.ToString(IsoWithOffset, CultureInfo.InvariantCulture),
        DateOnly date => date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        TimeOnly time => time.ToString("HH:mm:ss.FFFFFF", CultureInfo.InvariantCulture),
        // "c" named, so the wire spelling is not the framework's to change.
        TimeSpan span => span.ToString("c", CultureInfo.InvariantCulture),
        Guid guid => guid.ToString("D", CultureInfo.InvariantCulture),
        Uri uri => uri.ToString(),
        char character => character.ToString(),
        sbyte or byte or short or ushort or int or uint or long or ulong
            or float or double or decimal or System.Numerics.BigInteger =>
            ((IFormattable)value).ToString(null, CultureInfo.InvariantCulture),
        _ => null,
    };

    private const string Iso = "yyyy-MM-ddTHH:mm:ss.FFFFFF";
    private const string IsoWithOffset = Iso + "zzz";

    /// <summary>A DateTime with its offset: a UTC value without +00:00 is read as store-local time.</summary>
    private static string IsoTime(DateTime value) => value.Kind switch
    {
        // Not through DateTimeOffset, which reads an Unspecified value as local time.
        DateTimeKind.Utc => value.ToString(Iso, CultureInfo.InvariantCulture) + "+00:00",
        DateTimeKind.Local => new DateTimeOffset(value).ToString(IsoWithOffset, CultureInfo.InvariantCulture),
        _ => value.ToString(Iso, CultureInfo.InvariantCulture),
    };
}
