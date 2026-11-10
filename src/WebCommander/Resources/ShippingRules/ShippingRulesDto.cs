namespace WebCommander.Resources.ShippingRules.ShippingRulesDto;

/// <summary>An <c>{id, name}</c> pair: the rule's class, rate, zones and profiles. A write names a class, a rate or a
/// zone by its id, bare or as <c>{"id": ...}</c>; <see cref="Name"/> is read-only.</summary>
public sealed class ShippingReference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>One rule, the same ten keys on a listing row, the row read and every write's answer.
/// <see cref="ShippingClass"/> and <see cref="ShippingRate"/> are <c>null</c> when unset, <see cref="Zones"/> and
/// <see cref="Profiles"/> empty lists.</summary>
public sealed class ShippingRule : Dto
{
    protected override string? EnvelopeKey => "shipping_rule";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public ShippingReference? ShippingClass { get; set; }

    public ShippingReference? ShippingRate { get; set; }

    public IReadOnlyList<ShippingReference?>? Zones { get; set; }

    public IReadOnlyList<ShippingReference?>? Profiles { get; set; }

    public string? TransactionNo { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The writable fields, wrapped in <c>shipping_rule</c>, for the create, the <c>PUT</c> replace and the
/// <c>PATCH</c>. <see cref="Zones"/> is the whole list of zones, each once; <see cref="ProfileId"/> adds the new rule to a
/// profile on a create. <c>PUT</c> needs <see cref="Name"/>, <see cref="Description"/>, <see cref="ShippingClass"/>,
/// <see cref="ShippingRate"/> and <see cref="Zones"/>. A <c>null</c> field is not sent: to clear a class or a rate, pass
/// a map holding <c>null</c>.</summary>
public sealed class ShippingRuleInput : Dto
{
    protected override string? EnvelopeKey => "shipping_rule";

    public string? Name { get; set; }

    public string? Description { get; set; }

    /// <summary>A class id, or a <see cref="ShippingReference"/>.</summary>
    public object? ShippingClass { get; set; }

    /// <summary>A rate id, or a <see cref="ShippingReference"/>.</summary>
    public object? ShippingRate { get; set; }

    /// <summary>Zone ids, or <see cref="ShippingReference"/> items.</summary>
    public IReadOnlyList<object?>? Zones { get; set; }

    public long? ProfileId { get; set; }

    public string? TransactionNo { get; set; }

    public bool? SuggestName { get; set; }
}

/// <summary>The five-key <c>pagination</c> block; <see cref="Limit"/> is the size the store applied.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the listing, newest first.</summary>
public sealed class ShippingRuleListing : Dto
{
    public IReadOnlyList<ShippingRule?>? ShippingRules { get; set; }

    public Pagination? Pagination { get; set; }
}

/// <summary>The count sub-path's body, a bare <c>{"total": N}</c>.</summary>
public sealed class ShippingRuleCount : Dto
{
    public long? Total { get; set; }
}
