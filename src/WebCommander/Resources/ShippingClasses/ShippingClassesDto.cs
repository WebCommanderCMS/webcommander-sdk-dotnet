namespace WebCommander.Resources.ShippingClasses.ShippingClassesDto;

/// <summary>One shipping class, the same four fields on a listing row and on the row path.</summary>
public sealed class ShippingClass : Dto
{
    protected override string? EnvelopeKey => "shipping_class";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? IsDefault { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary>The body both writes take; <see cref="Name"/> is required and unique.</summary>
public sealed class ShippingClassInput : Dto
{
    protected override string? EnvelopeKey => "shipping_class";

    public string? Name { get; set; }

    public string? Description { get; set; }
}

/// <summary>The listing's ten-key <c>pagination</c> block.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public long? Count { get; set; }

    public long? CurrentPage { get; set; }

    public long? TotalPages { get; set; }

    public bool? HasNext { get; set; }

    public bool? HasPrevious { get; set; }

    public string? PreviousPage { get; set; }

    public string? NextPage { get; set; }
}

/// <summary>One page of the listing, with the schema when it was asked for.</summary>
public sealed class ShippingClassPage : Dto
{
    public IReadOnlyList<ShippingClass?>? ShippingClasses { get; set; }

    public Pagination? Pagination { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary>The count sub-path's <c>{"shipping_classes": {"count": N}}</c>.</summary>
public sealed class ShippingClassCount : Dto
{
    protected override string? EnvelopeKey => "shipping_classes";

    public long? Count { get; set; }
}
