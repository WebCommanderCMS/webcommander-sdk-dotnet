namespace WebCommander.Resources.TaxCodes;

/// <summary>Path templates for the tax code endpoints.</summary>
public static class TaxCodesUrls
{
    public const string TaxCodes = "/api/{api_version}/admin/tax_codes";

    public const string TaxCode = "/api/{api_version}/admin/tax_codes/{tax_code_id}";
}
