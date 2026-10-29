namespace WebCommander.Resources.Countries.CountriesDto;

/// <summary>One country: a listing row, and the row path's body.</summary>
public sealed class Country : Dto
{
    protected override string? EnvelopeKey => "country";

    public long? Id { get; set; }

    /// <summary>Two upper-case letters, the ISO code.</summary>
    public string? Code { get; set; }

    public string? Name { get; set; }

    /// <summary>The <c>total</c> the country's state listing answers.</summary>
    public long? StateCount { get; set; }

    public bool? Active { get; set; }

    /// <summary>Held by one country, the administration settings' <c>default_country</c>.</summary>
    public bool? Default { get; set; }
}

/// <summary>The short parent country a state listing and a state read carry beside the state.</summary>
public sealed class CountryReference : Dto
{
    public long? Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }
}

/// <summary>One state: a listing row, and the state path's body with its parent <see cref="Country"/> beside it.</summary>
public sealed class CountryState : Dto
{
    protected override string? EnvelopeKey => "state";

    public long? Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? Active { get; set; }

    public bool? Default { get; set; }

    /// <summary><c>null</c> on a listing row; the state read sends it beside the envelope.</summary>
    public CountryReference? Country { get; set; }
}

/// <summary>The five-key <c>pagination</c> block of both listings.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    /// <summary>The page size applied: the <c>limit</c> sent, 20, or under <c>limit=all</c> the rows that arrived.</summary>
    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the country listing: the rows and the block beside them, with no envelope.</summary>
public sealed class CountryPage : Dto
{
    public IReadOnlyList<Country?>? Countries { get; set; }

    public Pagination? Pagination { get; set; }
}

/// <summary>One page of a country's states, beside the short parent country.</summary>
public sealed class CountryStatePage : Dto
{
    public CountryReference? Country { get; set; }

    public IReadOnlyList<CountryState?>? States { get; set; }

    public Pagination? Pagination { get; set; }
}
