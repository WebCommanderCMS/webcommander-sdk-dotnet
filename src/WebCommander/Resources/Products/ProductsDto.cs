using System.Text.Json.Serialization;

namespace WebCommander.Resources.Products.ProductsDto;

/// <summary>The product's slug and its absolute storefront address.</summary>
public sealed class ProductUrls : Dto
{
    public string? Url { get; set; }

    public string? ProductUrl { get; set; }
}

/// <summary>An <c>id</c> and a <c>name</c>: an author, a page, a customer or a customer group.</summary>
public sealed class ProductNamedReference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>One category the product is filed under.</summary>
public sealed class ProductParent : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public bool? IsInTrash { get; set; }

    public bool? IsParentInTrash { get; set; }

    public bool? IsDisposable { get; set; }
}

/// <summary>One customer an audience names: <c>id</c> is the customer's <c>customer_id</c>, <c>internal_id</c> its
/// <c>internal_id</c>.</summary>
public sealed class ProductSelectedCustomer : Dto
{
    public long? Id { get; set; }

    public long? InternalId { get; set; }

    public string? Name { get; set; }
}

/// <summary>Who <c>available_for: selected</c> admits, or whom a restriction names; both lists are empty when nobody
/// is named.</summary>
public sealed class ProductSelectedCustomers : Dto
{
    public IReadOnlyList<ProductSelectedCustomer?>? Customers { get; set; }

    public IReadOnlyList<ProductNamedReference?>? Groups { get; set; }
}

/// <summary>An audience to write: customer and group ids. A group written as <c>{"id": ...}</c> is <c>400</c>.</summary>
public sealed class ProductSelectedCustomersInput : Dto
{
    public IReadOnlyList<long?>? Customers { get; set; }

    public IReadOnlyList<long?>? Groups { get; set; }
}

/// <summary>One day of the weekday schedule.</summary>
public sealed class ProductWeekday : Dto
{
    public string? Day { get; set; }

    public long? StartHour { get; set; }

    public long? StartMinute { get; set; }

    public long? EndHour { get; set; }

    public long? EndMinute { get; set; }

    public bool? IsFullDay { get; set; }

    public bool? Enabled { get; set; }
}

/// <summary>The weekday schedule: a switch and the seven days.</summary>
public sealed class ProductAvailableOnWeekdays : Dto
{
    public bool? Enabled { get; set; }

    public IReadOnlyList<ProductWeekday?>? Weekdays { get; set; }
}

/// <summary>Dimensions as written; the top level of the product repeats the four numbers read-only.</summary>
public sealed class ProductProperties : Dto
{
    public double? Length { get; set; }

    public double? Width { get; set; }

    public double? Height { get; set; }

    public double? Weight { get; set; }

    public string? LengthUnit { get; set; }

    public string? WeightUnit { get; set; }
}

/// <summary>Per-product tax overrides, applied only when <see cref="OverwriteDefaultConfig"/> is <c>true</c>.</summary>
public sealed class ProductTaxSettings : Dto
{
    public bool? OverwriteDefaultConfig { get; set; }

    public bool? IsTaxExempt { get; set; }

    public bool? ShowPriceWithTax { get; set; }

    public bool? IsPriceWithTax { get; set; }

    public bool? SystemDefaultTax { get; set; }

    public string? TaxMessage { get; set; }

    public string? BasePriceRounding { get; set; }

    public string? DefaultTaxCode { get; set; }

    public string? CustomTaxProfileId { get; set; }
}

/// <summary>Which variant price a listing shows, and the labels it shows it with. <c>lowest_price_label</c> and
/// <c>highest_price_label</c> are second names for the two <c>label_for_*</c> fields.</summary>
public sealed class ProductVariationPriceDisplay : Dto
{
    public string? Method { get; set; }

    public string? LabelForLowestPrice { get; set; }

    public string? LabelForHighestPrice { get; set; }

    public string? LowestPriceLabel { get; set; }

    public string? HighestPriceLabel { get; set; }
}

/// <summary>One location's units in the legacy shape; <see cref="Warehouse"/> is the location id as text.</summary>
public sealed class ProductWarehouseStock : Dto
{
    public string? Warehouse { get; set; }

    public long? Stock { get; set; }
}

/// <summary>One location's holding of the product; exactly one row of a product holds <see cref="IsDefault"/>.</summary>
public sealed class ProductStockLocation : Dto
{
    public long? Warehouse { get; set; }

    public string? WarehouseName { get; set; }

    public long? Stock { get; set; }

    /// <summary><c>null</c> when the row was written without one.</summary>
    public long? LowStockLevel { get; set; }

    public bool? IsDefault { get; set; }
}

/// <summary>The pricing and stock block. <see cref="Locations"/> matches the stock sub-path's rows, and
/// <see cref="LowLevelStock"/> follows the default location's <c>low_stock_level</c>.</summary>
public sealed class ProductPricingAndStock : Dto
{
    public bool? TrackInventory { get; set; }

    public long? AvailableStock { get; set; }

    public long? LowLevelStock { get; set; }

