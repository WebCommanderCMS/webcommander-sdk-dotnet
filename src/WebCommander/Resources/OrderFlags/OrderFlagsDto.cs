namespace WebCommander.Resources.OrderFlags.OrderFlagsDto;

/// <summary>One order flag, as a listing row and as the row path send it.</summary>
public sealed class OrderFlag : Dto
{
    protected override string? EnvelopeKey => "order_flag";

    /// <summary>What <c>{order_flag_id}</c> takes; an integer the store assigns, never the caller.</summary>
    public long? Id { get; set; }

    /// <summary>Unique across the collection: a name another flag holds is <c>409 conflict</c>.</summary>
    public string? Name { get; set; }

    /// <summary>Required on both writes; a hex code like <c>#FF6868</c>.</summary>
    public string? Color { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }
}

/// <summary>The writable form of <see cref="OrderFlag"/>; the update replaces, blanking unset fields.</summary>
public sealed class OrderFlagInput : Dto
{
    protected override string? EnvelopeKey => "order_flag";

    public string? Name { get; set; }

    public string? Color { get; set; }

    public string? Description { get; set; }
}

/// <summary>The five-key <c>pagination</c> block of the flag listing.</summary>
public sealed class Pagination : Dto
{
    /// <summary>How many rows the collection holds across every page.</summary>
    public long? Total { get; set; }

    /// <summary>The page size the store applied, capped at 20 whatever was asked.</summary>
    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasNext { get; set; }

    public bool? HasPrevious { get; set; }
}

/// <summary>One page of the flag listing: the rows and the block beside them, with no envelope.</summary>
public sealed class OrderFlagPage : Dto
{
    public IReadOnlyList<OrderFlag?>? OrderFlags { get; set; }

    public Pagination? Pagination { get; set; }
}
