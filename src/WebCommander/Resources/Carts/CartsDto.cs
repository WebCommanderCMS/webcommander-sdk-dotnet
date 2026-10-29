using System.Text.Json.Serialization;

namespace WebCommander.Resources.Carts.CartsDto;

/// <summary>The cart's <c>totals</c>; <see cref="Subtotal"/> sums each line's <c>unit_price</c> times its quantity.</summary>
public sealed class CartTotals : Dto
{
    public double? Subtotal { get; set; }

    public double? Discount { get; set; }

    public double? ShippingDiscount { get; set; }

    public double? Tax { get; set; }

    public double? Total { get; set; }

    public double? GrandTotal { get; set; }

    public double? Paid { get; set; }
}

/// <summary>The cart's <c>shipping</c> block; its two taxes follow the shipping address's country.</summary>
public sealed class CartShipping : Dto
{
    public double? Cost { get; set; }

    public double? HandlingFee { get; set; }

    public double? HandlingTax { get; set; }

    public double? ShippingTax { get; set; }

    public double? TotalTax { get; set; }
}

/// <summary>One line's <c>pricing</c>.</summary>
public sealed class CartItemPricing : Dto
{
    public double? UnitPrice { get; set; }

    public double? DisplayUnitPrice { get; set; }

    public double? Total { get; set; }

    public double? Discount { get; set; }

    public double? DiscountTax { get; set; }

    public double? Tax { get; set; }
}

/// <summary>The product one line holds; <see cref="AvailableStock"/> is the product's, not the line's.</summary>
public sealed class CartItemProduct : Dto
{
    public long? Id { get; set; }

    public string? Sku { get; set; }

    public double? BasePrice { get; set; }

    public long? AvailableStock { get; set; }

    public bool? OnSale { get; set; }

    public double? CostPrice { get; set; }

    public double? SalePrice { get; set; }

    public string? Url { get; set; }

    public string? Name { get; set; }

    public string? ProductType { get; set; }

    public string? ImageLink { get; set; }
}

/// <summary>One line of the cart; <see cref="Id"/> is the <c>cart_item_id</c> the line's writes take.</summary>
public sealed class CartItem : Dto
{
    public long? Id { get; set; }

    public long? Quantity { get; set; }

    public object? Variations { get; set; }

    public bool? Taxable { get; set; }

    public bool? Shippable { get; set; }

    public CartItemPricing? Pricing { get; set; }

    public CartItemProduct? Product { get; set; }
}

/// <summary>The cart's <c>discount</c> block.</summary>
public sealed class CartDiscount : Dto
{
    public double? Discount { get; set; }

    public double? OrderDiscount { get; set; }

    public double? ShippingDiscount { get; set; }

    public double? DiscountOnTax { get; set; }

    public double? DiscountOnShippingTax { get; set; }
}

/// <summary>The <c>discount</c> entry of <c>messages</c>.</summary>
public sealed class CartDiscountMessage : Dto
{
    public string? Message { get; set; }

    public object? Status { get; set; }
}

/// <summary>The cart's <c>messages</c> block.</summary>
public sealed class CartMessages : Dto
{
    public CartDiscountMessage? Discount { get; set; }
}

/// <summary>The one entry of <c>group_meta</c>.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block, which only a read sends.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? Cart { get; set; }
}

/// <summary>One cart: the answer of the add, the read and the line update.</summary>
public sealed class Cart : Dto
{
    protected override string? EnvelopeKey => "cart";

    public long? CustomerId { get; set; }

    public string? SessionId { get; set; }

    public string? CurrencyCode { get; set; }

    public string? CurrencySymbol { get; set; }

    public string? DeliveryType { get; set; }

    public CartTotals? Totals { get; set; }

    public CartShipping? Shipping { get; set; }

    public IReadOnlyList<CartItem?>? Items { get; set; }

    public CartDiscount? Discount { get; set; }

    public CartMessages? Messages { get; set; }

    public GroupMeta? GroupMeta { get; set; }
}

