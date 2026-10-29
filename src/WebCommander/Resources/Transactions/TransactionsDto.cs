namespace WebCommander.Resources.Transactions.TransactionsDto;

/// <summary>One listing row: a number, how many records carry it, and its read path.</summary>
public sealed class TransactionListEntry : Dto
{
    public string? TransactionNo { get; set; }

    public long? TotalRecords { get; set; }

    public string? Url { get; set; }
}

/// <summary>One <c>by_type</c> row of a number's read: <c>count</c> is <c>created</c> plus <c>updated</c>,
/// and <c>trashed</c> counts those of them now in the trash.</summary>
public sealed class TransactionTypeCount : Dto
{
    public string? Type { get; set; }

    public long? Count { get; set; }

    public long? Created { get; set; }

    public long? Updated { get; set; }

    public long? Trashed { get; set; }

    /// <summary>Sent on every row, <c>count</c> 0 included; it form-encodes the number, so a space arrives as <c>+</c>.</summary>
    public string? ItemsUrl { get; set; }
}

/// <summary>One number's read: a bare body, its 15 <c>by_type</c> rows always in the same order.</summary>
public sealed class Transaction : Dto
{
    public string? TransactionNo { get; set; }

    public long? TotalRecords { get; set; }

    public IReadOnlyList<TransactionTypeCount?>? ByType { get; set; }
}

/// <summary>One record under a number, as <c>detail=slim</c> sends it: <c>type</c>, <c>id</c>, <c>stamp</c>
/// (<c>created</c> or <c>updated</c>), <c>trashed</c> and the fields of its <c>type</c>; a field the type does
/// not send reads <c>null</c>. A <c>full</c> row stays in <c>.Map</c>.</summary>
public sealed class TransactionItem : Dto
{
    public string? Type { get; set; }

    public long? Id { get; set; }

    public string? Stamp { get; set; }

    public bool? Trashed { get; set; }

    public string? Name { get; set; }

    public string? Sku { get; set; }

    public double? Rating { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public long? OrderNo { get; set; }

    public double? GrandTotal { get; set; }

    public string? Status { get; set; }

    public string? Code { get; set; }

    public string? Symbol { get; set; }

    public string? CountryCode { get; set; }

    public string? StateName { get; set; }

    public string? Postcode { get; set; }

    public double? Rate { get; set; }

    public long? Priority { get; set; }

    public string? Description { get; set; }

    public string? PolicyType { get; set; }

    public string? Url { get; set; }
}

/// <summary>The five-key <c>pagination</c> block of both listings. There is no next-page link, so step <c>offset</c>.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    /// <summary>The page size applied.</summary>
    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the numbers; a <c>transaction_no</c> filter answers <c>pagination</c> too.</summary>
public sealed class TransactionPage : Dto
{
    public IReadOnlyList<TransactionListEntry?>? Transactions { get; set; }

    public Pagination? Pagination { get; set; }
}

/// <summary>One page of the records carrying a number, beside the number; each item names its own <c>type</c>.
/// A number or a type with no records answers <c>items</c> empty and <c>pagination.total</c> 0.</summary>
public sealed class TransactionItemPage : Dto
{
    public string? TransactionNo { get; set; }

    public IReadOnlyList<TransactionItem?>? Items { get; set; }

    public Pagination? Pagination { get; set; }
}
