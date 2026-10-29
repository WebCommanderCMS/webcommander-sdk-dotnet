namespace WebCommander.Resources.Settings.PriceDisplaySettings.PriceDisplaySettingsDto;

/// <summary>The storefront surfaces one setting applies to.</summary>
public sealed class DisplayScopes : Dto
{
    public bool? ShoppingCart { get; set; }

    public bool? Checkout { get; set; }

    public bool? OrderSuccess { get; set; }
}

/// <summary>A setting with a master switch and optional per-surface control through <see cref="Scopes"/>.</summary>
public sealed class ScopedToggle : Dto
{
    public bool? Enabled { get; set; }

    public bool? Customize { get; set; }

    public DisplayScopes? Scopes { get; set; }
}

/// <summary>A setting with nothing but a master switch.</summary>
public sealed class PlainToggle : Dto
{
    public bool? Enabled { get; set; }
}

/// <summary>The <c>subtotal</c> block - where the subtotal line is shown, and whether it carries tax and discount.</summary>
public sealed class Subtotal : Dto
{
    public DisplayScopes? Show { get; set; }

    public ScopedToggle? IncludeTax { get; set; }

    public ScopedToggle? IncludeDiscount { get; set; }
}

/// <summary>The <c>discount</c> block - where the discount line is shown, and whether it hides at zero or carries tax.</summary>
public sealed class Discount : Dto
{
    public DisplayScopes? Show { get; set; }

    public ScopedToggle? HideIfZero { get; set; }

    public ScopedToggle? IncludeTax { get; set; }
}

/// <summary>The <c>shipping_cost</c> block - where the shipping line is shown, and whether it carries tax and discount.</summary>
public sealed class ShippingCost : Dto
{
    public DisplayScopes? Show { get; set; }

    public ScopedToggle? IncludeTax { get; set; }

    public ScopedToggle? IncludeDiscount { get; set; }
}

/// <summary>The <c>shipping_discount</c> block - the shipping discount line, its tax, and the shipping tax switch.</summary>
public sealed class ShippingDiscount : Dto
{
    public DisplayScopes? Show { get; set; }

    public ScopedToggle? IncludeTax { get; set; }

    public PlainToggle? ShippingTax { get; set; }
}

/// <summary>The <c>handling</c> block - whether handling is shown apart from shipping, and its two tax settings.</summary>
public sealed class Handling : Dto
{
    public DisplayScopes? ShowSeparately { get; set; }

    public ScopedToggle? IncludeTax { get; set; }

    public ScopedToggle? HandlingTax { get; set; }
}

/// <summary>The <c>pricing_info</c> block - a note after the total, and whether order success hides the due amount.</summary>
public sealed class PricingInfo : Dto
{
    public bool? Enabled { get; set; }

    public string? Text { get; set; }

    public bool? Customize { get; set; }

    public DisplayScopes? Scopes { get; set; }

    public bool? DoNotShowDueAmount { get; set; }
}

/// <summary>The <c>order_item</c> section - how a line item's own price is displayed, in five toggles.</summary>
public sealed class OrderItem : Dto
{
    protected override string? EnvelopeKey => "order_item";

    public ScopedToggle? PriceIncludingTax { get; set; }

    public ScopedToggle? PriceIncludingDiscount { get; set; }

    public ScopedToggle? TaxInLine { get; set; }

    public ScopedToggle? DiscountInLineItem { get; set; }

    public ScopedToggle? ItemTotalIncludingTax { get; set; }
}

/// <summary>The <c>order_summary</c> section - which lines the summary panel shows and what each one includes.</summary>
public sealed class OrderSummary : Dto
{
    protected override string? EnvelopeKey => "order_summary";

    public ScopedToggle? OrderTotalExcludingTax { get; set; }

    public ScopedToggle? OrderTotalIncludingDiscount { get; set; }

    public Subtotal? Subtotal { get; set; }

    public Discount? Discount { get; set; }

    public PlainToggle? SubtotalTax { get; set; }

    public PlainToggle? TotalTax { get; set; }

    public PlainToggle? OrderTotal { get; set; }

    public ShippingCost? ShippingCost { get; set; }

    public ShippingDiscount? ShippingDiscount { get; set; }

    public Handling? Handling { get; set; }

    public ScopedToggle? PaymentSurcharge { get; set; }

    public PricingInfo? PricingInfo { get; set; }
}

/// <summary>One entry of <c>group_meta</c>: what a thing is, whether it can be written and where it is managed.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block, one <see cref="SectionMeta"/> per section; only the facade sends it.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? OrderItem { get; set; }

    public SectionMeta? OrderSummary { get; set; }
}

/// <summary>The price display settings, as the facade or a single section sends them.</summary>
public sealed class PriceDisplaySettings : Dto
{
    protected override string? EnvelopeKey => "price_display_settings";

    public OrderItem? OrderItem { get; set; }

    public OrderSummary? OrderSummary { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