    public IReadOnlyList<ProductWarehouseStock?>? Warehouses { get; set; }

    public long? MinimumOrderQuantity { get; set; }

    public long? MaximumOrderQuantity { get; set; }

    public object? TaxProfile { get; set; }

    public object? ShippingProfile { get; set; }

    public bool? EnableMultipleOrder { get; set; }

    public long? MultipleOrderQuantity { get; set; }

    public bool? OnSale { get; set; }

    public string? OnSalePriceType { get; set; }

    public double? OnSalePrice { get; set; }

    public bool? ExpectedToPay { get; set; }

    public double? ExpectedToPayPrice { get; set; }

    public bool? EnableCallForPrice { get; set; }

    public string? RestrictPriceFor { get; set; }

    public IReadOnlyList<object?>? RestrictPriceForExceptCustomers { get; set; }

    public string? RestrictPurchaseFor { get; set; }

    public IReadOnlyList<object?>? RestrictPurchaseForSelectedCustomers { get; set; }

    public bool? HidePrice { get; set; }

    public bool? EnableCombinedStockSellingLimit { get; set; }

    public long? CombinedStockSellingLimit { get; set; }

    public bool? EnableAdvancePricing { get; set; }

    public IReadOnlyList<object?>? Suppliers { get; set; }

    public bool? IsMultiLocationEnabled { get; set; }

    public IReadOnlyList<ProductStockLocation?>? Locations { get; set; }
}

/// <summary>One product image.</summary>
public sealed class ProductImage : Dto
{
    public long? Id { get; set; }

    public string? Filename { get; set; }

    public string? AltText { get; set; }

    public string? Thumbnail { get; set; }

    public string? Link { get; set; }
}

/// <summary>One product video, on the product or on a variant.</summary>
public sealed class ProductVideo : Dto
{
    public long? Id { get; set; }

    public string? Filename { get; set; }

    public string? Thumbnail { get; set; }

    public string? Link { get; set; }
}

/// <summary>The media block.</summary>
public sealed class ProductImageAndVideo : Dto
{
    public IReadOnlyList<ProductImage?>? Images { get; set; }

    public IReadOnlyList<ProductVideo?>? Videos { get; set; }

    public IReadOnlyList<object?>? Specs { get; set; }
}

/// <summary>Identifiers and origin; <c>mpn</c>, <c>upc</c>, <c>barcode</c> and <c>hs_code</c> are written here,
/// not at the top level, which refuses them.</summary>
public sealed class ProductAdvanceInformation : Dto
{
    public string? GlobalTradeItemNumber { get; set; }

    public bool? EnableProductIdentifiers { get; set; }

    public string? Gtin { get; set; }

    public string? Mpn { get; set; }

    public string? Upc { get; set; }

    public string? Barcode { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string? HsCode { get; set; }

    public string? ProductCondition { get; set; }

    public string? Model { get; set; }
}

/// <summary>A flag and the label shown with it.</summary>
public sealed class ProductFlag : Dto
{
    public bool? Enabled { get; set; }

    public string? Label { get; set; }
}

/// <summary>The flags in blocks: <c>is_featured</c>, <c>is_new</c>, <c>sold_out</c> and <c>model</c> again.
/// <c>sold_out</c> is read-only.</summary>
public sealed class ProductFlags : Dto
{
    public ProductFlag? Featured { get; set; }

    public bool? NewProduct { get; set; }

    public ProductFlag? SoldOut { get; set; }

    public string? ModelNumber { get; set; }
}

/// <summary><c>product_properties</c> again; the two units are read-only here.</summary>
public sealed class ProductDimension : Dto
{
    public string? LengthUnit { get; set; }

    public string? WeightUnit { get; set; }

    public double? Width { get; set; }

    public double? Height { get; set; }

    public double? Length { get; set; }

    public double? Weight { get; set; }
}

/// <summary><c>advance_information</c> again: a write here changes it there, <c>condition</c> being its
/// <c>product_condition</c>.</summary>
public sealed class ProductIdentifiers : Dto
{
    public bool? Enabled { get; set; }

    public string? Gtin { get; set; }

    public string? Condition { get; set; }

    public string? Barcode { get; set; }

    public string? HsCode { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string? Mpn { get; set; }

    public string? Upc { get; set; }
}

/// <summary><c>webtool_disable_tracking</c> again, read-only.</summary>
public sealed class ProductWebtool : Dto
{
    public bool? DisableTracking { get; set; }
}

/// <summary>The advanced block.</summary>
public sealed class ProductAdvanced : Dto
{
    public ProductAdvanceInformation? AdvanceInformation { get; set; }

    public bool? WebtoolDisableTracking { get; set; }

    public IReadOnlyList<object?>? MetaTags { get; set; }

    public ProductFlags? ProductFlags { get; set; }

    public ProductDimension? ProductDimension { get; set; }

    public ProductIdentifiers? ProductIdentifiers { get; set; }

