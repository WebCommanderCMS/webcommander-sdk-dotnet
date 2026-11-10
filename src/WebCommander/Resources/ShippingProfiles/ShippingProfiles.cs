using WebCommander.Client;
using WebCommander.Resources.ShippingProfiles.ShippingProfilesDto;

namespace WebCommander.Resources.ShippingProfiles;

/// <summary>Shipping profiles - <c>/admin/shipping_profiles*</c> - as a typed resource bound to one client.</summary>
public sealed class ShippingProfiles : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.ShippingProfiles</c>.</summary>
    public ShippingProfiles(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the profiles, newest first - <c>GET /admin/shipping_profiles</c>. <c>search</c> (part of the name) and
    /// <c>default</c> (<c>true</c> or <c>false</c>) filter; <c>sort</c> is <c>id</c>, <c>name</c>, <c>description</c>,
    /// <c>rule_preference</c>, <c>created_at</c>, <c>updated_at</c> or <c>transaction_no</c> with <c>order</c> <c>asc</c>
    /// or <c>desc</c>; <c>limit</c> is 1 to 100 and defaults to 20, <c>offset</c> pages. An unknown value is <c>400</c>;
    /// an unknown name is ignored.</summary>
    public Task<ApiResponse> ListShippingProfilesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingProfilesUrls.ShippingProfiles,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingProfileListing),
            }, cancellationToken);

    /// <summary>HEAD the collection - <c>HEAD /admin/shipping_profiles</c>; no body.</summary>
    public Task<ApiResponse> HeadShippingProfilesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingProfilesUrls.ShippingProfiles,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Count the profiles - <c>GET /admin/shipping_profiles/count</c>, a bare <c>{"total": N}</c>. <c>search</c> and
    /// <c>default</c> narrow it.</summary>
    public Task<ApiResponse> CountShippingProfilesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingProfilesUrls.ShippingProfilesCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingProfileCount),
            }, cancellationToken);

    /// <summary>HEAD the count - <c>HEAD /admin/shipping_profiles/count</c>; no body.</summary>
    public Task<ApiResponse> HeadShippingProfilesCountAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingProfilesUrls.ShippingProfilesCount,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a profile - <c>POST /admin/shipping_profiles</c>, answered <c>201</c> with a <c>Location</c>. The body is
    /// wrapped in <c>shipping_profile</c> and needs <c>name</c>, unique without regard to case (<c>409</c>);
    /// <c>rule_preference</c> defaults to <c>product_assigned_rule</c>. An unknown field, an <c>id</c> or an unwrapped
    /// body is <c>400</c>. <paramref name="transactionNo"/> is sent as <c>X-Transaction-No</c> and wins over a
    /// <c>transaction_no</c> in the body.</summary>
    public Task<ApiResponse> CreateShippingProfileAsync(
        object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ShippingProfilesUrls.ShippingProfiles,
            new RequestOptions
            {
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [ShippingProfilesUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingProfile),
            }, cancellationToken);
    }

    /// <summary>Create a profile with its rules and their rates in one call -
    /// <c>POST /admin/shipping_profiles/wizard</c>. A new profile name is <c>201</c>; a name that exists adds the rules
    /// to that profile and is <c>200</c>. Every rule's rate, class and zone is checked first, and one bad value saves
    /// nothing. A zone given as <c>{"name": ...}</c> is created.</summary>
    public Task<ApiResponse> CreateShippingProfileWizardAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ShippingProfilesUrls.ShippingProfilesWizard,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingProfile),
            }, cancellationToken);
    }

    /// <summary>Retrieve one profile by its <c>id</c>, the row under <c>shipping_profile</c> -
    /// <c>GET /admin/shipping_profiles/{shipping_profile_id}</c>. An id that is not plain digits, or unknown, is <c>404</c>.</summary>
    public Task<ApiResponse> GetShippingProfileAsync(
        object shippingProfileId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingProfilesUrls.ShippingProfile,
            new RequestOptions
            {
                PathParams = Id(shippingProfileId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingProfile),
            }, cancellationToken);

    /// <summary>Ask whether a profile exists - <c>HEAD /admin/shipping_profiles/{shipping_profile_id}</c>; <c>200</c> with no
    /// body, <c>404</c> for an unknown id.</summary>
    public Task<ApiResponse> HeadShippingProfileAsync(
        object shippingProfileId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingProfilesUrls.ShippingProfile,
            new RequestOptions
            {
                PathParams = Id(shippingProfileId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Replace one profile - <c>PUT /admin/shipping_profiles/{shipping_profile_id}</c>. <c>name</c>, <c>description</c>,
    /// <c>rule_preference</c> and <c>shipping_rules</c> are all required (<c>description</c> may be <c>null</c>); the rules
    /// not listed are taken off the profile and kept.</summary>
    public Task<ApiResponse> UpdateShippingProfileAsync(
        object shippingProfileId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(ShippingProfilesUrls.ShippingProfile,
            new RequestOptions
            {
                PathParams = Id(shippingProfileId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingProfile),
            }, cancellationToken);
    }

    /// <summary>Change only the fields sent - <c>PATCH /admin/shipping_profiles/{shipping_profile_id}</c>. <c>shipping_rules</c>
    /// is the full list, so <c>[]</c> takes every rule off. <c>default</c> true moves the store default to this profile;
    /// the default cannot be switched off directly (<c>409</c>).</summary>
    public Task<ApiResponse> PatchShippingProfileAsync(
        object shippingProfileId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ShippingProfilesUrls.ShippingProfile,
            new RequestOptions
            {
                PathParams = Id(shippingProfileId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingProfile),
            }, cancellationToken);
    }

    /// <summary>Delete one profile - <c>DELETE /admin/shipping_profiles/{shipping_profile_id}</c>, <c>204</c>. Its rules and
    /// zones stay. The store default is <c>409</c> until another profile holds the flag.</summary>
    public Task<ApiResponse> DeleteShippingProfileAsync(
        object shippingProfileId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(ShippingProfilesUrls.ShippingProfile,
            new RequestOptions
            {
                PathParams = Id(shippingProfileId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object shippingProfileId) =>
        new() { ["shipping_profile_id"] = shippingProfileId };
}
