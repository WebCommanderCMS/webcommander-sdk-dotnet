namespace WebCommander.Resources.ShippingRates;

/// <summary>Path templates and the transaction number header for the shipping rates endpoints.</summary>
public static class ShippingRatesUrls
{
    public const string ShippingRates = "/api/{api_version}/admin/shipping_rates";

    public const string ShippingRatesCount = "/api/{api_version}/admin/shipping_rates/count";

    public const string ShippingRate = "/api/{api_version}/admin/shipping_rates/{shipping_rate_id}";

    /// <summary>Files the record under a transaction number; it wins over a <c>transaction_no</c> in the body.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
