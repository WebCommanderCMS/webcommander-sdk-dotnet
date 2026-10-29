using System.Text.Json.Serialization;

namespace WebCommander.Resources.Variations.VariationsDto;

/// <summary>An image option's stored picture; <c>null</c> on text and color options.</summary>
public sealed class VariationOptionImage : Dto
{
    public string? Url { get; set; }

    public string? FileName { get; set; }
}

/// <summary>One option of a type. <see cref="Value"/> is <c>null</c> on an image option.</summary>
public sealed class VariationOption : Dto
{
    public long? Id { get; set; }

    public string? Label { get; set; }

    public string? Value { get; set; }

    public long? Order { get; set; }

    /// <summary>Held by one option at most; setting it on one clears the option that held it.</summary>
    public bool? Default { get; set; }

    public VariationOptionImage? Image { get; set; }
}

/// <summary>One variation type: a listing row, a row read and every type write's answer.</summary>
public sealed class Variation : Dto
{
    protected override string? EnvelopeKey => "variation";

    public long? Id { get; set; }

    /// <summary>2 to 100 characters, trimmed, and unique without regard to case.</summary>
    public string? Name { get; set; }

    /// <summary><c>text</c>, <c>color</c> or <c>image</c>, stored lower-case and fixed once created.</summary>
    public string? Standard { get; set; }

    public bool? Disposable { get; set; }

