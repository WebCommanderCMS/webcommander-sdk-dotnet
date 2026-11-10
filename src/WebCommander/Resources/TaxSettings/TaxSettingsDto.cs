namespace WebCommander.Resources.TaxSettings.TaxSettingsDto;

/// <summary>The country tax the store charges by default, as <c>tax_region</c> reads it.</summary>
public sealed class TaxRegion : Dto
{
    public long? CountryTaxId { get; set; }

    public string? CountryCode { get; set; }

    public string? CountryName { get; set; }
}

/// <summary>A custom tax profile as a read names it: its id and name, or <c>null</c>.</summary>
public sealed class TaxProfileReference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>One <c>group_meta</c> entry: what the area is, whether it can be written, and its path.</summary>
public sealed class TaxSettingsAreaMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block: the settings area and the two tax collections beside it.</summary>
public sealed class TaxSettingsGroupMeta : Dto
{
    public TaxSettingsAreaMeta? TaxSettings { get; set; }

    public TaxSettingsAreaMeta? TaxProfiles { get; set; }

    public TaxSettingsAreaMeta? TaxRules { get; set; }
}

/// <summary>The nine keys under <c>tax_settings</c>, with <c>group_meta</c> and, when asked for, <c>field_metadata</c>.</summary>
public sealed class TaxSettings : Dto
{
    protected override string? EnvelopeKey => "tax_settings";

    public string? TaxRateForCustomers { get; set; }

    public TaxRegion? TaxRegion { get; set; }

    public TaxProfileReference? CustomTaxProfile { get; set; }

    public bool? IsPriceShownWithTax { get; set; }

    public string? PricesInCatalog { get; set; }

    public string? CalculatedBy { get; set; }

    public bool? IsOutOfRegionTaxFree { get; set; }

    public string? TaxMessage { get; set; }

    public string? BasePriceRounding { get; set; }

    public TaxSettingsGroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary><c>tax_region</c> as a write takes it: a <c>country_code</c> (with an optional <c>state_code</c>) or a <c>country_tax_id</c>.</summary>
public sealed class TaxRegionInput : Dto
{
    public long? CountryTaxId { get; set; }

    public string? CountryCode { get; set; }

    public string? StateCode { get; set; }
}

/// <summary>The body <c>PUT</c> and <c>PATCH</c> take, wrapped in <c>tax_settings</c>; both merge.
/// The profile is written as <c>custom_tax_profile_id</c>; <c>calculated_by</c> is read-only.</summary>
public sealed class TaxSettingsInput : Dto
{
    protected override string? EnvelopeKey => "tax_settings";

    public string? TaxRateForCustomers { get; set; }

    public TaxRegionInput? TaxRegion { get; set; }

    public long? CustomTaxProfileId { get; set; }

    public bool? IsPriceShownWithTax { get; set; }

    public string? PricesInCatalog { get; set; }

    public bool? IsOutOfRegionTaxFree { get; set; }

    public string? TaxMessage { get; set; }

    public string? BasePriceRounding { get; set; }
}
