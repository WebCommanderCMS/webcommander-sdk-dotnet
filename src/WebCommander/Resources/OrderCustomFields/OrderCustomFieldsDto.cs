namespace WebCommander.Resources.OrderCustomFields.OrderCustomFieldsDto;

/// <summary>One order custom field. A listing row carries the twelve fields up to <see cref="MaxLength"/>;
/// the row path adds the nine after it, and <see cref="FieldMetadata"/> when that is asked for.</summary>
public sealed class OrderCustomField : Dto
{
    protected override string? EnvelopeKey => "order_custom_field";

    public long? Id { get; set; }

    public string? FieldType { get; set; }

    public string? Label { get; set; }

    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public long? OrderNo { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedOn { get; set; }

    public bool? IsRequired { get; set; }

    public string? InputFormat { get; set; }

    public bool? HasMaxLength { get; set; }

    public long? MaxLength { get; set; }

    public string? HoverText { get; set; }

    public string? Placeholder { get; set; }

    public string? DefaultValue { get; set; }

    public string? HtmlClass { get; set; }

    public IReadOnlyList<string?>? Options { get; set; }

    public bool? IsAllowedForShipping { get; set; }

    public bool? IsAllowedForDelivery { get; set; }

    public bool? IsAllowedForStorePickup { get; set; }

    public bool? IsAllowedForPreOrder { get; set; }

    /// <summary><c>object</c>, the schema's entries differing in width.</summary>
    public object? FieldMetadata { get; set; }
}

/// <summary>The ten-key <c>pagination</c> block; <see cref="Limit"/> is the size the store applied, capped at 20.</summary>
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

/// <summary>The listing's one <c>group_meta</c> entry: <c>kind</c> <c>resource</c>, <c>writable</c> <c>false</c>.</summary>
public sealed class ResourceMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The listing's <c>group_meta</c> block, keyed by the collection's own name.</summary>
public sealed class GroupMeta : Dto
{
    public ResourceMeta? OrderCustomFields { get; set; }
}

/// <summary>One page of the listing: the rows, where they sit, <c>group_meta</c> and any schema.</summary>
public sealed class OrderCustomFieldPage : Dto
{
    public IReadOnlyList<OrderCustomField?>? OrderCustomFields { get; set; }

    public Pagination? Pagination { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary>The <c>validate_timeslot</c> body; no envelope. <see cref="SelectedDate"/> is required, <c>dd/MM/yyyy</c>;
/// an <see cref="OrderLimit"/> above 0 makes both hours required and <see cref="FieldName"/> read, as text.</summary>
public sealed class OrderCustomFieldTimeslotInput : Dto
{
    public string? SelectedDate { get; set; }

    public string? FieldName { get; set; }

    public long? WeekdayId { get; set; }

    public long? StartHour { get; set; }

    public long? EndHour { get; set; }

    public long? OrderLimit { get; set; }
}