    public ProductWebtool? Webtool { get; set; }
}

/// <summary>A related, similar or up-sell product. A write takes <c>{"id": ...}</c>; a list of bare ids in
/// <c>up_sell_products</c> is answered <c>200</c> and leaves it empty.</summary>
public sealed class ProductLink : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public string? Status { get; set; }

    public string? Sku { get; set; }

    public bool? InStock { get; set; }

    public double? Price { get; set; }
}

/// <summary>The bundle block; <c>bundle</c> is <c>null</c> on a product that is not a bundle. No bundle item
/// was read, so <see cref="Items"/> is untyped.</summary>
public sealed class ProductBundle : Dto
{
    public bool? PriceFixed { get; set; }

    public bool? QuantityFlexible { get; set; }

    public IReadOnlyList<object?>? Items { get; set; }
}

/// <summary>One SEO setting on the product.</summary>
public sealed class ProductSeoConfig : Dto
{
    public long? Id { get; set; }

    public string? Value { get; set; }

    public string? Type { get; set; }

    public string? ConfigKey { get; set; }

    public IReadOnlyList<object?>? MetaTags { get; set; }
}

/// <summary>One value of a product tag.</summary>
public sealed class ProductTagValue : Dto
{
    public long? Id { get; set; }

    public string? Title { get; set; }

    public string? Value { get; set; }

    public string? Url { get; set; }
}

/// <summary>One tag on the product, with its values.</summary>
public sealed class ProductTag : Dto
{
    public long? Id { get; set; }

    public long? TagId { get; set; }

    public string? Name { get; set; }

    public IReadOnlyList<ProductTagValue?>? Values { get; set; }

    public bool? DisplayInProductPage { get; set; }

    public bool? EnableExpandedView { get; set; }
}

/// <summary>One custom property.</summary>
public sealed class ProductCustomProperty : Dto
{
    public long? Id { get; set; }

    public string? Label { get; set; }

    public string? Description { get; set; }
}

/// <summary>Who wrote a review; <c>name</c> and <c>email</c> again.</summary>
public sealed class ProductReviewer : Dto
{
    public string? Name { get; set; }

    public string? Email { get; set; }
}

/// <summary>One review; the store's <c>productId</c> is the one camelCase key in the body. <c>comment</c> and
/// <c>created_at</c> repeat <c>review</c> and <c>date</c>; <c>created_at</c> carries a <c>Z</c>.</summary>
public sealed class ProductReview : Dto
{
    public long? Id { get; set; }

    public long? ProductId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Review { get; set; }

    public double? Rating { get; set; }

    public string? Date { get; set; }

    public string? Status { get; set; }

    public IReadOnlyList<object?>? Files { get; set; }

    public ProductReviewer? Reviewer { get; set; }

    public string? Comment { get; set; }

    public string? CreatedAt { get; set; }

    /// <summary>The key the store sends the product id under.</summary>
    public const string StoreProductIdKey = "productId";

    /// <summary>The key <see cref="ProductId"/> reads, which the property name derives.</summary>
    public const string DeclaredProductIdKey = "product_id";

    /// <summary>File the store's <c>productId</c> under <c>product_id</c>, the key a property name derives.</summary>
    protected override IReadOnlyDictionary<string, object?> ReadSource(
        IReadOnlyDictionary<string, object?> body)
    {
        var source = base.ReadSource(body);
        if (!source.TryGetValue(StoreProductIdKey, out var productId))
            return source;
        return new Dictionary<string, object?>(source, StringComparer.Ordinal)
        {
            [DeclaredProductIdKey] = productId,
        };
    }
}

/// <summary>One option of a variation type the product uses.</summary>
public sealed class ProductVariationTypeOption : Dto
{
    public long? Id { get; set; }

    public string? Label { get; set; }

    public string? Value { get; set; }

    public bool? Default { get; set; }
}

/// <summary>One variation type the product uses, with its options.</summary>
public sealed class ProductVariationType : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Standard { get; set; }

    public IReadOnlyList<ProductVariationTypeOption?>? Options { get; set; }
}

/// <summary>The type one option of a combination belongs to.</summary>
public sealed class ProductCombinationOptionType : Dto
{
    public long? Id { get; set; }

    public string? Role { get; set; }

    public string? Name { get; set; }

    public string? Standard { get; set; }

    public bool? IsDisposable { get; set; }
}

/// <summary>One option of a combination; <see cref="Type"/> is a list of one type.</summary>
public sealed class ProductCombinationOption : Dto
{
    public long? Id { get; set; }

    public long? Index { get; set; }

    public IReadOnlyList<ProductCombinationOptionType?>? Type { get; set; }

    public string? Label { get; set; }

    public string? Value { get; set; }

    public bool? ColorHasSwatch { get; set; }

    public string? ColorSwatchValue { get; set; }

    public string? ImageBaseUrl { get; set; }
}

/// <summary>One variant image.</summary>
public sealed class ProductCombinationImage : Dto
{
    public long? Id { get; set; }

    public string? Thumbnail { get; set; }

    public string? Url { get; set; }
}

/// <summary>A variant's pricing and stock block.</summary>
public sealed class ProductCombinationPriceAndStock : Dto
{
    public bool? TrackInventory { get; set; }

