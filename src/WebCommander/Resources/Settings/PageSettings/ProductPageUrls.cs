namespace WebCommander.Resources.Settings.PageSettings;

/// <summary>Path templates for the product page settings endpoints.</summary>
public static class ProductPageUrls
{
    public const string ProductPage =
        "/api/{api_version}/admin/settings/page_settings/product_page";

    public const string ProductPageSection =
        "/api/{api_version}/admin/settings/page_settings/product_page/{section}";
}
