namespace WebCommander.Resources.DiscountProfiles.DiscountProfilesDto;

/// <summary>The block an <c>amount</c> profile carries.</summary>
public sealed class AmountDetails : Dto
{
    public string? Type { get; set; }

    public string? ApplyTo { get; set; }

    public double? SingleAmount { get; set; }

    public string? SingleAmountType { get; set; }

    public string? MinimumAmountOn { get; set; }

    /// <summary>Tier ids; tier records sent in a write are not stored.</summary>
    public IReadOnlyList<long?>? Tiers { get; set; }
}

/// <summary>The block a <c>shipping</c> profile carries.</summary>
public sealed class ShippingDetails : Dto
{
    public string? Type { get; set; }

    public string? ApplyTo { get; set; }

    public string? MinimumQtyOn { get; set; }

    public string? AmountType { get; set; }

    public string? SingleAmountType { get; set; }

    public long? MinimumQty { get; set; }

    public double? MinimumAmount { get; set; }

    public object? SingleAmount { get; set; }

    public object? CapAmount { get; set; }

    public object? MaximumTime { get; set; }

    public object? Zone { get; set; }

    public object? ShippingClass { get; set; }

    public IReadOnlyList<object?>? Tiers { get; set; }
}

/// <summary>The block a <c>product</c> profile carries.</summary>
public sealed class ProductDetails : Dto
{
    public string? Type { get; set; }

    public string? FreeProductMinQtyOn { get; set; }

    public long? FreeProductMaxQty { get; set; }

    public double? FreeProductMinAmount { get; set; }

    public string? MinimumQtyOn { get; set; }

    public string? AmountType { get; set; }

    public string? SingleAmountType { get; set; }

    public object? SingleAmount { get; set; }

    public object? CapPrice { get; set; }

    public object? CapPriceMaxQty { get; set; }

    /// <summary>Read back <c>[]</c> whatever a write sent.</summary>
    public IReadOnlyList<object?>? ProductIds { get; set; }

    public IReadOnlyList<object?>? CategoryIds { get; set; }

    public IReadOnlyList<object?>? Tiers { get; set; }
}

/// <summary>One generated coupon; <see cref="CustomerEmail"/> is <c>""</c> until a customer is assigned.</summary>
public sealed class CustomerCoupon : Dto
{
    public long? Id { get; set; }

    public string? Code { get; set; }

    public string? CustomerEmail { get; set; }
}

/// <summary>One discount profile: a listing row, the row path's body and every write's answer, carrying the one details block its kind names.</summary>
public sealed class DiscountProfile : Dto
{
    protected override string? EnvelopeKey => "discount_profile";

    public long? Id { get; set; }

    public string? Uuid { get; set; }

    public string? Name { get; set; }

    /// <summary><c>""</c> when a create leaves it out.</summary>
    public string? Type { get; set; }

    public string? DiscountDetailsType { get; set; }

    public AmountDetails? AmountDetails { get; set; }

    public ShippingDetails? ShippingDetails { get; set; }

    public ProductDetails? ProductDetails { get; set; }

    public bool? IsActive { get; set; }

    /// <summary>Read back as <c>yyyy-MM-ddTHH:mm:ss</c> in UTC with no zone; <c>null</c> when unset.</summary>
    public string? StartDate { get; set; }

    public bool? IsSpecifyEndDate { get; set; }

    /// <summary>Read back as <see cref="StartDate"/> is, and <c>""</c> when unset.</summary>
    public string? EndDate { get; set; }

    public bool? IsApplyCouponCode { get; set; }

    public string? DefaultCouponCode { get; set; }

    public bool? IsCouponCodeAutoGenerate { get; set; }

    public bool? IsCreateUniqueCouponEachCustomer { get; set; }

    public bool? IsImportedCoupon { get; set; }

    public bool? IsMaximumUseTotal { get; set; }

    public long? MaximumUseCount { get; set; }

    public bool? IsMaximumUseCustomer { get; set; }

    public long? MaximumUseCustomerCount { get; set; }

    public bool? IsMaximumDiscountAllowed { get; set; }

    public double? MaximumDiscountAllowedAmount { get; set; }

    public bool? IsExcludeProductsOnSale { get; set; }

    public bool? IsDiscountUsedWithOtherDiscount { get; set; }

    public bool? IsDisplayDiscountInformationProdDetail { get; set; }

    public bool? IsDisplayTextCoupon { get; set; }

    public string? DisplayTextCoupon { get; set; }

