namespace WebCommander.Resources.ShippingProfiles;

/// <summary>Path templates and the transaction number header for the shipping profiles endpoints.</summary>
public static class ShippingProfilesUrls
{
    public const string ShippingProfiles = "/api/{api_version}/admin/shipping_profiles";

    public const string ShippingProfilesCount = "/api/{api_version}/admin/shipping_profiles/count";

    public const string ShippingProfilesWizard = "/api/{api_version}/admin/shipping_profiles/wizard";

    public const string ShippingProfile = "/api/{api_version}/admin/shipping_profiles/{shipping_profile_id}";

    /// <summary>Files the record under a transaction number; a different <c>transaction_no</c> beside the
    /// <c>shipping_profile</c> wrapper is refused with <c>400 header_body_mismatch</c>, and one inside the wrapper is
    /// ignored.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
