namespace WebCommander.Resources.ShippingRules;

/// <summary>Path templates and the transaction number header for the shipping rules endpoints.</summary>
public static class ShippingRulesUrls
{
    public const string ShippingRules = "/api/{api_version}/admin/shipping_rules";

    public const string ShippingRulesCount = "/api/{api_version}/admin/shipping_rules/count";

    public const string ShippingRule = "/api/{api_version}/admin/shipping_rules/{shipping_rule_id}";

    /// <summary>Files the record under a transaction number; it wins over a <c>transaction_no</c> in the body.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
