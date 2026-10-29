using WebCommander.Client;
using WebCommander.Resources.TaxCodes.TaxCodesDto;

namespace WebCommander.Resources.TaxCodes;

/// <summary>Tax codes - <c>/admin/tax_codes*</c> - as a typed resource bound to one client.</summary>
public sealed class TaxCodes : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.TaxCodes</c>.</summary>
    public TaxCodes(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the tax codes - <c>GET /admin/tax_codes</c>. <c>limit</c> is capped at 100.</summary>
    public Task<ApiResponse> ListTaxCodesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxCodesUrls.TaxCodes,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxCodePage),
            }, cancellationToken);

    /// <summary>HEAD the tax code collection - <c>HEAD /admin/tax_codes</c>.</summary>
    public Task<ApiResponse> HeadTaxCodesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TaxCodesUrls.TaxCodes,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a tax code - <c>POST /admin/tax_codes</c>.</summary>
    public Task<ApiResponse> CreateTaxCodeAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(TaxCodesUrls.TaxCodes,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxCode),
            }, cancellationToken);
    }

    /// <summary>Retrieve one tax code - <c>GET /admin/tax_codes/{tax_code_id}</c>.</summary>
    public Task<ApiResponse> GetTaxCodeAsync(
        object taxCodeId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxCodesUrls.TaxCode,
            new RequestOptions
            {
                PathParams = Id(taxCodeId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxCode),
            }, cancellationToken);

    /// <summary>HEAD one tax code - <c>HEAD /admin/tax_codes/{tax_code_id}</c>.</summary>
    public Task<ApiResponse> HeadTaxCodeAsync(
        object taxCodeId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TaxCodesUrls.TaxCode,
            new RequestOptions
            {
                PathParams = Id(taxCodeId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Update one tax code - <c>PUT /admin/tax_codes/{tax_code_id}</c>; resend priority and resolver_type.</summary>
    public Task<ApiResponse> UpdateTaxCodeAsync(
        object taxCodeId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(TaxCodesUrls.TaxCode,
            new RequestOptions
            {
                PathParams = Id(taxCodeId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxCode),
            }, cancellationToken);
    }

    /// <summary>Delete one tax code - <c>DELETE /admin/tax_codes/{tax_code_id}</c>.</summary>
    public Task<ApiResponse> DeleteTaxCodeAsync(
        object taxCodeId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(TaxCodesUrls.TaxCode,
            new RequestOptions
            {
                PathParams = Id(taxCodeId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object taxCodeId) =>
        new() { ["tax_code_id"] = taxCodeId };
}