    public long? AvailableStock { get; set; }

    public long? LowLevelStock { get; set; }

    public long? MinimumOrderQuantity { get; set; }

    public long? MaximumOrderQuantity { get; set; }

    public long? SupportedMaxOrderQuantity { get; set; }

    public bool? EnableMultipleOrder { get; set; }

    public long? MultipleOrderQuantity { get; set; }

    public bool? OnSale { get; set; }

    public bool? SoldOut { get; set; }

    public string? SoldOutLabel { get; set; }

    public string? OnSalePriceType { get; set; }

    public double? OnSalePrice { get; set; }

    public bool? ExpectedToPay { get; set; }

    public double? ExpectedToPayPrice { get; set; }

    public bool? EnableCallForPrice { get; set; }

    public bool? HidePrice { get; set; }
}

/// <summary>One variant. <see cref="Weight"/>, <see cref="Height"/>, <see cref="Length"/> and <see cref="Width"/>
/// arrive as a number or as text, so they are declared <c>object</c>.</summary>
public sealed class ProductCombinationDetails : Dto
{
    public long? VariationId { get; set; }

    public bool? Available { get; set; }

    public string? ProductName { get; set; }

    public string? Sku { get; set; }

    public string? CustomSku { get; set; }

    public ProductUrls? Urls { get; set; }

    public IReadOnlyList<object?>? MetaTags { get; set; }

    public bool? IsInventoryEnabled { get; set; }

    public long? AvailableStock { get; set; }

    public long? LowStockLevel { get; set; }

    public string? Spec { get; set; }

    public bool? IsMultipleOrderQuantity { get; set; }

    public bool? IsOnSale { get; set; }

    public bool? IsExpectToPay { get; set; }

    public bool? IsCallForPriceEnabled { get; set; }

    public bool? IsNew { get; set; }

    public bool? IsFeatured { get; set; }

    public string? OnSaleAmountType { get; set; }

    public string? Title { get; set; }

    public string? Heading { get; set; }

    public string? Summary { get; set; }

    public string? Description { get; set; }

    public string? Model { get; set; }

    public string? ProductCondition { get; set; }

    public string? FeaturedLabel { get; set; }

    public double? BasePrice { get; set; }

    public double? CostPrice { get; set; }

    public double? CompareAtPrice { get; set; }

    public double? SalePrice { get; set; }

    public double? ExpectToPayPrice { get; set; }

    public object? Weight { get; set; }

    public object? Height { get; set; }

    public object? Length { get; set; }

    public object? Width { get; set; }

    public double? DisplayPrice { get; set; }

    public double? PreviousPrice { get; set; }

    public ProductCombinationPriceAndStock? PriceAndStock { get; set; }

    public IReadOnlyList<ProductCombinationImage?>? Images { get; set; }

    public IReadOnlyList<ProductVideo?>? Videos { get; set; }

    public IReadOnlyList<object?>? Specs { get; set; }

    public long? SupportedMaxOrderQuantity { get; set; }

    public bool? IsBase { get; set; }

    public string? Barcode { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string? HsCode { get; set; }

    public double? Tax { get; set; }

    public string? TaxMessage { get; set; }
}

/// <summary>One option combination and the variant it makes.</summary>
public sealed class ProductCombination : Dto
{
    public IReadOnlyList<ProductCombinationOption?>? Options { get; set; }

    public ProductCombinationDetails? Details { get; set; }
}

/// <summary>The variation block; <c>{}</c> on a product with no variations, so every field reads <c>null</c>.</summary>
public sealed class ProductVariation : Dto
{
    public IReadOnlyList<ProductVariationType?>? Types { get; set; }

    public IReadOnlyList<ProductCombination?>? AvailableCombinations { get; set; }
}

/// <summary>One category the product is filed under, as <c>categories</c> lists it; <c>parents</c> again.</summary>
public sealed class ProductCategory : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public bool? InTrash { get; set; }

    public bool? ParentInTrash { get; set; }

    public bool? Disposable { get; set; }
}

/// <summary>One image in the media block.</summary>
public sealed class ProductMediaImage : Dto
{
    public long? Id { get; set; }

    public string? Filename { get; set; }

    public long? Position { get; set; }

    public bool? Primary { get; set; }

    public string? AltText { get; set; }

    public string? Url { get; set; }

    public string? ThumbnailUrl { get; set; }
}

/// <summary>One video in the media block.</summary>
public sealed class ProductMediaVideo : Dto
{
    public long? Id { get; set; }

    public string? Filename { get; set; }

    public long? Position { get; set; }

    public string? Url { get; set; }

    public string? ThumbnailUrl { get; set; }
}

/// <summary>The media block. A write names files the product already holds, by <c>id</c>; another id is <c>400</c>.
/// <c>specification_file</c> was <c>null</c> on every product read, so it is declared <c>object</c>.</summary>
public sealed class ProductMedia : Dto
{
    public IReadOnlyList<ProductMediaImage?>? Images { get; set; }

