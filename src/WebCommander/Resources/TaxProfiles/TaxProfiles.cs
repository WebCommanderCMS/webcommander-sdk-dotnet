using WebCommander.Client;
using WebCommander.Resources.TaxProfiles.TaxProfilesDto;

namespace WebCommander.Resources.TaxProfiles;

/// <summary>Tax profiles - <c>/admin/tax_profiles*</c> - as a typed resource bound to one client.</summary>
public sealed class TaxProfiles : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.TaxProfiles</c>.</summary>
    public TaxProfiles(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the tax profiles - <c>GET /admin/tax_profiles</c>.</summary>
    public Task<ApiResponse> ListTaxProfilesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxProfilesUrls.TaxProfiles,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxProfilePage),
            }, cancellationToken);

    /// <summary>HEAD the tax profile collection - <c>HEAD /admin/tax_profiles</c>.</summary>
    public Task<ApiResponse> HeadTaxProfilesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TaxProfilesUrls.TaxProfiles,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a tax profile - <c>POST /admin/tax_profiles</c>; send no <c>id</c>.</summary>
    public Task<ApiResponse> CreateTaxProfileAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(TaxProfilesUrls.TaxProfiles,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxProfile),
            }, cancellationToken);
    }

    /// <summary>Retrieve one tax profile - <c>GET /admin/tax_profiles/{tax_profile_id}</c>.</summary>
    public Task<ApiResponse> GetTaxProfileAsync(
        object taxProfileId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxProfilesUrls.TaxProfile,
            new RequestOptions
            {
                PathParams = Id(taxProfileId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxProfile),
            }, cancellationToken);

    /// <summary>HEAD one tax profile - <c>HEAD /admin/tax_profiles/{tax_profile_id}</c>.</summary>
    public Task<ApiResponse> HeadTaxProfileAsync(
        object taxProfileId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TaxProfilesUrls.TaxProfile,
            new RequestOptions
            {
                PathParams = Id(taxProfileId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Update one tax profile - <c>PUT /admin/tax_profiles/{tax_profile_id}</c>, a merge.</summary>
    public Task<ApiResponse> UpdateTaxProfileAsync(
        object taxProfileId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(TaxProfilesUrls.TaxProfile,
            new RequestOptions
            {
                PathParams = Id(taxProfileId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxProfile),
            }, cancellationToken);
    }

    /// <summary>Delete one tax profile - <c>DELETE /admin/tax_profiles/{tax_profile_id}</c>.</summary>
    public Task<ApiResponse> DeleteTaxProfileAsync(
        object taxProfileId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(TaxProfilesUrls.TaxProfile,
            new RequestOptions
            {
                PathParams = Id(taxProfileId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object taxProfileId) =>
        new() { ["tax_profile_id"] = taxProfileId };
}
