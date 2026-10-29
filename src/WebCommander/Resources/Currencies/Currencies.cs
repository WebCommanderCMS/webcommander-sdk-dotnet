using WebCommander.Client;
using WebCommander.Resources.Currencies.CurrenciesDto;

namespace WebCommander.Resources.Currencies;

/// <summary>Currencies - <c>/admin/currencies*</c> - as a typed resource bound to one client.</summary>
public sealed class Currencies : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Currencies</c>.</summary>
    public Currencies(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the currencies by ascending <c>id</c> - <c>GET /admin/currencies</c>. <c>search</c> matches
    /// <c>name</c> or <c>code</c>, <c>active</c> filters, <c>sort</c> and <c>dir</c> order, <c>limit</c> is 1 to 100.</summary>
    public Task<ApiResponse> ListCurrenciesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CurrenciesUrls.Currencies,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CurrencyPage),
            }, cancellationToken);

    /// <summary>HEAD the currency collection - <c>HEAD /admin/currencies</c>; no body.</summary>
    public Task<ApiResponse> HeadCurrenciesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CurrenciesUrls.Currencies,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Count the currencies - <c>GET /admin/currencies/count</c>, a bare <c>{"total": N}</c>.
    /// <c>search</c> and <c>active</c> narrow it as they narrow the listing; <c>limit</c> is ignored.</summary>
    public Task<ApiResponse> CountCurrenciesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CurrenciesUrls.CurrenciesCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CurrencyCount),
            }, cancellationToken);

    /// <summary>HEAD the count - <c>HEAD /admin/currencies/count</c>; no body.</summary>
    public Task<ApiResponse> HeadCurrenciesCountAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CurrenciesUrls.CurrenciesCount,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a currency - <c>POST /admin/currencies</c>, answered <c>201</c>. A bare record: <c>name</c>,
    /// <c>code</c> and <c>symbol</c> are required, and <c>name</c> and <c>code</c> must be free.
    /// <paramref name="transactionNo"/> is sent as <c>X-Transaction-No</c> and files the currency under that number.</summary>
    public Task<ApiResponse> CreateCurrencyAsync(
        object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(CurrenciesUrls.Currencies,
            new RequestOptions
            {
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [CurrenciesUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Currency),
            }, cancellationToken);
    }

    /// <summary>Retrieve one currency, with an <c>ETag</c> - <c>GET /admin/currencies/{currency_id}</c>;
    /// <c>field_metadata</c> adds the schema beside it.</summary>
    public Task<ApiResponse> GetCurrencyAsync(
        object currencyId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CurrenciesUrls.Currency,
            new RequestOptions
            {
                PathParams = Id(currencyId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Currency),
            }, cancellationToken);

    /// <summary>HEAD one currency - <c>HEAD /admin/currencies/{currency_id}</c>; no body, the <c>ETag</c> its
    /// <c>GET</c> carries.</summary>
    public Task<ApiResponse> HeadCurrencyAsync(
        object currencyId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CurrenciesUrls.Currency,
            new RequestOptions
            {
                PathParams = Id(currencyId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Replace one currency - <c>PUT /admin/currencies/{currency_id}</c>. <c>name</c>, <c>code</c> and
    /// <c>symbol</c> are required and every field left out is reset to its default; not the base.</summary>
    public Task<ApiResponse> UpdateCurrencyAsync(
        object currencyId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(CurrenciesUrls.Currency,
            new RequestOptions
            {
                PathParams = Id(currencyId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Currency),
            }, cancellationToken);
    }

    /// <summary>Change only the fields sent - <c>PATCH /admin/currencies/{currency_id}</c>. An empty body is
    /// <c>400</c>, and the base currency is read-only here as on <c>PUT</c>.</summary>
    public Task<ApiResponse> PatchCurrencyAsync(
        object currencyId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CurrenciesUrls.Currency,
            new RequestOptions
            {
                PathParams = Id(currencyId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Currency),
            }, cancellationToken);
    }

    /// <summary>Delete a currency - <c>DELETE /admin/currencies/{currency_id}</c>; <c>204</c>. The base
    /// currency is <c>409</c>: make another currency the base first.</summary>
    public Task<ApiResponse> DeleteCurrencyAsync(
        object currencyId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(CurrenciesUrls.Currency,
            new RequestOptions
            {
                PathParams = Id(currencyId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Make an active currency the store's base; takes no body. Every rate is divided by the new
    /// base's old rate. <c>PUT /admin/currencies/{currency_id}/base</c></summary>
    public Task<ApiResponse> SetBaseCurrencyAsync(
        object currencyId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPutRequestAsync(CurrenciesUrls.CurrencyBase,
            new RequestOptions
            {
                PathParams = Id(currencyId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CurrencyBaseChange),
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object currencyId) =>
        new() { ["currency_id"] = currencyId };
}