    public IReadOnlyList<ProductMediaVideo?>? Videos { get; set; }

    public object? SpecificationFile { get; set; }
}

/// <summary>One day of the schedule; times are <c>HH:mm</c>.</summary>
public sealed class ProductWeekdayWindow : Dto
{
    public bool? Enabled { get; set; }

    public bool? FullDay { get; set; }

    public string? StartTime { get; set; }

    public string? EndTime { get; set; }
}

/// <summary>The seven days of the schedule, each always present.</summary>
public sealed class ProductScheduleWeekdays : Dto
{
    public ProductWeekdayWindow? Monday { get; set; }

    public ProductWeekdayWindow? Tuesday { get; set; }

    public ProductWeekdayWindow? Wednesday { get; set; }

    public ProductWeekdayWindow? Thursday { get; set; }

    public ProductWeekdayWindow? Friday { get; set; }

    public ProductWeekdayWindow? Saturday { get; set; }

    public ProductWeekdayWindow? Sunday { get; set; }
}

/// <summary>When the product is on sale. <c>type</c> is <c>schedule_type</c> (<c>date_range</c> or
/// <c>custom_schedule</c>), <c>preset</c> is <c>custom_schedule_option</c> (<c>all_days</c>, <c>weekdays</c>,
/// <c>business_hour</c>, <c>weekends</c> or <c>custom</c>).</summary>
public sealed class ProductSchedule : Dto
{
    public bool? Enabled { get; set; }

    public string? Type { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public string? Preset { get; set; }

    public ProductScheduleWeekdays? Weekdays { get; set; }
}

/// <summary><c>password_protected</c> and <c>password</c> again; a set password reads <c>***</c>.</summary>
public sealed class ProductPasswordProtection : Dto
{
    public bool? Enabled { get; set; }

    public string? Password { get; set; }
}

/// <summary>Availability in one block: <c>enabled</c> is <c>available</c>, <c>active</c> is
/// <c>administrative_status</c>, <c>who_can_view</c> is <c>available_for</c>. <c>restrict_for_selected_customers</c>
/// is read-only.</summary>
public sealed class ProductAvailableForPurchase : Dto
{
    public bool? Enabled { get; set; }

    public bool? Active { get; set; }

    public string? WhoCanView { get; set; }

    public ProductSelectedCustomers? SelectedCustomers { get; set; }

    public ProductSelectedCustomers? RestrictForSelectedCustomers { get; set; }

    public ProductSchedule? Schedule { get; set; }

    public ProductPasswordProtection? PasswordProtection { get; set; }
}

/// <summary>The availability block to write. While <c>enabled</c> is <c>false</c> the store answers <c>200</c> and
/// keeps none of <c>who_can_view</c>, <c>schedule</c> or <c>password_protection</c>.</summary>
public sealed class ProductAvailableForPurchaseInput : Dto
{
    public bool? Enabled { get; set; }

    public bool? Active { get; set; }

    public string? WhoCanView { get; set; }

    public ProductSelectedCustomersInput? SelectedCustomers { get; set; }

    public ProductSchedule? Schedule { get; set; }

    public ProductPasswordProtection? PasswordProtection { get; set; }
}

/// <summary>The sale price; <c>amount_type</c> is <c>on_sale_price_type</c>.</summary>
public sealed class ProductOnSale : Dto
{
    public bool? Enabled { get; set; }

    public double? DiscountedPrice { get; set; }

    public string? AmountType { get; set; }
}

/// <summary>Order in multiples of <c>multiple_of</c>.</summary>
public sealed class ProductQuantityMultiple : Dto
{
    public bool? Enabled { get; set; }

    public long? MultipleOf { get; set; }
}

/// <summary>The expect-to-pay price.</summary>
public sealed class ProductExpectToPay : Dto
{
    public bool? Enabled { get; set; }

    public double? Price { get; set; }
}

/// <summary>The advanced pricing switches.</summary>
public sealed class ProductAdvancedPricing : Dto
{
    public bool? Enabled { get; set; }

    public ProductQuantityMultiple? QuantityMultiple { get; set; }

    public ProductExpectToPay? ExpectToPay { get; set; }

    public bool? CallForPrice { get; set; }
}

/// <summary>Prices in one block; <c>rrp_price</c> is <c>compare_at_price</c>. On a create <c>base_price</c> here
/// stands in for the top-level one.</summary>
public sealed class ProductPricing : Dto
{
    public double? BasePrice { get; set; }

    public double? CostPrice { get; set; }

    public double? RrpPrice { get; set; }

    public ProductOnSale? OnSale { get; set; }

    public ProductAdvancedPricing? AdvancedPricing { get; set; }
}

/// <summary>Stock tracking; <c>suppliers</c> echoes the ids written.</summary>
public sealed class ProductInventoryTracking : Dto
{
    public bool? Enabled { get; set; }

    public long? AvailableStock { get; set; }

    public long? LowStockLevel { get; set; }

    public IReadOnlyList<object?>? Suppliers { get; set; }
}

/// <summary>The combined stock selling limit.</summary>
public sealed class ProductCombinedStockLimit : Dto
{
    public bool? Enabled { get; set; }

