using WebCommander.Resources.Orders.OrdersDto;

namespace WebCommander.Resources.OrderPreviews.OrderPreviewsDto;

/// <summary>The session block. <see cref="SessionId"/> starts <c>PRV-</c> and is the <c>previewSessionId</c> of every
/// other call. <see cref="ExpiresAt"/> is 30 minutes after <see cref="CreatedAt"/>; a coupon or gift card call,
/// applying or removing, moves it 30 minutes past that call, and a read does not.</summary>
public sealed class PreviewSession : Dto
{
    public string? SessionId { get; set; }

    public string? Status { get; set; }

    public string? CreatedAt { get; set; }

    public string? ExpiresAt { get; set; }
}

/// <summary>The customer as the preview shows it: the billing address names win over the record's.</summary>
public sealed class PreviewCustomer : Dto
{
    public long? CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }
}

/// <summary>One line of the preview, priced by the store.</summary>
public sealed class PreviewItem : Dto
{
    public long? ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? Sku { get; set; }

    public long? Quantity { get; set; }

    public double? Price { get; set; }

    public double? Discount { get; set; }

    public double? Tax { get; set; }

    public double? TotalAmount { get; set; }

    public bool? Taxable { get; set; }

    public bool? Shippable { get; set; }
}

/// <summary>The delivery the preview was priced for. A <c>store_pickup</c> has no shipping cost.</summary>
public sealed class PreviewShipping : Dto
{
    public string? DeliveryType { get; set; }

    public string? ShippingType { get; set; }

    public double? ShippingCost { get; set; }

    public double? ShippingTax { get; set; }
}

/// <summary>The coupon on the preview: <see cref="Amount"/> is the discount it takes off, <see cref="Status"/>
/// <c>applied</c>.</summary>
public sealed class PreviewCoupon : Dto
{
    public string? Code { get; set; }

    public string? Status { get; set; }

    public double? Amount { get; set; }
}

/// <summary>The gift card on the preview: <see cref="AppliedAmount"/> is the lesser of its balance and the grand total.
/// <see cref="RemainingBalance"/> came back <c>null</c>.</summary>
public sealed class PreviewGiftCard : Dto
{
    public string? Code { get; set; }

    public string? Status { get; set; }

    public double? AppliedAmount { get; set; }

    public double? RemainingBalance { get; set; }
}

/// <summary>At most one coupon and one gift card; <c>null</c> for one not applied.</summary>
public sealed class PreviewDiscounts : Dto
{
    public PreviewCoupon? Coupon { get; set; }

    public PreviewGiftCard? GiftCard { get; set; }
}

/// <summary>The money. <see cref="GrandTotal"/> is <see cref="SubTotal"/> less <see cref="TotalDiscount"/> plus the tax,
/// shipping (and its tax), handling and surcharge; a gift card lowers <see cref="Due"/>, not
/// <see cref="GrandTotal"/>.</summary>
public sealed class PreviewTotals : Dto
{
    public double? SubTotal { get; set; }

    public double? TotalDiscount { get; set; }

    public double? TotalTax { get; set; }

    public double? ShippingCost { get; set; }

    public double? HandlingCost { get; set; }

    public double? TotalSurcharge { get; set; }

    public double? GiftCardApplied { get; set; }

    public double? GrandTotal { get; set; }

    public double? Due { get; set; }
}

/// <summary>A preview. The start and the read answer every field; a coupon or gift card call answers only
/// <see cref="Session"/>, <see cref="Discounts"/> and <see cref="Totals"/>.</summary>
public sealed class OrderPreview : Dto
{
    protected override string? EnvelopeKey => "order";

    public PreviewSession? Session { get; set; }

    public PreviewCustomer? Customer { get; set; }

    public string? DeliveryType { get; set; }

    public string? CurrencyCode { get; set; }

    public IReadOnlyList<PreviewItem?>? Items { get; set; }

    public PreviewShipping? Shipping { get; set; }

    public PreviewDiscounts? Discounts { get; set; }

    public PreviewTotals? Totals { get; set; }
}

/// <summary>The start body, bare. <see cref="Products"/> is required. Without <see cref="CustomerId"/> the billing
/// address is required, and a shipping address too unless <see cref="DeliveryType"/> is <c>store_pickup</c>.
/// <see cref="DeliveryType"/> is <c>shipping</c> unless it is <c>delivery</c>, <c>others_shipping</c> or
/// <c>store_pickup</c>. An unknown field, such as <c>coupon_code</c> or a product line's <c>price</c>, is
/// <c>400</c>.</summary>
public sealed class OrderPreviewInput : Dto
{
    public long? CustomerId { get; set; }

    public string? DeliveryType { get; set; }

    public OrderAddressInput? BillingAddress { get; set; }

    public OrderAddressInput? ShippingAddress { get; set; }

    public IReadOnlyList<OrderProductInput?>? Products { get; set; }
}

/// <summary>The body of a coupon or gift card call: <see cref="Code"/>, required, at most 100 characters.</summary>
public sealed class OrderPreviewCodeInput : Dto
{
    public string? Code { get; set; }
}

/// <summary>The payment a confirm records. <see cref="Amount"/> above what is due is <c>422</c>, a missing
/// <see cref="PaymentMethod"/> is <c>400</c>, and <see cref="PaymentStatus"/> <c>failed</c> leaves the order
/// unpaid.</summary>
public sealed class OrderPreviewPaymentInput : Dto
{
    public double? Amount { get; set; }

    public string? PaymentStatus { get; set; }

    public string? PaymentMethod { get; set; }

    public string? TransactionReference { get; set; }

    public string? TransactionDate { get; set; }
}

/// <summary>The confirm body, bare, and optional. <see cref="PaymentGateway"/> takes a code such as <c>PIS</c> and leaves
/// the order unpaid; an unknown field is <c>400</c>.</summary>
public sealed class OrderPreviewConfirmInput : Dto
{
    public string? PaymentGateway { get; set; }

    public string? TransactionNo { get; set; }

    public bool? SendEmail { get; set; }

    public OrderPreviewPaymentInput? Payment { get; set; }
}
