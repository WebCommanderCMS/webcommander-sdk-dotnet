namespace WebCommander.Resources.Variations;

/// <summary>Path templates for the variation type and product variant endpoints.</summary>
public static class VariationsUrls
{
    public const string Variations = "/api/{api_version}/admin/variations";

    public const string Variation = "/api/{api_version}/admin/variations/{variation_id}";

    public const string VariationOption =
        "/api/{api_version}/admin/variations/{variation_id}/options/{variation_option_id}";

    public const string ProductVariations =
        "/api/{api_version}/admin/products/{product_id}/variations";

    public const string ProductVariationsBulk =
        "/api/{api_version}/admin/products/{product_id}/variations/bulk";

    /// <summary>Tags a write with a transaction number; it wins over a body <c>transaction_no</c> it disagrees with.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
