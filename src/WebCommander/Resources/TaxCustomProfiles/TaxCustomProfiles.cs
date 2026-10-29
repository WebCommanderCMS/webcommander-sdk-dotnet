using WebCommander.Client;
using WebCommander.Resources.TaxCustomProfiles.TaxCustomProfilesDto;

namespace WebCommander.Resources.TaxCustomProfiles;

/// <summary>Custom tax profiles - <c>/admin/tax/custom-profiles*</c> - as a typed resource bound to one client; no <c>HEAD</c>.</summary>
public sealed class TaxCustomProfiles : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.TaxCustomProfiles</c>.</summary>
    public TaxCustomProfiles(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the custom profiles - <c>GET /admin/tax/custom-profiles</c>, newest first. <c>limit</c> is 1 to 100
    /// with the cap echoed, <c>offset</c> or <c>page</c> move the window, <c>sort</c> takes <c>id</c> or <c>name</c>.</summary>
    public Task<ApiResponse> ListTaxCustomProfilesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxCustomProfilesUrls.TaxCustomProfiles,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomProfilePage),
            }, cancellationToken);

    /// <summary>Create a custom profile - <c>POST /admin/tax/custom-profiles</c>, answered <c>201</c>. Wrapped in
    /// <c>custom_profile</c>; <c>name</c> is required.
    /// <paramref name="transactionNo"/> is sent as <c>X-Transaction-No</c> and files the profile under that number.</summary>
    public Task<ApiResponse> CreateTaxCustomProfileAsync(
        object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(TaxCustomProfilesUrls.TaxCustomProfiles,
            new RequestOptions
            {
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [TaxCustomProfilesUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomProfile),
            }, cancellationToken);
    }

    /// <summary>Retrieve one custom profile, with an <c>ETag</c> - <c>GET /admin/tax/custom-profiles/{custom_profile_id}</c>;
    /// only this form carries <c>rules</c>.</summary>
    public Task<ApiResponse> GetTaxCustomProfileAsync(
        object customProfileId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxCustomProfilesUrls.TaxCustomProfile,
            new RequestOptions
            {
                PathParams = Id(customProfileId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomProfile),
            }, cancellationToken);

    /// <summary>Replace one custom profile - <c>PUT /admin/tax/custom-profiles/{custom_profile_id}</c>. An omitted
    /// <c>name</c> is kept; an omitted <c>description</c> or <c>tax_code</c> comes back <c>null</c>.</summary>
    public Task<ApiResponse> UpdateTaxCustomProfileAsync(
        object customProfileId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(TaxCustomProfilesUrls.TaxCustomProfile,
            new RequestOptions
            {
                PathParams = Id(customProfileId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomProfile),
            }, cancellationToken);
    }

    /// <summary>Delete one custom profile - <c>DELETE /admin/tax/custom-profiles/{custom_profile_id}</c>; <c>204</c>.</summary>
    public Task<ApiResponse> DeleteTaxCustomProfileAsync(
        object customProfileId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(TaxCustomProfilesUrls.TaxCustomProfile,
            new RequestOptions
            {
                PathParams = Id(customProfileId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object customProfileId) =>
        new() { ["custom_profile_id"] = customProfileId };
}
