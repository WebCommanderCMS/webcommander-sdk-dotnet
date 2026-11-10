namespace WebCommander.Resources.ShippingProfiles;

/// <summary>Path templates and the transaction number header for the shipping profiles endpoints.</summary>
public static class ShippingProfilesUrls
{
    public const string ShippingProfiles = "/api/{api_version}/admin/shipping_profiles";

    public const string ShippingProfilesCount = "/api/{api_version}/admin/shipping_profiles/count";

    public const string ShippingProfilesWizard = "/api/{api_version}/admin/shipping_profiles/wizard";

    public const string ShippingProfile = "/api/{api_version}/admin/shipping_profiles/{shipping_profile_id}";

    /// <summary>Files the record under a transaction number; it wins over a <c>transaction_no</c> in the body.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
