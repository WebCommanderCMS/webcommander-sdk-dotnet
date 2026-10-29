namespace WebCommander.Resources.Currencies;

/// <summary>Path templates and the transaction number header for the currency endpoints.</summary>
public static class CurrenciesUrls
{
    public const string Currencies = "/api/{api_version}/admin/currencies";

    public const string CurrenciesCount = "/api/{api_version}/admin/currencies/count";

    public const string Currency = "/api/{api_version}/admin/currencies/{currency_id}";

    public const string CurrencyBase = "/api/{api_version}/admin/currencies/{currency_id}/base";

    /// <summary>Files a new currency under this transaction number, the only way: a body field is <c>400</c>.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