// Not a Dto: the Dto name rule cannot spell address_line1.
/// <summary>A <c>shipping_address</c> or <c>billing_address</c>; the cart answers neither back.</summary>
public sealed class CartAddressInput
{
    [JsonPropertyName("first_name"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LastName { get; set; }

    [JsonPropertyName("address_line1"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AddressLine1 { get; set; }

    [JsonPropertyName("address_line2"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AddressLine2 { get; set; }

    [JsonPropertyName("city"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? City { get; set; }

    [JsonPropertyName("post_code"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PostCode { get; set; }

    [JsonPropertyName("phone"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Phone { get; set; }

    [JsonPropertyName("mobile"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Mobile { get; set; }

    [JsonPropertyName("fax"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Fax { get; set; }

    [JsonPropertyName("email"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Email { get; set; }

    [JsonPropertyName("country_code"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CountryCode { get; set; }

    [JsonPropertyName("state_code"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StateCode { get; set; }
}

/// <summary>The add's body, no envelope; <see cref="CustomerId"/> takes the customer's <c>internal_id</c>.</summary>
public sealed class CartItemInput : Dto
{
    public long? ProductId { get; set; }

    public long? Quantity { get; set; }

    public long? CustomerId { get; set; }

    public string? SessionId { get; set; }

    public CartAddressInput? ShippingAddress { get; set; }
}

/// <summary>The line update's body, no envelope; <see cref="CartItemId"/> and <see cref="Quantity"/> are required.</summary>
public sealed class CartItemUpdateInput : Dto
{
    public long? CartItemId { get; set; }

    public long? Quantity { get; set; }

    public CartAddressInput? ShippingAddress { get; set; }

    public CartAddressInput? BillingAddress { get; set; }
}

// Not a Dto: the Dto name rule cannot spell sessionId.
/// <summary>The session write's body; unset, it sends <c>{}</c>.</summary>
public sealed class CartSessionInput
{
    [JsonPropertyName(CartsUrls.CartSessionKey), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SessionId { get; set; }
}

/// <summary>The session write's answer.</summary>
public sealed class CartSession : Dto
{
    public string? SessionId { get; set; }

    /// <summary>The key <see cref="SessionId"/> reads, which the property name derives.</summary>
    public const string DeclaredSessionKey = "session_id";

    /// <summary>File the store's <c>sessionId</c> under <c>session_id</c>, the key a property name derives.</summary>
    protected override IReadOnlyDictionary<string, object?> ReadSource(
        IReadOnlyDictionary<string, object?> body)
    {
        var source = base.ReadSource(body);
        if (!source.TryGetValue(CartsUrls.CartSessionKey, out var sessionId))
            return source;
        return new Dictionary<string, object?>(source, StringComparer.Ordinal)
        {
            [DeclaredSessionKey] = sessionId,
        };
    }
}

/// <summary>The order write's body, no envelope; <see cref="CustomerId"/> takes the customer's <c>customer_id</c>.</summary>
public sealed class CartOrderInput : Dto
{
    public long? CustomerId { get; set; }

    public string? PaymentGateway { get; set; }

    public string? DeliveryType { get; set; }

    public string? TransactionNo { get; set; }
}

/// <summary>A country or a state, as an object; a state arrives <c>{}</c> when the address has none.</summary>
public sealed class Reference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }
}

/// <summary>The order's <c>customer_summary</c>, carrying both of the customer's ids.</summary>
public sealed class CartOrderCustomer : Dto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CustomerGroup { get; set; }

    public string? Gender { get; set; }

    public long? CustomerId { get; set; }

    public long? InternalId { get; set; }

    public string? Email { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public Reference? Country { get; set; }

    public Reference? State { get; set; }

    public string? PostCode { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public string? Fax { get; set; }

    public string? CompanyName { get; set; }
}

/// <summary>One entry of the order's <c>order_line_details</c>.</summary>
public sealed class CartOrderLine : Dto
{
    public long? ItemId { get; set; }

    public string? ProductName { get; set; }

    public long? ProductId { get; set; }

    public string? Sku { get; set; }

    public long? Quantity { get; set; }

    public double? Price { get; set; }

    public double? TotalAmount { get; set; }

    public double? Tax { get; set; }

    public double? Discount { get; set; }

    public double? TaxDiscount { get; set; }

    public bool? IsTaxable { get; set; }

    public bool? IsShippable { get; set; }

    public string? ImageUrl { get; set; }

    public IReadOnlyList<object?>? Variations { get; set; }

    public string? SerialBatchNumber { get; set; }
}

/// <summary>One entry of the order's <c>payments</c>.</summary>
public sealed class CartOrderPayment : Dto
{
    public long? Id { get; set; }

    public string? Status { get; set; }

    public double? Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? TrackInfo { get; set; }

    public string? GatewayName { get; set; }

    public string? GatewayCode { get; set; }

    public string? GatewayResponse { get; set; }

    public string? PayerInfo { get; set; }

    public string? PayingDate { get; set; }

    public string? TransactionDate { get; set; }

    public string? TransactionReference { get; set; }
}

/// <summary>The order's <c>billing_address</c> or <c>shipping_address</c>: the customer's, unless a line update
/// sent addresses through the conversion's own HTTP session; the add's never arrive here.</summary>
public sealed class CartOrderAddress : Dto
{
    public long? Id { get; set; }

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

/// <summary>The order a cart became, with the write's <see cref="Status"/> read from beside the envelope.</summary>
public sealed class CartOrder : Dto
{
    protected override string? EnvelopeKey => "order";

    public string? Status { get; set; }

    public long? OrderId { get; set; }

    public long? InternalId { get; set; }

    public long? OrderNo { get; set; }

    public string? TransactionNo { get; set; }

    public string? CurrencyCode { get; set; }

    public string? OrderStatus { get; set; }

    public string? PaymentStatus { get; set; }

    public string? ShippingStatus { get; set; }

    public double? SubTotal { get; set; }

    public double? ShippingCost { get; set; }

    public double? ShippingTax { get; set; }

    public double? HandlingCost { get; set; }

    public double? TotalSurcharge { get; set; }

    public double? TotalDiscount { get; set; }

    public double? TotalTax { get; set; }

    public double? GrandTotal { get; set; }

    public double? Paid { get; set; }

    public double? Due { get; set; }

    public long? ItemsTotal { get; set; }

    public string? IpAddress { get; set; }

    public string? OrderChannel { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public CartOrderCustomer? CustomerSummary { get; set; }

    public IReadOnlyList<CartOrderLine?>? OrderLineDetails { get; set; }

    public IReadOnlyList<CartOrderPayment?>? Payments { get; set; }

    public object? ExternalReferenceOrderId { get; set; }

    public object? ExternalReferenceStoreId { get; set; }

    public object? ExternalReferenceOrderNo { get; set; }

    public object? ExternalReferenceStatus { get; set; }

    public object? ExternalReferenceUrl { get; set; }

    public string? CustomerPoNumber { get; set; }

    public string? PaymentTerm { get; set; }

    public string? FulfillmentMode { get; set; }

    public string? FulfillmentTerm { get; set; }

    public string? InvoiceMode { get; set; }

    public CartOrderAddress? BillingAddress { get; set; }

    public CartOrderAddress? ShippingAddress { get; set; }

    public IReadOnlyList<object?>? CustomFields { get; set; }
}
