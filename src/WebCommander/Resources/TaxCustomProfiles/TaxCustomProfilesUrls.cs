namespace WebCommander.Resources.TaxCustomProfiles;

/// <summary>Path templates and the transaction number header for the custom tax profile endpoints.</summary>
public static class TaxCustomProfilesUrls
{
    /// <summary>Hyphenated: the underscore path <c>/admin/tax/custom_profiles</c> is <c>404</c> <c>Unknown tax path</c>.</summary>
    public const string TaxCustomProfiles = "/api/{api_version}/admin/tax/custom-profiles";

    public const string TaxCustomProfile =
        "/api/{api_version}/admin/tax/custom-profiles/{custom_profile_id}";

    /// <summary>Files a new custom profile under this transaction number; it wins over a different body number.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
