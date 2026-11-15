using WebCommander.Resources.Orders.OrdersDto;

using Reference = WebCommander.Resources.Carts.CartsDto.Reference;

namespace WebCommander.Resources.Carts.AdminCartsDto;

/// <summary>The session block. <see cref="SessionId"/> starts <c>PRV-</c> and is the <c>sessionId</c> of every other call.
/// <see cref="ExpiresAt"/> is 30 minutes after the last write.</summary>
public sealed class AdminCartSession : Dto
{
    public string? SessionId { get; set; }

    public string? Status { get; set; }

    public string? CreatedAt { get; set; }

    public string? ExpiresAt { get; set; }
}

/// <summary>The customer the cart is for; <see cref="CustomerId"/> is the customer's normal id.</summary>
public sealed class AdminCartCustomer : Dto
{
    public long? CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }
}

/// <summary>One line, priced by the store. <see cref="CartItemId"/> is what the line's update and delete take; a replace of
/// the cart numbers its lines again.</summary>
public sealed class AdminCartItem : Dto
{
    public long? CartItemId { get; set; }

    public long? ProductId { get; set; }

    public string? ProductName { get; set; }

    public long? VariationId { get; set; }

    public string? VariationName { get; set; }

    public string? Sku { get; set; }

    public double? Price { get; set; }

    public long? Quantity { get; set; }

    public double? Discount { get; set; }

    public double? Tax { get; set; }

    public double? TotalAmount { get; set; }

    public bool? Taxable { get; set; }

    public bool? Shippable { get; set; }
}

/// <summary>The cart's money before shipping.</summary>
public sealed class AdminCartTotals : Dto
{
    public double? SubTotal { get; set; }

    public double? TotalDiscount { get; set; }

    public double? TotalTax { get; set; }

    public double? GrandTotal { get; set; }
}

/// <summary>A cart: what every cart call but the deletes answers.</summary>
public sealed class AdminCart : Dto
{
    protected override string? EnvelopeKey => "cart";

    public AdminCartSession? Session { get; set; }

    public AdminCartCustomer? Customer { get; set; }

    public string? CurrencyCode { get; set; }

    public IReadOnlyList<AdminCartItem?>? Items { get; set; }

    public AdminCartTotals? Totals { get; set; }
}

/// <summary>A billing or shipping address as the checkout reads it back; <see cref="State"/> is <c>{}</c> when the address
/// has none.</summary>
public sealed class CheckoutAddress : Dto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? CompanyName { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public Reference? Country { get; set; }

    public Reference? State { get; set; }

    public string? City { get; set; }

    public string? PostCode { get; set; }

    public string? Mobile { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }
}

/// <summary>The delivery the checkout was priced for; empty until a <c>delivery_type</c> is set.</summary>
public sealed class CheckoutShipping : Dto
{
    public string? DeliveryType { get; set; }

    public string? ShippingType { get; set; }

    public double? ShippingCost { get; set; }

    public double? ShippingTax { get; set; }
}

/// <summary>The coupon applied: <see cref="AppliedAmount"/> is the discount it takes off.</summary>
public sealed class CheckoutCoupon : Dto
{
    public string? Code { get; set; }

    public string? Status { get; set; }

    public double? AppliedAmount { get; set; }
}

/// <summary>The gift card applied: <see cref="AppliedAmount"/> is the lesser of its balance and the grand total.</summary>
public sealed class CheckoutGiftCard : Dto
{
    public string? Code { get; set; }

    public string? Status { get; set; }

    public double? AppliedAmount { get; set; }

    public double? RemainingBalance { get; set; }
}

/// <summary>At most one coupon and one gift card; <c>null</c> for one not applied.</summary>
public sealed class CheckoutDiscounts : Dto
{
    public CheckoutCoupon? Coupon { get; set; }

    public CheckoutGiftCard? GiftCard { get; set; }
}

/// <summary>The money. A gift card lowers <see cref="Due"/>, not <see cref="GrandTotal"/>.</summary>
public sealed class CheckoutTotals : Dto
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

/// <summary>One thing still blocking a confirm, such as <c>delivery_type</c> <c>required</c>.</summary>
public sealed class CheckoutIssue : Dto
{
    public string? Field { get; set; }

    public string? Code { get; set; }

    public string? Message { get; set; }
}