    public long? Units { get; set; }
}

/// <summary>Stock in one block; <c>preorder</c> is <c>preorder_enabled</c>.</summary>
public sealed class ProductInventory : Dto
{
    public ProductInventoryTracking? Tracking { get; set; }

    public ProductCombinedStockLimit? CombinedStockLimit { get; set; }

    public bool? Preorder { get; set; }
}

/// <summary>The order quantity limits; a <c>max_quantity</c> of <c>0</c> is no maximum.</summary>
public sealed class ProductOrdering : Dto
{
    public long? MinQuantity { get; set; }

    public long? MaxQuantity { get; set; }
}

/// <summary>A price or purchase restriction. Only <c>mode</c> is written; <c>enabled</c> is read-only and
/// <c>customers</c> is answered <c>200</c> but kept empty.</summary>
public sealed class ProductRestriction : Dto
{
    public bool? Enabled { get; set; }

    public string? Mode { get; set; }

    public IReadOnlyList<object?>? Customers { get; set; }
}

/// <summary><c>restrict_price_for</c> and <c>restrict_purchase_for</c> in one block; <c>enabled</c> and
/// <c>display_only</c> are read-only.</summary>
public sealed class ProductRestrictions : Dto
{
    public bool? Enabled { get; set; }

    public ProductRestriction? Price { get; set; }

    public ProductRestriction? Purchase { get; set; }

    public bool? DisplayOnly { get; set; }
}

/// <summary>Whether <c>base_price</c> includes tax, and how it is rounded.</summary>
public sealed class ProductPriceIncludesTax : Dto
{
    public bool? Enabled { get; set; }

    public string? BasePriceRounding { get; set; }
}

/// <summary><c>tax_settings</c> in one block; <c>enabled</c> is <c>overwrite_default_config</c>. A write takes
/// <c>custom_tax_profile</c> as an id or <c>{"id": ...}</c> of a custom tax profile.</summary>
public sealed class ProductTax : Dto
{
    public bool? Enabled { get; set; }

    public bool? Exempt { get; set; }

    public ProductNamedReference? CustomTaxProfile { get; set; }

    public string? Message { get; set; }

    public bool? ShowPriceWithTax { get; set; }

    public ProductPriceIncludesTax? PriceIncludesTax { get; set; }
}

/// <summary>The shipping profile; <c>enabled</c> is read-only and true while a profile is set. A create drops
/// <c>profile</c>; an update takes it as an id or <c>{"id": ...}</c>.</summary>
public sealed class ProductShipping : Dto
{
    public bool? Enabled { get; set; }

    public ProductNamedReference? Profile { get; set; }
}

/// <summary>API paths of the product and its sub-paths, read-only.</summary>
public sealed class ProductLinks : Dto
{
    public string? Self { get; set; }

    public string? Variations { get; set; }

    public string? Stock { get; set; }

    public string? Reviews { get; set; }
}

/// <summary>One product: the row read's body and every write's answer. <see cref="FieldMetadata"/> sits beside
/// <c>product</c> when the read asks for it.</summary>
public sealed class Product : Dto
{
    protected override string? EnvelopeKey => "product";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Sku { get; set; }

    public string? Status { get; set; }

    public ProductUrls? Urls { get; set; }

    public string? Heading { get; set; }

    public IReadOnlyList<ProductParent?>? Parents { get; set; }

