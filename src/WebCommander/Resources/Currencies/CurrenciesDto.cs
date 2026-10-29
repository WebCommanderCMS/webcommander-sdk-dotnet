namespace WebCommander.Resources.Currencies.CurrenciesDto;

/// <summary>The country a currency is filed under, as every row answers it.</summary>
public sealed class CurrencyCountry : Dto
{
    public long? Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }
}

/// <summary>One currency: a listing row, the row path's body and every row write's answer. The base
/// write answers only <see cref="Id"/>, <see cref="Name"/>, <see cref="Code"/> and <see cref="Base"/> of it.</summary>
public sealed class Currency : Dto
{
    protected override string? EnvelopeKey => "currency";

    public long? Id { get; set; }

    public string? Name { get; set; }

    /// <summary>Three letters, stored upper-case and unique.</summary>
    public string? Code { get; set; }

    public string? Symbol { get; set; }

    public CurrencyCountry? Country { get; set; }

    /// <summary>Kept to 6 decimal places.</summary>
    public double? ConversionRate { get; set; }

    public string? Rounding { get; set; }

    public long? Precision { get; set; }

    public bool? Active { get; set; }

    /// <summary>Held by exactly one row; only <c>PUT .../base</c> moves it.</summary>
    public bool? Base { get; set; }

    public string? UpdatedAt { get; set; }

    /// <summary><c>object</c>, the schema's entries differing in width.</summary>
    public object? FieldMetadata { get; set; }
}

/// <summary>The writable fields, for the create, the <c>PUT</c> replace and the <c>PATCH</c>; no envelope.
/// <see cref="Country"/> takes a country's id, code, name or a <see cref="CurrencyCountry"/>.</summary>
public sealed class CurrencyInput : Dto
{
    public string? Name { get; set; }

    public string? Code { get; set; }

    public string? Symbol { get; set; }

    public object? Country { get; set; }

    public double? ConversionRate { get; set; }

    public string? Rounding { get; set; }

    public long? Precision { get; set; }

    public bool? Active { get; set; }
}

/// <summary>The five-key <c>pagination</c> block; <see cref="Limit"/> is the page size applied.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the listing: the rows, where they sit and any schema.</summary>
public sealed class CurrencyPage : Dto
{
    public IReadOnlyList<Currency?>? Currencies { get; set; }

    public Pagination? Pagination { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary>The count sub-path's body, a bare <c>{"total": N}</c>.</summary>
public sealed class CurrencyCount : Dto
{
    public long? Total { get; set; }
}

/// <summary>The row that held the base flag before a base write.</summary>
public sealed class CurrencyReference : Dto
{
    public long? Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }
}

/// <summary>The base write's body: the new base, and the row that held the flag or <c>null</c>.</summary>
public sealed class CurrencyBaseChange : Dto
{
    public Currency? Currency { get; set; }

    public CurrencyReference? PreviousBase { get; set; }
}