    public bool? IsDisplayTextCart { get; set; }

    public string? DisplayTextCart { get; set; }

    public bool? IsDisplayTextPartialDiscountCondition { get; set; }

    public string? DisplayTextPartialDiscountCondition { get; set; }

    public string? InvoiceNote { get; set; }

    public IReadOnlyList<long?>? ExcludeProducts { get; set; }

    public IReadOnlyList<CustomerCoupon?>? CustomerCoupons { get; set; }

    public IReadOnlyList<long?>? Usage { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedOn { get; set; }

    /// <summary><c>object</c>, the schema's entries differing in width; present only when asked for.</summary>
    public object? FieldMetadata { get; set; }
}

/// <summary>The writable form of <see cref="DiscountProfile"/>, with no envelope; the update resets every flag, date and limit it omits.</summary>
public sealed class DiscountProfileInput : Dto
{
    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? DiscountDetailsType { get; set; }

    public AmountDetails? AmountDetails { get; set; }

    public ShippingDetails? ShippingDetails { get; set; }

    public ProductDetails? ProductDetails { get; set; }

    public bool? IsActive { get; set; }

    /// <summary>Sent as <c>yyyy-MM-dd HH:mm:ss</c>, store-local.</summary>
    public string? StartDate { get; set; }

    public bool? IsSpecifyEndDate { get; set; }

    /// <summary>Sent as <see cref="StartDate"/> is, and dropped by a create without <see cref="IsSpecifyEndDate"/>.</summary>
    public string? EndDate { get; set; }

    public bool? IsApplyCouponCode { get; set; }

    public string? DefaultCouponCode { get; set; }

    public bool? IsCouponCodeAutoGenerate { get; set; }

    public bool? IsCreateUniqueCouponEachCustomer { get; set; }

    public bool? IsImportedCoupon { get; set; }

    public bool? IsMaximumUseTotal { get; set; }

    public long? MaximumUseCount { get; set; }

    public bool? IsMaximumUseCustomer { get; set; }

    public long? MaximumUseCustomerCount { get; set; }

    public bool? IsMaximumDiscountAllowed { get; set; }

    public double? MaximumDiscountAllowedAmount { get; set; }

    public bool? IsExcludeProductsOnSale { get; set; }

    public bool? IsDiscountUsedWithOtherDiscount { get; set; }

    public bool? IsDisplayDiscountInformationProdDetail { get; set; }

    public bool? IsDisplayTextCoupon { get; set; }

    public string? DisplayTextCoupon { get; set; }

    public bool? IsDisplayTextCart { get; set; }

    public string? DisplayTextCart { get; set; }

    public bool? IsDisplayTextPartialDiscountCondition { get; set; }

    public string? DisplayTextPartialDiscountCondition { get; set; }

    public string? InvoiceNote { get; set; }

    /// <summary>Real product ids only.</summary>
    public IReadOnlyList<long?>? ExcludeProducts { get; set; }
}

/// <summary>The listing's ten-key <c>pagination</c> block; <see cref="Limit"/> is the size applied, capped at 20.</summary>
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

/// <summary>The one <c>group_meta</c> entry: what the collection is, whether it is writable and where.</summary>
public sealed class ResourceMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The listing's <c>group_meta</c> block.</summary>
public sealed class GroupMeta : Dto
{
    public ResourceMeta? DiscountProfiles { get; set; }
}

/// <summary>One page of the listing: the rows, where they sit, the <c>group_meta</c> and the schema when it was asked for.</summary>
public sealed class DiscountProfilePage : Dto
{
    public IReadOnlyList<DiscountProfile?>? DiscountProfiles { get; set; }

    public Pagination? Pagination { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary>The create-coupons answer: the codes generated, beside <c>status</c> and <c>message</c>.</summary>
public sealed class DiscountProfileCoupons : Dto
{
    public string? Status { get; set; }

    public string? Message { get; set; }

    public IReadOnlyList<string?>? Codes { get; set; }
}

/// <summary>The create-coupons body; <see cref="RequestedCodes"/> is required and at least 1.</summary>
public sealed class DiscountProfileCouponsInput : Dto
{
    public long? RequestedCodes { get; set; }
}

/// <summary>The assign-customer body: a generated <see cref="Code"/> and the <see cref="Email"/> of an existing customer.</summary>
public sealed class DiscountCouponCustomerInput : Dto
{
    public string? Code { get; set; }

    public string? Email { get; set; }
}
