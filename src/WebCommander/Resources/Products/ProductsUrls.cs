namespace WebCommander.Resources.Products;

/// <summary>Path templates for the product endpoints.</summary>
public static class ProductsUrls
{
    public const string Products = "/api/{api_version}/admin/products";

    public const string ProductsCount = "/api/{api_version}/admin/products/count";

    public const string ProductsCheckSku = "/api/{api_version}/admin/products/check-sku";

    public const string ProductsSpecial = "/api/{api_version}/admin/products/special";

    public const string ProductsBulkDelete = "/api/{api_version}/admin/products/bulk-delete";

    public const string Product = "/api/{api_version}/admin/products/{product_id}";

    public const string ProductReviews = "/api/{api_version}/admin/products/{product_id}/reviews";

    public const string ProductReview = "/api/{api_version}/admin/products/{product_id}/reviews/{review_id}";

    public const string ProductImage = "/api/{api_version}/admin/products/{product_id}/image";

    public const string ProductMedia = "/api/{api_version}/admin/products/{product_id}/media";

    public const string ProductMediaImage =
        "/api/{api_version}/admin/products/{product_id}/media/images/{image_id}";

    public const string ProductInventories = "/api/{api_version}/admin/products/{product_id}/inventories";

    public const string ProductStock = "/api/{api_version}/admin/products/{product_id}/stock";

    public const string ProductStockAdjustments =
        "/api/{api_version}/admin/products/{product_id}/stock/adjustments";

    public const string ProductSeo = "/api/{api_version}/admin/products/{product_id}/seo";

    public const string ProductMediaOrder = "/api/{api_version}/admin/products/{product_id}/media/order";

    public const string ProductStockLocation =
        "/api/{api_version}/admin/products/{product_id}/stock/locations/{inventory_location_id}";

    /// <summary>Tags a write with a transaction number; it wins over a body <c>transaction_no</c> it disagrees with.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
