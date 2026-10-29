namespace WebCommander.Resources.Settings.PriceDisplaySettings;

/// <summary>Path templates for the price display settings endpoints.</summary>
public static class PriceDisplaySettingsUrls
{
    public const string PriceDisplaySettings =
        "/api/{api_version}/admin/settings/price_display_settings";

    public const string PriceDisplaySettingsSection =
        "/api/{api_version}/admin/settings/price_display_settings/{section}";
}
