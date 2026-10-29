namespace WebCommander.Resources.Orders;

/// <summary>Path templates, the refund header and the transaction number header for the order endpoints.</summary>
public static class OrdersUrls
{
    public const string Orders = "/api/{api_version}/admin/orders";

    public const string OrdersCount = "/api/{api_version}/admin/orders/count";

    public const string OrdersWithPayment = "/api/{api_version}/admin/orders/with-payment";

    public const string Order = "/api/{api_version}/admin/orders/{order_id}";

    public const string OrderAddresses = "/api/{api_version}/admin/orders/{order_id}/addresses";

    public const string OrderCancel = "/api/{api_version}/admin/orders/{order_id}/cancel";

    public const string OrderComments = "/api/{api_version}/admin/orders/{order_id}/comments";

    public const string OrderPayments = "/api/{api_version}/admin/orders/{order_id}/payments";

    public const string OrderRefunds = "/api/{api_version}/admin/orders/{order_id}/refunds";

    public const string OrderShipmentStatus =
        "/api/{api_version}/admin/orders/{order_id}/shipment-status";

    public const string OrderShipments = "/api/{api_version}/admin/orders/{order_id}/shipments";

    public const string OrderShipment =
        "/api/{api_version}/admin/orders/{order_id}/shipments/{shipment_id}";

    public const string OrderUpdateStatus =
        "/api/{api_version}/admin/orders/{order_id}/update-status";

    /// <summary>Resolves its slot against the order's <c>internal_id</c>: an <c>order_id</c> there names another order, or none.</summary>
    public const string OrderCustomFields =
        "/api/{api_version}/admin/orders/{order_internal_id}/custom_fields";

    /// <summary>Resolves its slot against the order's <c>internal_id</c>, as <see cref="OrderCustomFields"/> does.</summary>
    public const string OrderSendInvoice =
        "/api/{api_version}/admin/orders/{order_internal_id}/send-invoice";

    public const string PaymentRefunds = "/api/{api_version}/admin/payment/{payment_id}/refunds";

    public const string OrderStatusAdd = "/api/{api_version}/admin/order/add-order-status";

    public const string CustomerOrders = "/api/{api_version}/admin/customers/{customer_id}/orders";

    /// <summary>The header a payment refund is refused without; a repeat under one key replays the first refund.</summary>
    public const string IdempotencyKeyHeader = "Idempotency-Key";

    /// <summary>Files a created order under this transaction number; it beats a <c>transaction_no</c> body field.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
