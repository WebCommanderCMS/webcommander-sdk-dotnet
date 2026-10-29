namespace WebCommander.Resources.Products;

/// <summary>Path templates for the product endpoints.</summary>
public static class ProductsUrls
{
    public const string Products = "/api/{api_version}/admin/products";

    public const string Product = "/api/{api_version}/admin/products/{product_id}";

    public const string ProductReviews = "/api/{api_version}/admin/products/{product_id}/reviews";

    public const string ProductStock = "/api/{api_version}/admin/products/{product_id}/stock";

    public const string ProductStockLocation =
        "/api/{api_version}/admin/products/{product_id}/stock/locations/{inventory_location_id}";

    /// <summary>Tags a write with a transaction number; it wins over a body <c>transaction_no</c> it disagrees with.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
