namespace WebCommander.Resources.Settings.SiteSettings;

/// <summary>Path templates for the site settings endpoints.</summary>
public static class SiteSettingsUrls
{
    public const string SiteSettings =
        "/api/{api_version}/admin/settings/site_settings";

    public const string SiteSettingsSection =
        "/api/{api_version}/admin/settings/site_settings/{section}";
}
