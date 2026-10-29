namespace WebCommander.Resources.Settings.SeoConfiguration;

/// <summary>Path templates for the SEO configuration settings endpoints.</summary>
public static class SeoConfigurationUrls
{
    public const string SeoConfiguration = "/api/{api_version}/admin/settings/seo_configuration";

    public const string SeoConfigurationSection = "/api/{api_version}/admin/settings/seo_configuration/{section}";

    public const string SeoConfigurationDocumentRoot =
        "/api/{api_version}/admin/settings/seo_configuration/document_root";

    public const string SeoConfigurationDocumentRootFile =
        "/api/{api_version}/admin/settings/seo_configuration/document_root/{config_key}";

    public const string SeoConfigurationRedirects =
        "/api/{api_version}/admin/settings/seo_configuration/redirects";

    public const string SeoConfigurationRedirect =
        "/api/{api_version}/admin/settings/seo_configuration/redirects/{redirect_id}";
}
