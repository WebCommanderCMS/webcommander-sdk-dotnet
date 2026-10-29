namespace WebCommander.Resources.TaxSettings.TaxSettingsDto;

/// <summary>The nine flat keys the tax settings facade sends.</summary>
public sealed class TaxSettings : Dto
{
    protected override string? EnvelopeKey => "tax_settings";

    public string? ConfigurationType { get; set; }

    public bool? SystemDefaultTax { get; set; }

    public string? DefaultCountry { get; set; }

    public long? CustomTaxProfileId { get; set; }

    public long? DefaultTaxProfile { get; set; }

    public bool? ShowPriceWithTax { get; set; }

    public bool? IsPriceWithTax { get; set; }

    public string? DefaultTaxCode { get; set; }

    public string? TaxMessage { get; set; }
}

/// <summary>The body the settings <c>PUT</c> takes, wrapped in <c>settings</c>; a merge.</summary>
public sealed class TaxSettingsInput : Dto
{
    protected override string? EnvelopeKey => "settings";

    public string? ConfigurationType { get; set; }

    public bool? SystemDefaultTax { get; set; }

    public string? DefaultCountry { get; set; }

    public long? CustomTaxProfileId { get; set; }

    public long? DefaultTaxProfile { get; set; }

    public bool? ShowPriceWithTax { get; set; }

    public bool? IsPriceWithTax { get; set; }

    public string? DefaultTaxCode { get; set; }

    public string? TaxMessage { get; set; }
}
