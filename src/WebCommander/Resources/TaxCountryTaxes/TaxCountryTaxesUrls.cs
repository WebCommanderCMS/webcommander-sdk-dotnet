namespace WebCommander.Resources.TaxCountryTaxes;

/// <summary>Path templates and the transaction number header for the country tax endpoints.</summary>
public static class TaxCountryTaxesUrls
{
    /// <summary>Hyphenated: the underscore path <c>/admin/tax/country_taxes</c> is <c>404</c> <c>Unknown tax path</c>.</summary>
    public const string TaxCountryTaxes = "/api/{api_version}/admin/tax/country-taxes";

    public const string TaxCountryTax =
        "/api/{api_version}/admin/tax/country-taxes/{country_tax_id}";

    /// <summary>Files a new country tax under this transaction number; it wins over a different body number.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
