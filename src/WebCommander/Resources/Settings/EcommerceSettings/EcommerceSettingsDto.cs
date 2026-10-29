namespace WebCommander.Resources.Settings.EcommerceSettings.EcommerceSettingsDto;

/// <summary>The <c>continue_shopping</c> section - where the storefront's continue-shopping link goes.</summary>
public sealed class ContinueShopping : Dto
{
    protected override string? EnvelopeKey => "continue_shopping";

    public bool? EnableContinueShopping { get; set; }

    public string? ContinueShoppingTarget { get; set; }

    public string? ContinueShoppingSpecifiedTarget { get; set; }
}

/// <summary>The <c>product_inventory</c> section - out-of-stock handling, and when a sale draws stock down.</summary>
public sealed class ProductInventory : Dto
{
    protected override string? EnvelopeKey => "product_inventory";

    public bool? ShowOutOfStockProducts { get; set; }

    public bool? LowStockNotification { get; set; }

    public bool? OutOfStockNotification { get; set; }

    public bool? HideOnSaleTagForOutOfStockProduct { get; set; }

    public string? OrderQuantityOverStock { get; set; }

    public string? UpdateStock { get; set; }
}

/// <summary>The <c>pricing</c> section - purchase floors, credit and payment terms.
/// The amounts are typed as the body sends them, numbers, not as <c>field_metadata</c> declares them.</summary>
public sealed class Pricing : Dto
{
    protected override string? EnvelopeKey => "pricing";

    public bool? UpdatePriceRange { get; set; }

    public bool? EnableMinimumPurchaseAmount { get; set; }

    public double? MinimumPurchaseAmount { get; set; }

    public bool? EnableMinimumPurchaseAmountInCart { get; set; }

    public double? MinimumPurchaseAmountInCart { get; set; }

    public bool? EnableOnAccountPurchase { get; set; }

    public bool? AssignCreditLimitPerOrder { get; set; }

    public double? CreditLimitPerOrder { get; set; }

    public bool? EnablePaymentTerms { get; set; }

    public string? PaymentTerms { get; set; }

    public bool? EnableCustomerPriceRestriction { get; set; }
}

/// <summary>The <c>product_restriction</c> section - one switch.</summary>
public sealed class ProductRestriction : Dto
{
    protected override string? EnvelopeKey => "product_restriction";

    public bool? EnableProductRestrictionBasedOnParentCategory { get; set; }
}

/// <summary>The <c>order_filters</c> section - the status each admin order list opens on.</summary>
public sealed class OrderFilters : Dto
{
    protected override string? EnvelopeKey => "order_filters";

    public string? DefaultOrderStatus { get; set; }

    public string? DefaultPaymentStatus { get; set; }

    public string? DefaultShipmentStatus { get; set; }
}

/// <summary>One row of <c>location_settings.locations</c> - a stock location as the facade previews it.</summary>
public sealed class InventoryLocation : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public bool? Active { get; set; }

    public bool? Default { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The <c>location_settings</c> section - the multi-location switch and a read-only preview of the locations.</summary>
public sealed class LocationSettings : Dto
{
    protected override string? EnvelopeKey => "location_settings";

    public bool? EnableMultiLocation { get; set; }

    public IReadOnlyList<InventoryLocation?>? Locations { get; set; }
}

/// <summary>One row of <c>order_flags</c> - a flag as the facade inlines it; nothing here writes one.</summary>
public sealed class OrderFlag : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Color { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }
}

/// <summary>One entry of <c>group_meta</c>: what a thing is, whether it can be written and where it is managed.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }

    public string? LocationsHref { get; set; }
}

/// <summary>The <c>group_meta</c> block, one <see cref="SectionMeta"/> per area key; only the facade sends it.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? ContinueShopping { get; set; }

    public SectionMeta? ProductInventory { get; set; }

    public SectionMeta? Pricing { get; set; }

    public SectionMeta? ProductRestriction { get; set; }

    public SectionMeta? OrderFilters { get; set; }

    public SectionMeta? LocationSettings { get; set; }

    public SectionMeta? OrderFlags { get; set; }
}

/// <summary>The ecommerce settings, as the facade or a single section sends them.</summary>
public sealed class EcommerceSettings : Dto
{
    protected override string? EnvelopeKey => "ecommerce_settings";

    public ContinueShopping? ContinueShopping { get; set; }

    public ProductInventory? ProductInventory { get; set; }

    public Pricing? Pricing { get; set; }

    public ProductRestriction? ProductRestriction { get; set; }

    public OrderFilters? OrderFilters { get; set; }

    public LocationSettings? LocationSettings { get; set; }

    public IReadOnlyList<OrderFlag?>? OrderFlags { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
