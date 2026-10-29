namespace WebCommander.Resources.ShippingMethods.ShippingMethodsDto;

/// <summary>One shipping method, the same three fields on a listing row and on the row path.</summary>
public sealed class ShippingMethod : Dto
{
    protected override string? EnvelopeKey => "shipping_method";

    /// <summary>What <c>{shipping_method_id}</c> takes; an integer the store assigns.</summary>
    public long? Id { get; set; }

    /// <summary>Unique across the collection, and 100 characters or fewer.</summary>
    public string? Name { get; set; }

    /// <summary>Held by at most one row; only the default sub-path writes it.</summary>
    public bool? IsDefault { get; set; }

    /// <summary>The schema block <c>field_metadata=true</c> adds beside the row.</summary>
    public object? FieldMetadata { get; set; }
}

/// <summary>The writable form of <see cref="ShippingMethod"/>: <see cref="Name"/>, the one field read.</summary>
public sealed class ShippingMethodInput : Dto
{
    protected override string? EnvelopeKey => "shipping_method";

    public string? Name { get; set; }
}

/// <summary>The ten-key <c>pagination</c> block of the method listing.</summary>
public sealed class Pagination : Dto
{
    /// <summary>How many rows the collection holds across every page.</summary>
    public long? Total { get; set; }

    /// <summary>The page size the store applied, capped at 20 whatever was asked.</summary>
    public long? Limit { get; set; }

    public long? Offset { get; set; }

    /// <summary>The rows on this page; <see cref="Total"/> is the rows matching.</summary>
    public long? Count { get; set; }

    public long? CurrentPage { get; set; }

    public long? TotalPages { get; set; }

    public bool? HasNext { get; set; }

    public bool? HasPrevious { get; set; }

    /// <summary>A whole URL carrying the query, or <c>null</c>.</summary>
    public string? PreviousPage { get; set; }

    /// <summary>A whole URL carrying the query, or <c>null</c>.</summary>
    public string? NextPage { get; set; }
}

/// <summary>One page of the listing: the rows, the block beside them and any schema block.</summary>
public sealed class ShippingMethodPage : Dto
{
    public IReadOnlyList<ShippingMethod?>? ShippingMethods { get; set; }

    public Pagination? Pagination { get; set; }

    /// <summary>The same schema block the row path serves.</summary>
    public object? FieldMetadata { get; set; }
}

/// <summary>The count sub-path's body, <c>{"shipping_methods": {"count": N}}</c>.</summary>
public sealed class ShippingMethodCount : Dto
{
    protected override string? EnvelopeKey => "shipping_methods";

    public long? Count { get; set; }
}