/// <summary>The checkout. The read and the update answer every field; the coupon, gift card and delivery schedule calls
/// answer the parts they change. <see cref="DeliverySchedule"/> is <c>null</c> until one is set.</summary>
public sealed class AdminCheckout : Dto
{
    protected override string? EnvelopeKey => "checkout";

    public AdminCartSession? Session { get; set; }

    public AdminCartCustomer? Customer { get; set; }

    public string? DeliveryType { get; set; }

    public string? CurrencyCode { get; set; }

    public IReadOnlyList<AdminCartItem?>? Items { get; set; }

    public CheckoutAddress? BillingAddress { get; set; }

    public CheckoutAddress? ShippingAddress { get; set; }

    public CheckoutShipping? Shipping { get; set; }

    public CheckoutDiscounts? Discounts { get; set; }

    public object? DeliverySchedule { get; set; }

    public CheckoutTotals? Totals { get; set; }

    public IReadOnlyList<CheckoutIssue?>? Issues { get; set; }
}

/// <summary>One product line to put on a cart. <see cref="ProductId"/> and <see cref="Quantity"/> are required;
/// <see cref="VariationId"/> picks the variation of a variable product.</summary>
public sealed class AdminCartProductInput : Dto
{
    public long? ProductId { get; set; }

    public long? VariationId { get; set; }

    public long? Quantity { get; set; }
}

/// <summary>The create and replace body, bare. <see cref="Products"/> is required, and so is <see cref="CustomerId"/> on a
/// replace; <see cref="SessionId"/> on a create names the session yourself, in the <c>PRV-</c> form. An unknown field is
/// <c>400</c> <c>unknown_field</c>.</summary>
public sealed class AdminCartInput : Dto
{
    public long? CustomerId { get; set; }

    public string? SessionId { get; set; }

    public IReadOnlyList<AdminCartProductInput?>? Products { get; set; }
}

/// <summary>The cart update body: the customer to move the cart to. <see cref="CustomerId"/> is required.</summary>
public sealed class AdminCartCustomerInput : Dto
{
    public long? CustomerId { get; set; }
}

/// <summary>The add-lines body. Adding a product already on the cart raises that line's quantity.</summary>
public sealed class AdminCartItemsInput : Dto
{
    public IReadOnlyList<AdminCartProductInput?>? Products { get; set; }
}

/// <summary>The line update body: the new <see cref="Quantity"/>, required.</summary>
public sealed class AdminCartItemUpdateInput : Dto
{
    public long? Quantity { get; set; }
}

/// <summary>The checkout update body. <see cref="DeliveryType"/> is <c>shipping</c>, <c>delivery</c>,
/// <c>others_shipping</c> or <c>store_pickup</c>; an address replaces the whole one.</summary>
public sealed class AdminCheckoutInput : Dto
{
    public string? DeliveryType { get; set; }

    public OrderAddressInput? BillingAddress { get; set; }

    public OrderAddressInput? ShippingAddress { get; set; }
}

/// <summary>The body of a coupon or gift card call: <see cref="Code"/>, required.</summary>
public sealed class AdminCheckoutCodeInput : Dto
{
    public string? Code { get; set; }
}

/// <summary>The payment a confirm records. <see cref="Amount"/> and <see cref="PaymentMethod"/> are required; an
/// <see cref="Amount"/> above what is due is refused.</summary>
public sealed class AdminCheckoutPaymentInput : Dto
{
    public double? Amount { get; set; }

    public string? PaymentStatus { get; set; }

    public string? PaymentMethod { get; set; }

    public string? TransactionReference { get; set; }

    public string? TransactionDate { get; set; }
}

/// <summary>The confirm body, bare, and optional. <see cref="PaymentGateway"/> takes one of the store's codes, such as
/// <c>PIS</c>; an unknown field is <c>400</c>.</summary>
public sealed class AdminCheckoutConfirmInput : Dto
{
    public string? PaymentGateway { get; set; }

    public string? TransactionNo { get; set; }

    public bool? SendEmail { get; set; }

    public AdminCheckoutPaymentInput? Payment { get; set; }
}

/// <summary>The delivery schedule to choose: its <see cref="Id"/> and the <see cref="Date"/> wanted, both required.</summary>
public sealed class AdminCheckoutDeliveryScheduleInput : Dto
{
    public long? Id { get; set; }

    public string? Date { get; set; }
}
