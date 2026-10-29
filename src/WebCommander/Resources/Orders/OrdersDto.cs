namespace WebCommander.Resources.Orders.OrdersDto;

/// <summary>A country or a state as the store answers it: <c>{"id", "name", "code"}</c>.</summary>
public sealed class Reference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }
}

/// <summary>The order's <c>customer</c> section, also sent as <c>customer_summary</c>. A listing row carries five
/// of these keys; <see cref="CustomerId"/> is the public id and <see cref="InternalId"/> the other.</summary>
public sealed class OrderCustomer : Dto
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

/// <summary>A billing or shipping address; <see cref="CompanyName"/> reads <c>""</c> whatever was written.</summary>
public sealed class OrderAddress : Dto
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

/// <summary>One order line, under <c>items</c> or <c>order_line_details</c>.</summary>
public sealed class OrderItem : Dto
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

    /// <summary><c>[]</c>, except on a create's answer, which sends <c>null</c>.</summary>
    public IReadOnlyList<object?>? Variations { get; set; }

    public string? SerialBatchNumber { get; set; }
}

/// <summary>The <c>shipping</c> section; <see cref="ShippingType"/> is the first shipment's method.</summary>
public sealed class OrderShipping : Dto
{
    public string? DeliveryType { get; set; }

    public string? ShippingType { get; set; }

    public string? ShippingStatus { get; set; }

    public double? ShippingCost { get; set; }

    public double? ShippingTax { get; set; }
}

public sealed class OrderDiscounts : Dto
{
    public double? TotalDiscount { get; set; }

    public string? CouponCode { get; set; }

    public string? GiftCardCode { get; set; }
}

/// <summary>The <c>charges_totals</c> section; a payment refund lowers <see cref="Paid"/> and an order refund does not.</summary>
public sealed class OrderChargesTotals : Dto
{
    public double? SubTotal { get; set; }

    public double? TotalTax { get; set; }

    public double? HandlingCost { get; set; }

    public double? TotalSurcharge { get; set; }

    public double? OtherCharges { get; set; }

    public double? GrandTotal { get; set; }

    public double? Paid { get; set; }

    public double? Due { get; set; }

    /// <summary>Sent as the integer <c>0</c> until a refund lands.</summary>
    public double? Refunded { get; set; }

    public string? RefundStatus { get; set; }

    public long? ItemsTotal { get; set; }
}

/// <summary>One entry of an order's <c>payments</c>; <see cref="GatewayResponse"/> comes only in the flat shape.</summary>
public sealed class OrderPayment : Dto
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

public sealed class OrderFulfillment : Dto
{
    public string? FulfillmentMode { get; set; }

    public string? FulfillmentTerm { get; set; }

    public string? InvoiceMode { get; set; }

    public string? PaymentTerm { get; set; }

    public string? CustomerPoNumber { get; set; }

    public string? ExternalReferenceOrderId { get; set; }

    public string? ExternalReferenceStoreId { get; set; }

    public string? ExternalReferenceOrderNo { get; set; }

    public string? ExternalReferenceStatus { get; set; }

    public string? ExternalReferenceUrl { get; set; }

    public IReadOnlyList<object?>? CustomFields { get; set; }
}

/// <summary>One <c>group_meta</c> entry.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block; the listing's has no entry for the two addresses.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? Customer { get; set; }

    public SectionMeta? BillingAddress { get; set; }

    public SectionMeta? ShippingAddress { get; set; }

    public SectionMeta? Items { get; set; }

    public SectionMeta? Shipping { get; set; }

    public SectionMeta? Discounts { get; set; }

    public SectionMeta? ChargesTotals { get; set; }

    public SectionMeta? Payments { get; set; }

    public SectionMeta? Fulfillment { get; set; }
}

/// <summary>One order in sections: the row path's body, a <c>PUT</c> or <c>PATCH</c> answer, and a listing row,
/// which carries no <c>billing_address</c> or <c>shipping_address</c>.</summary>
public sealed class Order : Dto
{
    protected override string? EnvelopeKey => "order";

    public long? OrderId { get; set; }

    public long? OrderNo { get; set; }

    public long? InternalId { get; set; }