    public ProductNamedReference? CreatedBy { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public string? TransactionNo { get; set; }

    public string? ProductType { get; set; }

    public string? CustomType { get; set; }

    public bool? IsVirtual { get; set; }

    public string? InvoiceNote { get; set; }

    public bool? Available { get; set; }

    public ProductNamedReference? ProductPage { get; set; }

    public object? ProductLayout { get; set; }

    public string? CustomClass { get; set; }

    public bool? AvailableOnDateRange { get; set; }

    public string? AvailableFromDate { get; set; }

    public string? AvailableToDate { get; set; }

    public string? AvailableFor { get; set; }

    public ProductSelectedCustomers? SelectedCustomers { get; set; }

    public ProductSelectedCustomers? RestrictForSelectedCustomers { get; set; }

    public bool? PasswordProtected { get; set; }

    /// <summary><c>***</c> when one is set and <c>""</c> otherwise.</summary>
    public string? Password { get; set; }

    public ProductAvailableOnWeekdays? AvailableOnWeekdays { get; set; }

    public double? BasePrice { get; set; }

    public double? CostPrice { get; set; }

    public double? CompareAtPrice { get; set; }

    public bool? IsOnSale { get; set; }

    public double? SalePrice { get; set; }

    public bool? SoldOut { get; set; }

    public double? Length { get; set; }

    public double? Width { get; set; }

    public double? Height { get; set; }

    public double? Weight { get; set; }

    public string? SoldOutLabel { get; set; }

    public bool? IsFeatured { get; set; }

    public string? FeaturedLabel { get; set; }

    public bool? IsNew { get; set; }

    public bool? Featured { get; set; }

    public bool? New { get; set; }

    public string? Model { get; set; }

    public ProductProperties? ProductProperties { get; set; }

    public long? Version { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedOn { get; set; }

    public ProductNamedReference? LastUpdatedBy { get; set; }

    public string? ScheduleType { get; set; }

    public string? CustomScheduleOption { get; set; }

    public ProductTaxSettings? TaxSettings { get; set; }

    public ProductVariationPriceDisplay? VariationPriceDisplay { get; set; }

    public bool? AdministrativeStatus { get; set; }

    public bool? IsDraft { get; set; }

    public string? ProductSummary { get; set; }

    public string? ProductDescription { get; set; }

    public ProductPricingAndStock? ProductPricingAndStock { get; set; }

    public ProductImageAndVideo? ImageAndVideo { get; set; }

    public ProductAdvanced? Advanced { get; set; }

    public IReadOnlyList<ProductLink?>? RelatedProducts { get; set; }

    public IReadOnlyList<ProductLink?>? SimilarProducts { get; set; }

    public ProductBundle? Bundle { get; set; }

    public IReadOnlyList<ProductSeoConfig?>? SeoConfigs { get; set; }

    public IReadOnlyList<ProductTag?>? Tags { get; set; }

    public IReadOnlyList<ProductCustomProperty?>? CustomProperties { get; set; }

    public IReadOnlyList<ProductReview?>? Reviews { get; set; }

    public ProductVariation? ProductVariation { get; set; }

    public bool? PreorderEnabled { get; set; }

    public bool? HasVariations { get; set; }

    // The block-shaped copy of the fields above; a write to either changes both.
    public string? ProductUrl { get; set; }

    public ProductNamedReference? UpdatedBy { get; set; }

    public bool? Virtual { get; set; }

    public string? Summary { get; set; }

    public string? Description { get; set; }

    public IReadOnlyList<ProductCategory?>? Categories { get; set; }

    public IReadOnlyList<ProductLink?>? UpSellProducts { get; set; }

    public ProductMedia? Media { get; set; }

    public ProductAvailableForPurchase? AvailableForPurchase { get; set; }

    public ProductPricing? Pricing { get; set; }

    public ProductInventory? Inventory { get; set; }

    public ProductOrdering? Ordering { get; set; }

    public ProductRestrictions? Restrictions { get; set; }

    public ProductTax? Tax { get; set; }

    public ProductShipping? Shipping { get; set; }

    public ProductLinks? Links { get; set; }

    /// <summary><c>object</c>, the schema's entries differing in width.</summary>
    public object? FieldMetadata { get; set; }
}

/// <summary>The five-key <c>pagination</c> block; <c>limit</c> is the page size applied.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the listing: full product records, where they sit and any schema.</summary>
public sealed class ProductPage : Dto
{
    public IReadOnlyList<Product?>? Products { get; set; }

    public Pagination? Pagination { get; set; }

    /// <summary><c>object</c>, the schema's entries differing in width.</summary>
    public object? FieldMetadata { get; set; }
}

/// <summary>The create's, <c>PUT</c>'s and <c>PATCH</c>'s bare record; an update merges it at every level, and a
/// <c>PUT</c> also needs the ten fields <c>UpdateProductAsync</c> lists. A changed read-only field is
/// <c>400 read_only_field</c>; <c>product_url</c>, <c>links</c> and <c>restrict_for_selected_customers</c> are
/// read-only, and <c>updated_by</c> is answered <c>200</c> and ignored.</summary>
public sealed class ProductInput : Dto
{
    public string? Name { get; set; }

    public string? Sku { get; set; }

    /// <summary>Stored on a create; an update stores it only on a product without one, else drops it.</summary>
    public string? TransactionNo { get; set; }

    public string? Heading { get; set; }

    public string? ProductType { get; set; }

    public string? CustomType { get; set; }

    public bool? IsVirtual { get; set; }

    public string? InvoiceNote { get; set; }

    public bool? Available { get; set; }

    public bool? AvailableOnDateRange { get; set; }

    public string? AvailableFromDate { get; set; }

    public string? AvailableToDate { get; set; }

    public string? AvailableFor { get; set; }

    public ProductSelectedCustomersInput? SelectedCustomers { get; set; }

    public bool? PasswordProtected { get; set; }

    public string? Password { get; set; }

    public ProductAvailableOnWeekdays? AvailableOnWeekdays { get; set; }

    public double? BasePrice { get; set; }

    public double? CostPrice { get; set; }

    public double? CompareAtPrice { get; set; }

    public bool? IsOnSale { get; set; }

    public double? SalePrice { get; set; }

    public bool? IsFeatured { get; set; }

    public string? FeaturedLabel { get; set; }

    public bool? IsNew { get; set; }

    public ProductProperties? ProductProperties { get; set; }

    public string? ScheduleType { get; set; }

    public string? CustomScheduleOption { get; set; }

    public ProductTaxSettings? TaxSettings { get; set; }

    public ProductVariationPriceDisplay? VariationPriceDisplay { get; set; }

    public bool? AdministrativeStatus { get; set; }

    public string? ProductSummary { get; set; }

    public string? ProductDescription { get; set; }

    public ProductPricingAndStock? ProductPricingAndStock { get; set; }

    public ProductImageAndVideo? ImageAndVideo { get; set; }

    public ProductAdvanced? Advanced { get; set; }

    /// <summary>Category ids.</summary>
    public IReadOnlyList<long?>? Parents { get; set; }

    public IReadOnlyList<object?>? RelatedProducts { get; set; }

    public IReadOnlyList<object?>? SimilarProducts { get; set; }

    public IReadOnlyList<object?>? Tags { get; set; }

    public bool? PreorderEnabled { get; set; }

    public bool? Virtual { get; set; }

    public string? Summary { get; set; }

    public string? Description { get; set; }

    /// <summary>Category ids or <c>{"id": ...}</c> records; <c>[]</c> is answered <c>200</c> and changes nothing.</summary>
    public IReadOnlyList<long?>? Categories { get; set; }

    public IReadOnlyList<ProductLink?>? UpSellProducts { get; set; }

    public ProductMedia? Media { get; set; }

    public ProductAvailableForPurchaseInput? AvailableForPurchase { get; set; }

    public ProductPricing? Pricing { get; set; }

    public ProductInventory? Inventory { get; set; }

    public ProductOrdering? Ordering { get; set; }

    public ProductRestrictions? Restrictions { get; set; }

    public ProductTax? Tax { get; set; }

    public ProductShipping? Shipping { get; set; }
}

/// <summary>The review import's answer: every review the product holds, the ones just added last.</summary>
public sealed class ProductReviews : Dto
{
    protected override string? EnvelopeKey => "product";

    public IReadOnlyList<ProductReview?>? Reviews { get; set; }
}

// Not a Dto: the Dto name rule cannot spell dateCreated.
/// <summary>One review to add. <c>name</c>, <c>email</c> and a <c>rating</c> of 1 to 5 are required; a second review
/// under an <c>email</c> the product already has overwrites that review, keeping its <c>id</c>.</summary>
public sealed class ProductReviewInput
{
    [JsonPropertyName("name"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    [JsonPropertyName("email"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Email { get; set; }

    [JsonPropertyName("rating"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Rating { get; set; }

    [JsonPropertyName("review"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Review { get; set; }

    /// <summary>The same text as <see cref="Review"/>, under the name the collection sends.</summary>
    [JsonPropertyName("comment"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Comment { get; set; }

    [JsonPropertyName("status"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Status { get; set; }

    /// <summary><c>yyyy-MM-ddTHH:mm:ss</c>; the read's <c>date</c> key is refused on the write.</summary>
    [JsonPropertyName("dateCreated"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DateCreated { get; set; }
}

/// <summary>The review import's body: <c>{"product": {"reviews": [...]}}</c> with <see cref="TransactionNo"/> beside
/// the envelope; <see cref="Reviews"/> must not be empty.</summary>
public sealed class ProductReviewsImportInput : Dto
{
    protected override string? EnvelopeKey => "product";

    protected override IReadOnlyList<string> TopLevel { get; } = new[] { "transaction_no" };

    public IReadOnlyList<ProductReviewInput?>? Reviews { get; set; }

    public string? TransactionNo { get; set; }
}

/// <summary>One variant's stock on a product with variations.</summary>
public sealed class ProductStockVariation : Dto
{
    public long? VariationId { get; set; }

    public long? ProductId { get; set; }

    public string? Name { get; set; }

    public string? Sku { get; set; }

    public long? AvailableStock { get; set; }

    public IReadOnlyList<ProductStockLocation?>? Locations { get; set; }
}

/// <summary>The stock sub-path's body, read and written. <see cref="AvailableStock"/> is the sum of the rows'
/// <c>stock</c>; <see cref="Variations"/> is there only on a product with variations.</summary>
public sealed class ProductStock : Dto
{
    public string? Status { get; set; }

    public long? ProductId { get; set; }

    public bool? MultiLocationEnabled { get; set; }

    public bool? TrackInventory { get; set; }

    public long? AvailableStock { get; set; }

    public IReadOnlyList<ProductStockLocation?>? Locations { get; set; }

    public IReadOnlyList<ProductStockVariation?>? Variations { get; set; }
}

/// <summary>One row of the stock write. An omitted <see cref="LowStockLevel"/> is stored <c>null</c>; with no row
/// marked <see cref="IsDefault"/>, the first row sent becomes the default.</summary>
public sealed class ProductStockLocationInput : Dto
{
    public long? Warehouse { get; set; }

    public long? Stock { get; set; }

    public long? LowStockLevel { get; set; }

    public bool? IsDefault { get; set; }
}

/// <summary>The stock write's body: <see cref="Locations"/> is required and replaces the product's whole set, so a
/// location left out is unassigned whatever it holds.</summary>
public sealed class ProductStockInput : Dto
{
    public string? Note { get; set; }

    public IReadOnlyList<ProductStockLocationInput?>? Locations { get; set; }
}
