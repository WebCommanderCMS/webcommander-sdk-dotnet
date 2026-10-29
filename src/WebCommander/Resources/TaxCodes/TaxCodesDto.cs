namespace WebCommander.Resources.TaxCodes.TaxCodesDto;

/// <summary>The <c>pagination</c> block of the tax code listing.</summary>
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

/// <summary>One tax code; <see cref="Default"/> is read-only and not exclusive.</summary>
public sealed class TaxCode : Dto
{
    protected override string? EnvelopeKey => "tax_code";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Label { get; set; }

    public string? Description { get; set; }

    public double? Rate { get; set; }

    public string? ResolverType { get; set; }

    public long? Priority { get; set; }

    public bool? Default { get; set; }
}

/// <summary>The writable form of <see cref="TaxCode"/>, shared by the create and the update.</summary>
public sealed class TaxCodeInput : Dto
{
    protected override string? EnvelopeKey => "tax_code";

    public string? Name { get; set; }

    public string? Label { get; set; }

    public string? Description { get; set; }

    public double? Rate { get; set; }

    public string? ResolverType { get; set; }

    public long? Priority { get; set; }
}

/// <summary>One page of the tax code listing.</summary>
public sealed class TaxCodePage : Dto
{
    public IReadOnlyList<TaxCode?>? TaxCodes { get; set; }

    public Pagination? Pagination { get; set; }
}
