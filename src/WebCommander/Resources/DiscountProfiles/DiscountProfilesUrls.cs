namespace WebCommander.Resources.DiscountProfiles;

/// <summary>Path templates for the discount profile endpoints.</summary>
public static class DiscountProfilesUrls
{
    public const string DiscountProfiles = "/api/{api_version}/admin/discount_profiles";

    public const string DiscountProfile =
        "/api/{api_version}/admin/discount_profiles/{discount_profile_id}";

    public const string DiscountProfileCreateCoupons =
        "/api/{api_version}/admin/discount_profiles/{discount_profile_id}/create-coupons";

    public const string DiscountCouponsAssignCustomer =
        "/api/{api_version}/admin/discount_coupons/assign-customer";
}