    public IReadOnlyList<VariationOption?>? Options { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary>The listing's five-key <c>pagination</c> block.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the listing, and the schema when <c>field_metadata</c> is asked for.</summary>
public sealed class VariationPage : Dto
{
    public IReadOnlyList<Variation?>? Variations { get; set; }

    public object? FieldMetadata { get; set; }

    public Pagination? Pagination { get; set; }
}

// Not a Dto: the Dto name rule cannot spell base64.
/// <summary>An image option's upload, read back as <see cref="VariationOptionImage"/>.</summary>
public sealed class VariationOptionImageInput
{
    [JsonPropertyName("base64"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Base64 { get; set; }

    /// <summary>Written as <c>filename</c>, read back as <c>file_name</c>.</summary>
    [JsonPropertyName("filename"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Filename { get; set; }
}

/// <summary>One option in a type write; an <see cref="Id"/> names an option of this type to change.</summary>
public sealed class VariationOptionInput : Dto
{
    public long? Id { get; set; }

    public string? Label { get; set; }

    public string? Value { get; set; }

    public long? Order { get; set; }

    public bool? Default { get; set; }

    public VariationOptionImageInput? Image { get; set; }
}

/// <summary>The type writes' body, <c>{"variation": {...}}</c>; <see cref="Standard"/> cannot change after the create.</summary>
public sealed class VariationInput : Dto
{
    protected override string? EnvelopeKey => "variation";

    public string? Name { get; set; }

    public string? Standard { get; set; }

    public IReadOnlyList<VariationOptionInput?>? Options { get; set; }
}

/// <summary>A variant's expected-price display.</summary>
public sealed class ProductVariantExpectToPay : Dto
{
    public bool? IsEnabled { get; set; }

    public double? Price { get; set; }
}

/// <summary>A variant's prices; the bulk write answers the first four fields.</summary>
public sealed class ProductVariantPricing : Dto
{
    public double? BasePrice { get; set; }

    public double? CostPrice { get; set; }

    public double? SalePrice { get; set; }

    public bool? IsOnSale { get; set; }

    public string? SaleAmountType { get; set; }

    public bool? HidePrice { get; set; }

    public bool? IsCallForPrice { get; set; }

    public ProductVariantExpectToPay? ExpectToPay { get; set; }

    public object? TieredPricing { get; set; }
}

/// <summary>A variant's stock; the bulk write answers the first three fields.</summary>
public sealed class ProductVariantInventory : Dto
{
    public bool? IsEnabled { get; set; }

    public long? Quantity { get; set; }

    public long? LowStockThreshold { get; set; }

    public bool? IsSoldOut { get; set; }

    public string? SoldOutLabel { get; set; }
}

/// <summary>A variant's order quantities; a <see cref="MaxQuantity"/> of 0 is no maximum.</summary>
public sealed class ProductVariantOrdering : Dto
{
    public long? MinQuantity { get; set; }

    public long? MaxQuantity { get; set; }

    public long? MultipleOf { get; set; }

    public bool? IsMultipleQuantityRequired { get; set; }
}

/// <summary>A variant's size and weight. <see cref="Weight"/> arrives as a number or as text, so it is declared
/// <c>object</c>.</summary>
public sealed class ProductVariantDimensions : Dto
{
    public double? Length { get; set; }

    public double? Width { get; set; }

    public double? Height { get; set; }

    public object? Weight { get; set; }
}

/// <summary>A variant's two badges.</summary>
public sealed class ProductVariantBadges : Dto
{
    public bool? IsFeatured { get; set; }

    public bool? IsNewArrival { get; set; }
}

/// <summary>One variant of a product; <see cref="Key"/> joins its option labels with <c>/</c>.</summary>
public sealed class ProductVariant : Dto
{
    public long? VariationId { get; set; }

    public bool? Available { get; set; }

    public bool? IsBase { get; set; }

    public string? Key { get; set; }

    public string? Name { get; set; }

    public string? Sku { get; set; }

    public string? CustomSku { get; set; }

    public string? Title { get; set; }

    public string? Summary { get; set; }

    public string? Description { get; set; }

    public string? Barcode { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string? HsCode { get; set; }

    public ProductVariantPricing? Pricing { get; set; }

    public ProductVariantInventory? Inventory { get; set; }

    public ProductVariantOrdering? Ordering { get; set; }

    public ProductVariantDimensions? Dimensions { get; set; }

    public ProductVariantBadges? Badges { get; set; }

    public IReadOnlyList<object?>? Images { get; set; }
}

/// <summary>A product's variants, under <see cref="ProductKey"/>: its type names joined with <c>/</c>.</summary>
public sealed class ProductVariationGroup : Dto
{
    public string? ProductKey { get; set; }

    public IReadOnlyList<ProductVariant?>? Variants { get; set; }
}

/// <summary>The product read's body; <see cref="Variations"/> is empty for a product without variants.</summary>
public sealed class ProductVariations : Dto
{
    public IReadOnlyList<ProductVariationGroup?>? Variations { get; set; }
}

/// <summary>One option a variant carries, in the bulk write's full answer.</summary>
public sealed class ProductVariantOptionReference : Dto
{
    public long? Id { get; set; }

    public string? Type { get; set; }

    public string? Label { get; set; }
}

/// <summary>One written variant: <see cref="Id"/> and <see cref="Name"/>, and the rest with <c>response=full</c>.</summary>
public sealed class ProductVariantUpdate : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Sku { get; set; }

    public string? CustomSku { get; set; }

    public string? Barcode { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string? HsCode { get; set; }

    public bool? Available { get; set; }

    public IReadOnlyList<ProductVariantOptionReference?>? Options { get; set; }

    public ProductVariantPricing? Pricing { get; set; }

    public ProductVariantInventory? Inventory { get; set; }

    public ProductVariantOrdering? Ordering { get; set; }

    public string? TransactionNo { get; set; }

    public string? Url { get; set; }
}

/// <summary>The bulk write's answer; <see cref="Failed"/> stays empty, a refused id refusing the whole write.</summary>
public sealed class ProductVariationsBulkResult : Dto
{
    public string? Status { get; set; }

    public string? Message { get; set; }

    public IReadOnlyList<ProductVariantUpdate?>? Updated { get; set; }

    public IReadOnlyList<object?>? Failed { get; set; }
}

/// <summary>The bulk write's body, no envelope: the variant ids and at least one of the six fields.</summary>
public sealed class ProductVariationsBulkInput : Dto
{
    public IReadOnlyList<long?>? VariationIds { get; set; }

    public bool? Available { get; set; }

    public double? BasePrice { get; set; }

    public double? CostPrice { get; set; }

    public long? AvailableStock { get; set; }

    public long? MinOrderQuantity { get; set; }

    public long? MaxOrderQuantity { get; set; }
}

/// <summary>One variant to import; <c>key</c> joins one option label per type with <c>/</c> and is required. A key
/// the product has is that variant, changed; an unknown field is <c>400 unknown_field</c>.</summary>
public sealed class ProductVariantInput : Dto
{
    public string? Key { get; set; }

    public string? Name { get; set; }

    public string? Sku { get; set; }

    public string? CustomSku { get; set; }

    public string? Title { get; set; }

    public string? Summary { get; set; }

    public string? Description { get; set; }

    public string? Barcode { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string? HsCode { get; set; }

    public ProductVariantPricing? Pricing { get; set; }

    public ProductVariantInventory? Inventory { get; set; }

    public ProductVariantOrdering? Ordering { get; set; }

    public ProductVariantDimensions? Dimensions { get; set; }

    public ProductVariantBadges? Badges { get; set; }

    public IReadOnlyList<object?>? Images { get; set; }
}

/// <summary>One group to import: <c>product_key</c> names the types, joined with <c>/</c>, and <c>variants</c> must
/// not be empty. A type name the store has is reused, an option label it lacks is added.</summary>
public sealed class ProductVariationGroupInput : Dto
{
    public string? ProductKey { get; set; }

    public IReadOnlyList<ProductVariantInput?>? Variants { get; set; }
}

/// <summary>The import's body, no envelope; <c>variations</c> must not be empty.</summary>
public sealed class ProductVariationsImportInput : Dto
{
    public string? TransactionNo { get; set; }

    public IReadOnlyList<ProductVariationGroupInput?>? Variations { get; set; }
}

/// <summary>The import's answer: <c>status</c>, a <c>message</c> counting the variants and <c>total_variants</c>.</summary>
public sealed class ProductVariationsImportResult : Dto
{
    public string? Status { get; set; }

    public string? Message { get; set; }

    public long? TotalVariants { get; set; }
}
