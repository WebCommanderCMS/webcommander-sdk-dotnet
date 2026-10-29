namespace WebCommander.Resources.TaxCountryTaxes.TaxCountryTaxesDto;

/// <summary>The <c>pagination</c> block of the country tax listing.</summary>
public sealed class Pagination : Dto
{
    /// <summary>How many rows match across every page.</summary>
    public long? Total { get; set; }

    /// <summary>The page size in effect, clamped to 100.</summary>
    public long? Limit { get; set; }

    public long? Offset { get; set; }

    /// <summary>The rows on this page; <see cref="Total"/> is the rows matching.</summary>
    public long? Count { get; set; }

    public long? CurrentPage { get; set; }

    public long? TotalPages { get; set; }

    public bool? HasNext { get; set; }

    public bool? HasPrevious { get; set; }

    public string? PreviousPage { get; set; }

    public string? NextPage { get; set; }
}

/// <summary>A country as this family nests it, on a country tax's <c>country</c>; <c>null</c> on the rest-of-the-world row.</summary>
public sealed class TaxCountry : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }
}

/// <summary>A state as a state rule nests it, on its <c>state</c>.</summary>
public sealed class TaxState : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }
}

/// <summary>One entry of a country tax's <c>state_rules</c>, sent only by the row path and the update.</summary>
public sealed class CountryTaxStateRule : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public TaxState? State { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? PostCode { get; set; }

    public double? TaxRate { get; set; }

    public string? TaxRule { get; set; }

    public double? EffectiveTaxRate { get; set; }

    public bool? IsDefault { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedOn { get; set; }

    public object? LastUpdatedBy { get; set; }
}

/// <summary>One country tax; the row path and the update add <see cref="StateRules"/> to the twelve keys a
/// listing row and the create answer carry. <see cref="TaxMethod"/> reads lower case.</summary>
public sealed class CountryTax : Dto
{
    protected override string? EnvelopeKey => "country_tax";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public TaxCountry? Country { get; set; }

    public string? TaxCode { get; set; }

    public double? TaxRate { get; set; }

    public string? TaxMethod { get; set; }

    public bool? IsDefault { get; set; }

    public bool? IsRestOfWorld { get; set; }

    public long? StateRulesCount { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedOn { get; set; }

    /// <summary>Only ever seen <c>null</c>.</summary>
    public object? LastUpdatedBy { get; set; }

    public IReadOnlyList<CountryTaxStateRule?>? StateRules { get; set; }
}

/// <summary>The writable form of <see cref="CountryTax"/>; <see cref="CountryCode"/> is the create's one requirement.</summary>
public sealed class CountryTaxInput : Dto
{
    protected override string? EnvelopeKey => "country_tax";

    protected override IReadOnlyList<string> TopLevel { get; } = new[] { "transaction_no" };

    public string? CountryCode { get; set; }

    public string? Name { get; set; }

    public string? TaxCode { get; set; }

    public double? TaxRate { get; set; }

    public string? TaxMethod { get; set; }

    public bool? IsDefault { get; set; }

    /// <summary>Beside the envelope, where the create files the row and the update re-files it; inside it nothing is filed.</summary>
    public string? TransactionNo { get; set; }
}

/// <summary>One <c>group_meta</c> entry: what the path is, whether it is writable and where.</summary>
public sealed class ResourceMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block only the listing sends.</summary>
public sealed class GroupMeta : Dto
{
    public ResourceMeta? CountryTax { get; set; }

    public ResourceMeta? StateRules { get; set; }
}

/// <summary>One page of the country tax listing: the rows, <c>group_meta</c> and the paging block.</summary>
public sealed class CountryTaxPage : Dto
{
    public IReadOnlyList<CountryTax?>? CountryTaxes { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public Pagination? Pagination { get; set; }
}
