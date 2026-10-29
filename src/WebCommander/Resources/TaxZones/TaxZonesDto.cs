namespace WebCommander.Resources.TaxZones.TaxZonesDto;

/// <summary>The <c>pagination</c> block of the tax zone listing.</summary>
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

/// <summary>A country as this family nests it, on a country tax and in a zone's <c>countries</c>.</summary>
public sealed class TaxCountry : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }
}

/// <summary>A state as a tax zone nests it in <c>states</c>.</summary>
public sealed class TaxState : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }
}

/// <summary>One tax zone, as the listing and the row path both send it; <c>post_code</c> reads as the flat list
/// of strings a write sends.</summary>
public sealed class TaxZone : Dto
{
    protected override string? EnvelopeKey => "tax_zone";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public bool? SystemGenerated { get; set; }

    public bool? Default { get; set; }

    public bool? UseRadius { get; set; }

    public double? Radius { get; set; }

    public string? StartLocation { get; set; }

    public IReadOnlyList<TaxCountry?>? Countries { get; set; }

    public IReadOnlyList<TaxState?>? States { get; set; }

    public IReadOnlyList<string?>? PostCode { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The writable form of <see cref="TaxZone"/>, shared by the create and the update.</summary>
public sealed class TaxZoneInput : Dto
{
    protected override string? EnvelopeKey => "tax_zone";

    public string? Name { get; set; }

    public IReadOnlyList<TaxCountry?>? Countries { get; set; }

    public IReadOnlyList<TaxState?>? States { get; set; }

    public IReadOnlyList<string?>? PostCode { get; set; }

    public double? Radius { get; set; }

    public string? StartLocation { get; set; }

    public bool? UseRadius { get; set; }
}

/// <summary>One page of the tax zone listing.</summary>
public sealed class TaxZonePage : Dto
{
    public IReadOnlyList<TaxZone?>? TaxZones { get; set; }

    public Pagination? Pagination { get; set; }
}
