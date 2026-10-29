namespace WebCommander.Resources.BlogCategories;

/// <summary>Path templates for the blog category endpoints.</summary>
public static class BlogCategoriesUrls
{
    public const string BlogCategories = "/api/{api_version}/admin/blog_categories";

    public const string BlogCategory =
        "/api/{api_version}/admin/blog_categories/{blog_category_id}";
}
