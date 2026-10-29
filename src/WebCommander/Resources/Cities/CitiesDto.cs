namespace WebCommander.Resources.Cities.CitiesDto;

/// <summary>The state a city row carries. <see cref="Id"/> is the one a country's <c>states</c> listing answers;
/// <see cref="Default"/> follows the <c>administration</c> section's <c>default_state</c>.</summary>
public sealed class CityState : Dto
{
    public long? Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? Active { get; set; }

    public bool? Default { get; set; }
}

/// <summary>The country a city row carries; <see cref="Id"/> is the one the <c>countries</c> listing answers.</summary>
public sealed class CityCountry : Dto
{
    public long? Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }
}

/// <summary>One listing row.</summary>
public sealed class City : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? PostCode { get; set; }

    public CityState? State { get; set; }

    public CityCountry? Country { get; set; }
}

/// <summary>The five-key <c>pagination</c> block: <see cref="Total"/> is the number of rows that matched,
/// <see cref="Limit"/> the page size applied and <see cref="Offset"/> the start.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the city listing: the rows, and the block that says where they sit.</summary>
public sealed class CityPage : Dto
{
    public IReadOnlyList<City?>? Cities { get; set; }

    public Pagination? Pagination { get; set; }
}
