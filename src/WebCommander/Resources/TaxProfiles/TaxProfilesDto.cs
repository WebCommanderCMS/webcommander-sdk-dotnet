namespace WebCommander.Resources.TaxProfiles.TaxProfilesDto;

/// <summary>The <c>pagination</c> block of the tax profile listing.</summary>
public sealed class Pagination : Dto
{
    /// <summary>How many rows match across every page.</summary>
    public long? Total { get; set; }

    /// <summary>The page size in effect, clamped to 20.</summary>
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

/// <summary>The tax code a nested rule points at.</summary>
public sealed class TaxProfileRuleCode : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public bool? IsDefault { get; set; }
}

/// <summary>One tax zone a nested rule points at.</summary>
public sealed class TaxProfileRuleZone : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public bool? IsSystemGenerated { get; set; }

    public bool? IsDefault { get; set; }
}

/// <summary>One tax rule as a profile nests it, the same ten keys a <c>/admin/tax_rules</c> read sends.</summary>
public sealed class TaxProfileRule : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public TaxProfileRuleCode? Code { get; set; }

    public string? Description { get; set; }

    public bool? Default { get; set; }

    public string? RoundingType { get; set; }

    public long? DecimalPoint { get; set; }

    public IReadOnlyList<TaxProfileRuleZone?>? Zones { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>One tax profile, the same seven keys on the listing row and on the row path; a new profile's
/// <c>tax_rules</c> is empty. <see cref="Default"/> is exclusive and moves off the row that held it.</summary>
public sealed class TaxProfile : Dto
{
    protected override string? EnvelopeKey => "tax_profile";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Default { get; set; }

    public IReadOnlyList<TaxProfileRule?>? TaxRules { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The writable form of <see cref="TaxProfile"/>; <see cref="Default"/> is one-way.</summary>
public sealed class TaxProfileInput : Dto
{
    protected override string? EnvelopeKey => "tax_profile";

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Default { get; set; }
}

/// <summary>One page of the tax profile listing.</summary>
public sealed class TaxProfilePage : Dto
{
    public IReadOnlyList<TaxProfile?>? TaxProfiles { get; set; }

    public Pagination? Pagination { get; set; }
}
