namespace WebCommander.Resources.TaxRules.TaxRulesDto;

/// <summary>The <c>pagination</c> block of the tax rule listing.</summary>
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

/// <summary>The tax code a rule points at, as the rule nests it; no rule write sets it.</summary>
public sealed class TaxRuleCode : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public bool? IsDefault { get; set; }
}

/// <summary>One tax zone a rule points at, as the rule nests it; no rule write sets it.</summary>
public sealed class TaxRuleZone : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public bool? IsSystemGenerated { get; set; }

    public bool? IsDefault { get; set; }
}

/// <summary>One tax rule; <see cref="Code"/>, <see cref="Zones"/> and the rounding fields are read-only.</summary>
public sealed class TaxRule : Dto
{
    protected override string? EnvelopeKey => "tax_rule";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public TaxRuleCode? Code { get; set; }

    public string? Description { get; set; }

    public bool? Default { get; set; }

    public string? RoundingType { get; set; }

    public long? DecimalPoint { get; set; }

    public IReadOnlyList<TaxRuleZone?>? Zones { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The writable form of <see cref="TaxRule"/>: two fields of the row's ten.</summary>
public sealed class TaxRuleInput : Dto
{
    protected override string? EnvelopeKey => "tax_rule";

    public string? Name { get; set; }

    public string? Description { get; set; }
}

/// <summary>One page of the tax rule listing.</summary>
public sealed class TaxRulePage : Dto
{
    public IReadOnlyList<TaxRule?>? TaxRules { get; set; }

    public Pagination? Pagination { get; set; }
}
