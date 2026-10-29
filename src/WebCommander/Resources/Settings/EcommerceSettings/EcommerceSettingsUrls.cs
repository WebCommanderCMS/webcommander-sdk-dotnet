namespace WebCommander.Resources.Settings.EcommerceSettings;

/// <summary>Path templates for the ecommerce settings endpoints.</summary>
public static class EcommerceSettingsUrls
{
    public const string EcommerceSettings =
        "/api/{api_version}/admin/settings/ecommerce_settings";

    public const string EcommerceSettingsSection =
        "/api/{api_version}/admin/settings/ecommerce_settings/{section}";
}