    /// <summary>A UUID string the store assigns; not writable.</summary>
    public string? Uuid { get; set; }

    public string? OrderStatus { get; set; }

    public string? PaymentStatus { get; set; }

    public string? ShippingStatus { get; set; }

    public string? CurrencyCode { get; set; }

    public string? TransactionNo { get; set; }

    public string? IpAddress { get; set; }

    public string? OrderChannel { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedOn { get; set; }

    public OrderCustomer? Customer { get; set; }

    public OrderAddress? BillingAddress { get; set; }

    public OrderAddress? ShippingAddress { get; set; }

    public IReadOnlyList<OrderItem?>? Items { get; set; }

    public OrderShipping? Shipping { get; set; }

    public OrderDiscounts? Discounts { get; set; }

    public OrderChargesTotals? ChargesTotals { get; set; }

    public IReadOnlyList<OrderPayment?>? Payments { get; set; }

    public OrderFulfillment? Fulfillment { get; set; }

    /// <summary>Read from beside the envelope.</summary>
    public GroupMeta? GroupMeta { get; set; }

    /// <summary>Beside the envelope when <c>field_metadata</c> was asked for; declared <c>object</c>.</summary>
    public object? FieldMetadata { get; set; }
}

/// <summary>The listing's ten-key <c>pagination</c> block; <see cref="Limit"/> is the size applied, at most 20.</summary>
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

/// <summary>One page of the listing, with the <c>field_metadata</c> block when it was asked for.</summary>
public sealed class OrderPage : Dto
{
    public IReadOnlyList<Order?>? Orders { get; set; }

