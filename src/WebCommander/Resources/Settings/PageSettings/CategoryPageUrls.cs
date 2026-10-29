namespace WebCommander.Resources.Settings.PageSettings;

/// <summary>Path templates for the category page settings endpoints.</summary>
public static class CategoryPageUrls
{
    public const string CategoryPage =
        "/api/{api_version}/admin/settings/page_settings/category_page";

    public const string CategoryPageSection =
        "/api/{api_version}/admin/settings/page_settings/category_page/{section}";
}
