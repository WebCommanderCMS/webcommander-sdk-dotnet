using WebCommander.Client;
using WebCommander.Resources.TaxCountryTaxes.TaxCountryTaxesDto;

namespace WebCommander.Resources.TaxCountryTaxes;

/// <summary>Country taxes - <c>/admin/tax/country-taxes*</c> - as a typed resource bound to one client; no <c>HEAD</c>.</summary>
public sealed class TaxCountryTaxes : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.TaxCountryTaxes</c>.</summary>
    public TaxCountryTaxes(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the country taxes - <c>GET /admin/tax/country-taxes</c>, newest first. <c>limit</c> is 1 to 100
    /// with the cap echoed, <c>offset</c> or <c>page</c> move the window, <c>sort</c> takes <c>id</c> or <c>name</c>.</summary>
    public Task<ApiResponse> ListTaxCountryTaxesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxCountryTaxesUrls.TaxCountryTaxes,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CountryTaxPage),
            }, cancellationToken);

    /// <summary>Create a country tax - <c>POST /admin/tax/country-taxes</c>, answered <c>201</c>. Wrapped in
    /// <c>country_tax</c>; <c>country_code</c> is required and no read sends it.
    /// <paramref name="transactionNo"/> is sent as <c>X-Transaction-No</c> and files the row under that number.</summary>
    public Task<ApiResponse> CreateTaxCountryTaxAsync(
        object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(TaxCountryTaxesUrls.TaxCountryTaxes,
            new RequestOptions
            {
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [TaxCountryTaxesUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CountryTax),
            }, cancellationToken);
    }

    /// <summary>Retrieve one country tax, with an <c>ETag</c> - <c>GET /admin/tax/country-taxes/{country_tax_id}</c>;
    /// only the row path and the update carry <c>state_rules</c>.</summary>
    public Task<ApiResponse> GetTaxCountryTaxAsync(
        object countryTaxId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxCountryTaxesUrls.TaxCountryTax,
            new RequestOptions
            {
                PathParams = Id(countryTaxId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CountryTax),
            }, cancellationToken);

    /// <summary>Update one country tax - <c>PUT /admin/tax/country-taxes/{country_tax_id}</c>; a merge, wrapped in
    /// <c>country_tax</c>, answered <c>200</c> with an <c>ETag</c>.</summary>
    public Task<ApiResponse> UpdateTaxCountryTaxAsync(
        object countryTaxId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(TaxCountryTaxesUrls.TaxCountryTax,
            new RequestOptions
            {
                PathParams = Id(countryTaxId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CountryTax),
            }, cancellationToken);
    }

    /// <summary>Delete one country tax - <c>DELETE /admin/tax/country-taxes/{country_tax_id}</c>; <c>204</c>.</summary>
    public Task<ApiResponse> DeleteTaxCountryTaxAsync(
        object countryTaxId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(TaxCountryTaxesUrls.TaxCountryTax,
            new RequestOptions
            {
                PathParams = Id(countryTaxId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object countryTaxId) =>
        new() { ["country_tax_id"] = countryTaxId };
}
