using WebCommander.Client;
using WebCommander.Resources.DiscountProfiles.DiscountProfilesDto;

namespace WebCommander.Resources.DiscountProfiles;

/// <summary>Discount profiles and their coupons - <c>/admin/discount_profiles*</c> and <c>/admin/discount_coupons/assign-customer</c> - as a typed resource bound to one client.</summary>
public sealed class DiscountProfiles : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.DiscountProfiles</c>.</summary>
    public DiscountProfiles(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the profiles, newest <c>id</c> first - <c>GET /admin/discount_profiles</c>; <c>limit</c> is capped at 20 and an unknown name is ignored.</summary>
    public Task<ApiResponse> ListDiscountProfilesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DiscountProfilesUrls.DiscountProfiles,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DiscountProfilePage),
            }, cancellationToken);

    /// <summary>Create a profile - <c>POST /admin/discount_profiles</c>, answered <c>201</c>, with a bare-record body naming <c>name</c>, <c>discount_details_type</c> and the block it names.</summary>
    public Task<ApiResponse> CreateDiscountProfileAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(DiscountProfilesUrls.DiscountProfiles,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DiscountProfile),
            }, cancellationToken);
    }

    /// <summary>Retrieve one profile - <c>GET /admin/discount_profiles/{discount_profile_id}</c>; <c>field_metadata</c> adds the schema beside the row.</summary>
    public Task<ApiResponse> GetDiscountProfileAsync(
        object discountProfileId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DiscountProfilesUrls.DiscountProfile,
            new RequestOptions
            {
                PathParams = Id(discountProfileId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DiscountProfile),
            }, cancellationToken);

    /// <summary>Replace one profile - <c>PUT /admin/discount_profiles/{discount_profile_id}</c>; an omitted flag, date or limit is reset.</summary>
    public Task<ApiResponse> UpdateDiscountProfileAsync(
        object discountProfileId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(DiscountProfilesUrls.DiscountProfile,
            new RequestOptions
            {
                PathParams = Id(discountProfileId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DiscountProfile),
            }, cancellationToken);
    }

    /// <summary>Delete one profile and its coupons - <c>DELETE /admin/discount_profiles/{discount_profile_id}</c>, answered <c>204</c>.</summary>
    public Task<ApiResponse> DeleteDiscountProfileAsync(
        object discountProfileId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(DiscountProfilesUrls.DiscountProfile,
            new RequestOptions
            {
                PathParams = Id(discountProfileId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Generate codes on a profile that has <c>is_apply_coupon_code</c> - <c>POST /admin/discount_profiles/{discount_profile_id}/create-coupons</c>, a bare <c>{"requested_codes": n}</c>.</summary>
    public Task<ApiResponse> CreateDiscountProfileCouponsAsync(
        object discountProfileId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(DiscountProfilesUrls.DiscountProfileCreateCoupons,
            new RequestOptions
            {
                PathParams = Id(discountProfileId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DiscountProfileCoupons),
            }, cancellationToken);
    }

    /// <summary>Assign a generated code to an existing customer - <c>POST /admin/discount_coupons/assign-customer</c>, a bare <c>{"code", "email"}</c>; the answer is a verdict.</summary>
    public Task<ApiResponse> AssignDiscountCouponCustomerAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(DiscountProfilesUrls.DiscountCouponsAssignCustomer,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Id(object discountProfileId) =>
        new() { ["discount_profile_id"] = discountProfileId };
}
