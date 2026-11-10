namespace WebCommander.Resources.ShippingRates.ShippingRatesDto;

/// <summary>An <c>{id, name}</c> pair: a rule that uses the rate.</summary>
public sealed class ShippingReference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>A shipping or handling cost; <see cref="Type"/> is <c>fixed</c> or <c>percent</c>.</summary>
public sealed class ShippingCost : Dto
{
    public double? Amount { get; set; }

    public string? Type { get; set; }
}

/// <summary>The charge for each item past the first; the other two keys read <c>null</c> while <see cref="Enabled"/> is
/// false, and <see cref="Enabled"/> true needs both.</summary>
public sealed class ShippingAdditionalRule : Dto
{
    public bool? Enabled { get; set; }

    public long? ForEachAdditional { get; set; }

    public double? AdditionalCost { get; set; }
}

/// <summary>One rate, the same fourteen keys on a listing row, the row read and every write's answer, whatever the
/// method. <see cref="Method"/> is <c>flat_rate</c>, <c>free_shipping</c>, <c>ship_by_price</c>,
/// <c>ship_by_quantity</c>, <c>ship_by_weight</c>, <c>ship_by_variation</c> or <c>shipping_variant</c>. A flat rate
/// carries <see cref="ShippingCost"/> and <see cref="HandlingCost"/>, the <c>ship_by_*</c> methods carry them per row in
/// <see cref="Conditions"/>, and a key the method does not use reads <c>null</c> or an empty list.
/// <see cref="WeightUnit"/> is the store's, <c>null</c> unless the method is <c>ship_by_weight</c>.</summary>
public sealed class ShippingRate : Dto
{
    protected override string? EnvelopeKey => "shipping_rate";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Method { get; set; }

    public bool? CalculateForEntireOrder { get; set; }

    public bool? PriceEnteredWithTax { get; set; }

    public ShippingCost? ShippingCost { get; set; }

    public ShippingCost? HandlingCost { get; set; }

    public string? WeightUnit { get; set; }

    /// <summary>Rows stay untyped: a price, quantity or weight row is <c>{from, to, shipping_cost, handling_cost}</c>
    /// (a weight row adds <c>packet_weight</c>) and a variation row is <c>{variation_combination, shipping_cost,
    /// handling_cost}</c>.</summary>
    public IReadOnlyList<object?>? Conditions { get; set; }

    public ShippingAdditionalRule? AdditionalRule { get; set; }

    public IReadOnlyList<ShippingReference?>? ShippingRules { get; set; }

    public string? TransactionNo { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The writable fields, wrapped in <c>shipping_rate</c>, for the create, the <c>PUT</c> replace and the
/// <c>PATCH</c>. <see cref="Name"/> and <see cref="Method"/> are required on a create; a flat rate also needs
/// <see cref="ShippingCost"/> and the <c>ship_by_*</c> methods need <see cref="Conditions"/>. <c>PUT</c> needs every field
/// of the rate, <see cref="HandlingCost"/> and <see cref="AdditionalRule"/> as <c>null</c> in a map when unused.
/// <see cref="ShippingRuleId"/> attaches the new rate to a rule that has none. A <c>null</c> field is not sent: to clear
/// one, pass a map.</summary>
public sealed class ShippingRateInput : Dto
{
    protected override string? EnvelopeKey => "shipping_rate";

    public string? Name { get; set; }

    public string? Method { get; set; }

    public bool? CalculateForEntireOrder { get; set; }

    public bool? PriceEnteredWithTax { get; set; }

    public ShippingCost? ShippingCost { get; set; }

    public ShippingCost? HandlingCost { get; set; }

    /// <summary>Rows as maps, in the shape <see cref="ShippingRate.Conditions"/> reads.</summary>
    public IReadOnlyList<object?>? Conditions { get; set; }

    public ShippingAdditionalRule? AdditionalRule { get; set; }

    public long? ShippingRuleId { get; set; }

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
public sealed class ShippingRateListing : Dto
{
    public IReadOnlyList<ShippingRate?>? ShippingRates { get; set; }

    public Pagination? Pagination { get; set; }
}

/// <summary>The count sub-path's body, a bare <c>{"total": N}</c>.</summary>
public sealed class ShippingRateCount : Dto
{
    public long? Total { get; set; }
}