    public Pagination? Pagination { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary>The count sub-path's body, <c>{"orders": {"count": N}}</c>.</summary>
public sealed class OrderCount : Dto
{
    protected override string? EnvelopeKey => "orders";

    public long? Count { get; set; }
}

/// <summary>One order with its totals and fulfilment at the top: what a create, a cancel and a status move
/// answer, and a per-customer listing row. A create adds <see cref="Status"/> beside the envelope.</summary>
public sealed class FlatOrder : Dto
{
    protected override string? EnvelopeKey => "order";

    public long? OrderId { get; set; }

    public long? OrderNo { get; set; }

    public long? InternalId { get; set; }

    public string? Uuid { get; set; }

    public string? TransactionNo { get; set; }

    public string? CurrencyCode { get; set; }

    public string? OrderStatus { get; set; }

    public string? PaymentStatus { get; set; }

    /// <summary><c>unshipped</c> where the sectioned shape reads <c>not_shipped</c>.</summary>
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

    public OrderCustomer? CustomerSummary { get; set; }

    public IReadOnlyList<OrderItem?>? OrderLineDetails { get; set; }

    public IReadOnlyList<OrderPayment?>? Payments { get; set; }

    public string? ExternalReferenceOrderId { get; set; }

    public string? ExternalReferenceStoreId { get; set; }

    public string? ExternalReferenceOrderNo { get; set; }

    public string? ExternalReferenceStatus { get; set; }

    public string? ExternalReferenceUrl { get; set; }

    public string? CustomerPoNumber { get; set; }

    public string? PaymentTerm { get; set; }

    public string? FulfillmentMode { get; set; }

    public string? FulfillmentTerm { get; set; }

    public string? InvoiceMode { get; set; }

    public OrderAddress? BillingAddress { get; set; }

    public OrderAddress? ShippingAddress { get; set; }

    public IReadOnlyList<object?>? CustomFields { get; set; }

    public string? Status { get; set; }
}

/// <summary>One customer's orders, unpaged, cancelled ones left out.</summary>
public sealed class CustomerOrders : Dto
{
    public long? CustomerId { get; set; }

    public long? InternalId { get; set; }

    public IReadOnlyList<FlatOrder?>? Orders { get; set; }
}

/// <summary>What the addresses sub-path answers: both addresses as they now stand.</summary>
public sealed class OrderAddresses : Dto
{
    public long? OrderId { get; set; }

    public long? InternalId { get; set; }

    public OrderAddress? BillingAddress { get; set; }

    public OrderAddress? ShippingAddress { get; set; }
}

/// <summary>One comment; <see cref="Created"/> is <c>M/D/YYYY</c> and a time two spaces apart, not ISO 8601.</summary>
public sealed class OrderComment : Dto
{
    protected override string? EnvelopeKey => "comment";

    public long? Id { get; set; }

    public string? Content { get; set; }

    public string? AdminName { get; set; }

    public bool? VisibleToCustomer { get; set; }

    public bool? Admin { get; set; }

    public string? Created { get; set; }
}

/// <summary>An order's comments, oldest first and unpaged.</summary>
public sealed class OrderCommentList : Dto
{
    public IReadOnlyList<OrderComment?>? Comments { get; set; }
}

/// <summary>An order's custom field values; <see cref="OrderId"/> echoes the <c>internal_id</c> asked for, and a
/// <c>delivery</c> order adds <see cref="DeliveryCost"/>.</summary>
public sealed class OrderCustomFields : Dto
{
    public long? OrderId { get; set; }

    public IReadOnlyList<object?>? CustomFields { get; set; }

    public double? DeliveryCost { get; set; }
}

/// <summary>The customer a payment receipt names; <see cref="Id"/> is the public <c>customer_id</c>.</summary>
public sealed class PaymentReceiptCustomer : Dto
{
    public long? Id { get; set; }

    public long? InternalId { get; set; }

    public string? Name { get; set; }
}

/// <summary>What recording a payment answers; <see cref="Id"/> is the <c>payment_id</c> a payment refund takes.</summary>
public sealed class PaymentReceipt : Dto
{
    protected override string? EnvelopeKey => "payment";

    public string? Status { get; set; }

    public long? Id { get; set; }

    public string? Date { get; set; }

    public string? TransactionDate { get; set; }

    public string? TransactionReference { get; set; }

    public double? PaymentApplied { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PayerInfo { get; set; }

    public long? OrderId { get; set; }

    public long? OrderInternalId { get; set; }

    public double? OrderTotal { get; set; }

    public double? DueAmount { get; set; }

    public double? PaidAmount { get; set; }

    public PaymentReceiptCustomer? Customer { get; set; }

    public string? CreatedBy { get; set; }

    public string? CreatedAt { get; set; }
}

/// <summary>What an order refund answers; <see cref="Amount"/> is the text that was sent.</summary>
public sealed class OrderRefund : Dto
{
    public string? Status { get; set; }

    public long? RefundId { get; set; }

    public long? OrderId { get; set; }

    public long? InternalId { get; set; }

    public string? Amount { get; set; }
}

/// <summary>One order line a refund was spread over; the money fields are decimal text such as <c>"1.00"</c>.
/// <c>ext_item_referer</c> was <c>null</c> on every line read, so it is not typed.</summary>
public sealed class OrderRefundRecordLine : Dto
{
    public long? OrderItemId { get; set; }

    public string? ProductSku { get; set; }

    public string? ProductName { get; set; }

    public bool? PluginLine { get; set; }

    public long? Quantity { get; set; }

    public string? LinePaid { get; set; }

    public string? Amount { get; set; }

    public string? LineRefundedTotal { get; set; }

    public bool? LineFullyRefunded { get; set; }
}

/// <summary>One refund an order holds. <see cref="OrderId"/> is the order's internal id. A payment refund fills
/// <see cref="RecordOnly"/> to <see cref="Lines"/>; an order refund leaves them <c>null</c> and <see cref="Lines"/> empty.
/// <c>gateway_charge_id</c>, <c>gateway_refund_id</c> and <c>failure_cause</c> were <c>null</c> on every refund read,
/// so they are not typed.</summary>
public sealed class OrderRefundRecord : Dto
{
    public string? Gateway { get; set; }

    public bool? RecordOnly { get; set; }

    public string? InitiatedBy { get; set; }

    public bool? CancelledWithOrder { get; set; }

    public bool? NotifyCustomer { get; set; }

    public string? Allocation { get; set; }

    public string? OtherAmount { get; set; }

    public IReadOnlyList<OrderRefundRecordLine?>? Lines { get; set; }

    public long? RefundId { get; set; }

    public long? PaymentId { get; set; }

    public long? OrderId { get; set; }

    public double? Amount { get; set; }

    public string? Status { get; set; }

    public string? TransactionStatus { get; set; }

    public string? TransactionId { get; set; }

    public string? Reason { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>One page of an order's refunds, newest first. <see cref="OrderId"/> is the order's internal id and
/// <see cref="OrderNo"/> the <c>{order_id}</c> the path took.</summary>
public sealed class OrderRefundPage : Dto
{
    public long? OrderId { get; set; }

    public long? OrderNo { get; set; }

    public IReadOnlyList<OrderRefundRecord?>? Refunds { get; set; }

    public Pagination? Pagination { get; set; }
}

/// <summary>What a payment refund answers. <see cref="OrderId"/> holds the order's internal id and
/// <see cref="OrderNo"/> its order id; a replay drops four flags and adds <see cref="IdempotentReplay"/>.</summary>
public sealed class PaymentRefund : Dto
{
    public string? Status { get; set; }

    public long? RefundId { get; set; }

    public long? PaymentId { get; set; }

    public long? OrderId { get; set; }

    public long? OrderNo { get; set; }

    public string? Currency { get; set; }

    public double? Amount { get; set; }

    public string? Reason { get; set; }

    public bool? GatewayRefund { get; set; }

    public string? GatewayTransactionId { get; set; }

    public string? TransactionStatus { get; set; }

    public string? PaymentStatus { get; set; }

    public string? PaymentRefundStatus { get; set; }

    public string? OrderRefundStatus { get; set; }

    public double? OrderRefundedTotal { get; set; }

    public double? OrderAdjustment { get; set; }

    public double? OrderTotal { get; set; }

    public double? AvailableForRefund { get; set; }

    public bool? AdjustWithOrder { get; set; }

    public bool? RefundEmailQueued { get; set; }

    public bool? InventoryReadjusted { get; set; }

    public string? RefundedAt { get; set; }

    public bool? IdempotentReplay { get; set; }
}

/// <summary>What sending an invoice answers: who it went to and when.</summary>
public sealed class InvoiceDispatch : Dto
{
    public string? Status { get; set; }

    public long? OrderId { get; set; }

    public long? InternalId { get; set; }

    public string? SentTo { get; set; }

    public string? Template { get; set; }

    public object? InvoiceDocument { get; set; }

    public string? SentAt { get; set; }
}

/// <summary>One shipment as the shipment status lists it.</summary>
public sealed class ShipmentTracking : Dto
{
    public string? ShippingProvider { get; set; }

    public string? TrackingNumber { get; set; }

    public string? LastUpdated { get; set; }
}

/// <summary>The shipment status: <c>unshipped</c>, <c>partial</c> or <c>shipped</c>, where the order itself reads
/// <c>not_shipped</c> for the first.</summary>
public sealed class OrderShipmentStatus : Dto
{
    public long? OrderId { get; set; }

    public long? InternalId { get; set; }

    public string? ShipmentStatus { get; set; }

    public IReadOnlyList<ShipmentTracking?>? ShipmentsInfo { get; set; }
}

/// <summary>One line a shipment carries.</summary>
public sealed class ShipmentItem : Dto
{
    public long? OrderItemId { get; set; }

    public long? ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ProductSku { get; set; }

    public string? ShippingClass { get; set; }

    public long? OrderQuantity { get; set; }

    public long? ShippedQuantity { get; set; }

    public double? TotalAmount { get; set; }
}

/// <summary>One shipment. A write sends <c>method</c>, <c>tracking_info</c> and <c>shipping_date</c>, read back as
/// <see cref="ShippingMethod"/>, <see cref="TrackingNumber"/> and <see cref="DateCreated"/>; <see cref="Status"/> sits beside.</summary>
public sealed class OrderShipment : Dto
{
    protected override string? EnvelopeKey => "shipment";

    public long? ShippingId { get; set; }

    public long? OrderId { get; set; }

    public long? OrderInternalId { get; set; }

    public long? CustomerId { get; set; }

    public long? CustomerInternalId { get; set; }

    public long? OrderAddressId { get; set; }

    public string? DateCreated { get; set; }

    public string? TrackingNumber { get; set; }

    public string? ShippingMethod { get; set; }

    public OrderAddress? ShippingAddress { get; set; }

    public OrderAddress? BillingAddress { get; set; }

    public IReadOnlyList<ShipmentItem?>? Items { get; set; }

    public string? Status { get; set; }
}

/// <summary>An order's shipments, unpaged.</summary>
public sealed class OrderShipmentList : Dto
{
    public IReadOnlyList<OrderShipment?>? Shipments { get; set; }
}

/// <summary>An address to write, replacing the whole one: a field left out reads <c>""</c>.</summary>
public sealed class OrderAddressInput : Dto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? CompanyName { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? PostCode { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public string? Fax { get; set; }

    public string? CountryCode { get; set; }

    public string? StateCode { get; set; }
}

/// <summary>One line to order, by the product's id.</summary>
public sealed class OrderProductInput : Dto
{
    public long? ProductId { get; set; }

    public long? Quantity { get; set; }
}

/// <summary>The create body, with no envelope. <see cref="PaymentGateway"/> takes a code, <c>PIS</c> for pay in
/// store, and <see cref="DeliveryType"/> is lower case; <c>PAY_IN_STORE</c> and <c>SHIPPING</c> are <c>400</c>.</summary>
public class OrderInput : Dto
{
    public long? CustomerId { get; set; }

    public string? PaymentGateway { get; set; }

    public string? DeliveryType { get; set; }

    public bool? IsSendEmail { get; set; }

    public string? TransactionNo { get; set; }

    public string? ExternalReferenceOrderId { get; set; }

    public OrderAddressInput? BillingAddress { get; set; }

    public OrderAddressInput? ShippingAddress { get; set; }

    public IReadOnlyList<OrderProductInput?>? Products { get; set; }

    public string? OrderChannel { get; set; }

    public string? IpAddress { get; set; }
}

/// <summary>The payment a create-with-payment records. It is filed under method <c>API</c>, and
/// <see cref="PayerInfo"/>, <see cref="TransactionReference"/> and <see cref="TransactionDate"/> are not kept.</summary>
public sealed class InitialPaymentInput : Dto
{
    public double? Amount { get; set; }

    public double? Surcharge { get; set; }

    public string? Status { get; set; }

    public string? PaymentMethod { get; set; }

    public string? TrackInfo { get; set; }

    public string? PayerInfo { get; set; }

    public string? TransactionReference { get; set; }

    public string? TransactionDate { get; set; }
}

/// <summary>The create-with-payment body: a create's fields and the <see cref="Payment"/> it records. Its
/// <c>transaction_no</c>, <c>external_reference_order_id</c>, <c>order_channel</c> and <c>ip_address</c> are accepted
/// and not kept.</summary>
public sealed class OrderWithPaymentInput : OrderInput
{
    public InitialPaymentInput? Payment { get; set; }
}

/// <summary>The <c>PUT</c> body; it writes the customer and the two addresses, and nothing else.</summary>
public sealed class OrderUpdateInput : Dto
{
    protected override string? EnvelopeKey => "order";

    public long? CustomerId { get; set; }

    public OrderAddressInput? BillingAddress { get; set; }

    public OrderAddressInput? ShippingAddress { get; set; }
}

/// <summary>The <c>PATCH</c> body: the seven fields it writes; an address sent replaces that address.</summary>
public sealed class OrderPatchInput : Dto
{
    protected override string? EnvelopeKey => "order";

    public string? OrderStatus { get; set; }

    public string? CustomerPoNumber { get; set; }

    public string? PaymentTerm { get; set; }

    public OrderAddressInput? BillingAddress { get; set; }

    public OrderAddressInput? ShippingAddress { get; set; }

    public string? OrderChannel { get; set; }

    public string? IpAddress { get; set; }
}

/// <summary>The addresses sub-path's body; at least one address, and one left out is untouched.</summary>
public sealed class OrderAddressesInput : Dto
{
    public string? TransactionNo { get; set; }

    public OrderAddressInput? BillingAddress { get; set; }

    public OrderAddressInput? ShippingAddress { get; set; }
}

/// <summary>The cancel body; every field is optional and an empty request cancels too.</summary>
public sealed class OrderCancelInput : Dto
{
    public string? TransactionNo { get; set; }

    public string? Reason { get; set; }

    public bool? RefundAutomatically { get; set; }

    public bool? IsGatewayRefund { get; set; }
}

/// <summary>The comment body, with no envelope; <see cref="Content"/> is required.</summary>
public sealed class OrderCommentInput : Dto
{
    public string? Content { get; set; }

    public bool? SaveAndSend { get; set; }

    public string? TransactionNo { get; set; }
}

/// <summary>The payment body: <c>{"payment": {...}}</c> with <see cref="TransactionNo"/> beside the envelope.</summary>
public sealed class OrderPaymentInput : Dto
{
    protected override string? EnvelopeKey => "payment";

    protected override IReadOnlyList<string> TopLevel { get; } = new[] { "transaction_no" };

    public double? Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public string? TrackInfo { get; set; }

    public string? PayerInfo { get; set; }

    public string? TransactionDate { get; set; }

    public string? TransactionReference { get; set; }

    public double? Surcharge { get; set; }

    public string? TransactionNo { get; set; }
}

/// <summary>One line an order refund names, by the order line's <c>item_id</c>.</summary>
public sealed class RefundLineInput : Dto
{
    public long? OrderItemId { get; set; }

    public long? Quantity { get; set; }

    public double? Amount { get; set; }
}

/// <summary>The order refund body, <c>{"refund": {...}}</c>; <see cref="RefundDate"/> is required, and
/// <see cref="Amount"/> may not pass what was paid less earlier refunds.</summary>
public sealed class OrderRefundInput : Dto
{
    protected override string? EnvelopeKey => "refund";

    public string? Amount { get; set; }

    public string? RefundDate { get; set; }

    public string? Reason { get; set; }

    public IReadOnlyList<RefundLineInput?>? LineItems { get; set; }
}

/// <summary>The payment refund body: <c>{"refund": {...}}</c> with <see cref="TransactionNo"/> beside it.</summary>
public sealed class PaymentRefundInput : Dto
{
    protected override string? EnvelopeKey => "refund";

    protected override IReadOnlyList<string> TopLevel { get; } = new[] { "transaction_no" };

    public string? Amount { get; set; }

    public string? Reason { get; set; }

    public bool? IsGatewayRefund { get; set; }

    public bool? SendRefundEmail { get; set; }

    public bool? AdjustWithOrder { get; set; }

    public string? TransactionNo { get; set; }
}

/// <summary>One line to ship, by the order line's <c>item_id</c>.</summary>
public sealed class ShipmentItemInput : Dto
{
    public long? OrderItemId { get; set; }

    public long? Quantity { get; set; }
}

/// <summary>The shipment body; a create needs <see cref="Method"/>, <see cref="ShippingDate"/> and
/// <see cref="ShipmentItems"/>. An update changes only what it names, and new items need <see cref="ChangeNote"/>.</summary>
public sealed class OrderShipmentInput : Dto
{
    public string? TransactionNo { get; set; }

    public string? Method { get; set; }

    public string? TrackingInfo { get; set; }

    public string? ShippingDate { get; set; }

    public IReadOnlyList<ShipmentItemInput?>? ShipmentItems { get; set; }

    public string? ChangeNote { get; set; }
}

/// <summary>The status body; <see cref="Status"/> is required: <c>cancelled</c>, <c>completed</c>, <c>draft</c>,
/// <c>in_progress</c>, <c>paused</c> or <c>reopened</c>.</summary>
public sealed class OrderStatusInput : Dto
{
    public string? Status { get; set; }

    public string? TransactionNo { get; set; }

    public bool? RefundAutomatically { get; set; }
}

/// <summary>The custom status body; <see cref="OrderId"/> is required.</summary>
public sealed class CustomOrderStatusInput : Dto
{
    public long? OrderId { get; set; }

    public string? OrderStatusValue { get; set; }

    public string? OrderStatusLabel { get; set; }

    public string? OrderType { get; set; }

    public string? StatusType { get; set; }
}
