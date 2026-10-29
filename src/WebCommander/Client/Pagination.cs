using System.Globalization;
using System.Runtime.CompilerServices;

using WebCommander.Errors;
using WebCommander.Http;

namespace WebCommander.Client;

/// <summary>Lazily walks a paginated listing; the end is an absent <c>next_page</c>, not an empty page.</summary>
public static class Pagination
{
    public static async IAsyncEnumerable<ApiResponse> IterPagesAsync(
        WebCommanderClient client,
        string path,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        int? maxPages = null,
        bool raiseOnError = true,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (maxPages is < 1)
            throw new SdkUsageError(Redaction.RedactMessageText("maxPages: expected a positive integer or null, got "
                + maxPages.Value.ToString(CultureInfo.InvariantCulture)));

        var visited = new HashSet<string>(StringComparer.Ordinal);
        var yielded = 0;

        var response = await client.SendGetRequestAsync(
            path, new RequestOptions { QueryParams = queryParams, RaiseOnError = raiseOnError }, cancellationToken)
            .ConfigureAwait(false);
        visited.Add(CycleKey(client, path, queryParams));

        while (true)
        {
            yield return response;
            yielded++;

            if (!response.Succeeded)
                yield break;
            if (maxPages is int cap && yielded >= cap)
                yield break;

            var nextPage = ReadNextPage(response.Map);
            if (nextPage is null)
                yield break;
            if (!visited.Add(CycleKey(client, nextPage, null)))
                // The link is not in the message: it carries the store's host and cursor values.
                throw new SdkError(Redaction.RedactMessageText("pagination loop: page "
                    + yielded.ToString(CultureInfo.InvariantCulture) + " of this listing carries a "
                    + "next_page link already followed, so following it again would never "
                    + "terminate - report this listing to the store; the pages already yielded "
                    + "are good"));

            response = await client.SendGetRequestAsync(
                nextPage, new RequestOptions { RaiseOnError = raiseOnError }, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    /// <summary>Each record of a listing, across every page, from the list under the named key.</summary>
    public static async IAsyncEnumerable<IReadOnlyDictionary<string, object?>> IterRecordsAsync(
        WebCommanderClient client,
        string path,
        string recordKey,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        int? maxPages = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(recordKey))
            throw new SdkUsageError(Redaction.RedactMessageText("recordKey: expected the envelope key holding the records, "
                + $"like 'customers', got '{recordKey}'"));

        await foreach (var page in IterPagesAsync(client, path, queryParams, maxPages, true, cancellationToken)
            .ConfigureAwait(false))
        {
            var records = ListUnderKey(page, recordKey)
                ?? throw WrongKeyError(page, recordKey);
            foreach (var record in records)
            {
                // Checked, not cast: an InvalidCastException from an iterator is not an SdkError.
                if (record is not IReadOnlyDictionary<string, object?> mapping)
                    throw new SdkUsageError(Redaction.RedactMessageText($"recordKey '{recordKey}': the list under that key "
                        + $"holds {(record is null ? "null" : record.GetType().Name)} rather than "
                        + "records; name the key that holds the objects"));
                yield return mapping;
            }
        }
    }

    private static string CycleKey(
        WebCommanderClient client, string pathOrUrl, IReadOnlyDictionary<string, object?>? queryParams)
    {
        var resolved = client.ResolvePageUrl(pathOrUrl);
        var parsed = new Uri(Transport.BuildUrl(resolved, QueryParams.BuildQueryParams(queryParams)), UriKind.Absolute);
        var pairs = new List<string>();
        foreach (var (name, value) in UrlBuilder.QueryPairs(parsed.Query.TrimStart('?')))
            pairs.Add(Uri.UnescapeDataString(name) + "\u0000" + Uri.UnescapeDataString(value));
        pairs.Sort(StringComparer.Ordinal);
        return string.Concat(
            parsed.Scheme, "://", parsed.Host, ":",
            parsed.Port.ToString(CultureInfo.InvariantCulture), parsed.AbsolutePath,
            "\u0000", string.Join("\u0001", pairs));
    }

    internal static string? ReadNextPage(object? data)
    {
        if (data is IReadOnlyDictionary<string, object?> body
            && body.GetValueOrDefault("pagination") is IReadOnlyDictionary<string, object?> pagination
            && pagination.GetValueOrDefault("next_page") is string nextPage
            && nextPage.Trim().Length > 0)
        {
            return nextPage;
        }
        return null;
    }

    private static System.Collections.IEnumerable? ListUnderKey(ApiResponse page, string recordKey) =>
        page.Map is IReadOnlyDictionary<string, object?> body
            && body.GetValueOrDefault(recordKey) is System.Collections.IEnumerable records
            && IsRecordList(records)
            ? records
            : null;

    private static bool IsRecordList(System.Collections.IEnumerable records) =>
        records is not string
        && records is not System.Collections.IDictionary
        && records is not IReadOnlyDictionary<string, object?>;

    private static SdkUsageError WrongKeyError(ApiResponse page, string recordKey)
    {
        var available = page.Map is IReadOnlyDictionary<string, object?> body
            ? string.Join(", ", body.Keys.OrderBy(key => key, StringComparer.Ordinal))
            : "a " + (page.Map is null ? "null" : page.Map.GetType().Name) + " body";
        return new SdkUsageError(Redaction.RedactMessageText($"recordKey '{recordKey}': this page has no list under that key; "
            + $"it carries {available}"));
    }
}
