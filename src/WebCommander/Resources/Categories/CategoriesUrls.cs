namespace WebCommander.Resources.Categories;

/// <summary>Path templates and the transaction number header for the category endpoints.</summary>
public static class CategoriesUrls
{
    public const string Categories = "/api/{api_version}/admin/categories";

    public const string CategoriesCount = "/api/{api_version}/admin/categories/count";

    public const string CategoriesCheckName = "/api/{api_version}/admin/categories/check-name";

    public const string Category = "/api/{api_version}/admin/categories/{category_id}";

    public const string CategorySection =
        "/api/{api_version}/admin/categories/{category_id}/{section}";

    /// <summary>Files a new category under this transaction number; a different <c>transaction_no</c> inside
    /// <c>category</c> is refused with <c>400 header_body_mismatch</c>, and one beside <c>category</c> is
    /// ignored.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
