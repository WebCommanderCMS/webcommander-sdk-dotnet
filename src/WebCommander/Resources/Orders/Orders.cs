using WebCommander.Client;
using WebCommander.Resources.Orders.OrdersDto;

namespace WebCommander.Resources.Orders;

/// <summary>Orders - <c>/admin/orders*</c>, with a payment's refunds, the custom status write and one customer's orders - as a typed resource bound to one client.</summary>
public sealed class Orders : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Orders</c>.</summary>
    public Orders(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List orders - <c>GET /admin/orders</c>, newest first; cancelled ones only under <c>order_status</c>.
    /// <c>limit</c> caps at 20, <c>customerId</c> takes a customer's <c>internal_id</c>, an unknown name is <c>400</c>.</summary>
    public Task<ApiResponse> ListOrdersAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrdersUrls.Orders,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderPage),
            }, cancellationToken);

    /// <summary>Count orders - <c>GET /admin/orders/count</c>, <c>{"orders": {"count": N}}</c>; the listing's
    /// filters narrow it, and its paging, sort and <c>field_metadata</c> change nothing.</summary>
    public Task<ApiResponse> CountOrdersAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrdersUrls.OrdersCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderCount),
            }, cancellationToken);

    /// <summary>Create an order - <c>POST /admin/orders</c>, <c>201</c> with the flat shape and <c>status</c>.
    /// Only <c>products</c> is required; <c>payment_gateway</c> takes a code such as <c>PIS</c>.
    /// <paramref name="transactionNo"/> is sent as <c>X-Transaction-No</c> and files the order under that number.</summary>
    public Task<ApiResponse> CreateOrderAsync(
        object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrdersUrls.Orders,
            new RequestOptions
            {
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [OrdersUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(FlatOrder),
            }, cancellationToken);
    }

    /// <summary>Create an order and record a payment on it - <c>POST /admin/orders/with-payment</c>; <c>201</c>.
    /// <c>payment</c> is required and filed under gateway <c>API</c>; <c>transaction_no</c> is dropped.</summary>
    public Task<ApiResponse> CreateOrderWithPaymentAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrdersUrls.OrdersWithPayment,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(FlatOrder),
            }, cancellationToken);
    }

    /// <summary>Retrieve one order - <c>GET /admin/orders/{order_id}</c>; <c>field_metadata</c> adds the
    /// listing's schema, and any other parameter is ignored.</summary>
    public Task<ApiResponse> GetOrderAsync(
        object orderId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrdersUrls.Order,
            new RequestOptions
            {
                PathParams = Id(orderId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Order),
            }, cancellationToken);

    /// <summary>Update an order - <c>PUT /admin/orders/{order_id}</c>: the customer and the addresses, each address
    /// sent replacing the stored one. Sections left out are kept; <c>items</c> is <c>400</c>.</summary>
    public Task<ApiResponse> UpdateOrderAsync(
        object orderId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(OrdersUrls.Order,
            new RequestOptions
            {
                PathParams = Id(orderId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Order),
            }, cancellationToken);
    }

    /// <summary>Change named fields - <c>PATCH /admin/orders/{order_id}</c>: <c>order_status</c>, the addresses,
    /// and <c>customer_po_number</c>, <c>payment_term</c>, <c>order_channel</c> and <c>ip_address</c>, flat or under <c>fulfillment</c>.</summary>
    public Task<ApiResponse> PatchOrderAsync(
        object orderId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(OrdersUrls.Order,
            new RequestOptions
            {
                PathParams = Id(orderId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Order),
            }, cancellationToken);
    }

    /// <summary>Delete an order - <c>DELETE /admin/orders/{order_id}</c>; hard. Read <c>status</c>: <c>deleted</c>,
    /// or <c>not_found</c> for an id no order holds, both at <c>200</c>.</summary>
    public Task<ApiResponse> DeleteOrderAsync(
        object orderId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(OrdersUrls.Order,
            new RequestOptions
            {
                PathParams = Id(orderId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Replace one or both addresses - <c>PATCH /admin/orders/{order_id}/addresses</c>; an address left
    /// out is untouched, and <c>country_code</c> or a <c>country</c> object both resolve.</summary>
    public Task<ApiResponse> UpdateOrderAddressesAsync(
        object orderId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(OrdersUrls.OrderAddresses,
            new RequestOptions
            {
                PathParams = Id(orderId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderAddresses),
            }, cancellationToken);
    }

    /// <summary>Cancel an order - <c>POST /admin/orders/{order_id}/cancel</c>; <c>201</c>, the body optional. An
    /// order already cancelled is <c>409</c>; <see cref="UpdateOrderStatusAsync"/> can move it back.</summary>
    public Task<ApiResponse> CancelOrderAsync(
        object orderId, object? payload = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPostRequestAsync(OrdersUrls.OrderCancel,
            new RequestOptions
            {
                PathParams = Id(orderId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(FlatOrder),
            }, cancellationToken);

    /// <summary>An order's comments, oldest first and unpaged - <c>GET /admin/orders/{order_id}/comments</c>.</summary>
    public Task<ApiResponse> ListOrderCommentsAsync(
        object orderId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrdersUrls.OrderComments,
            new RequestOptions
            {
                PathParams = Id(orderId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderCommentList),
            }, cancellationToken);

    /// <summary>Add a comment - <c>POST /admin/orders/{order_id}/comments</c>; <c>201</c>. A bare record: a
    /// <c>comment</c> wrapper hides <c>content</c> and is <c>400</c>.</summary>
    public Task<ApiResponse> CreateOrderCommentAsync(
        object orderId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrdersUrls.OrderComments,
            new RequestOptions
            {
                PathParams = Id(orderId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderComment),
            }, cancellationToken);
    }

    /// <summary>An order's custom field values - <c>GET /admin/orders/{order_internal_id}/custom_fields</c>.
    /// <paramref name="orderInternalId"/> is the order's <c>internal_id</c>: an <c>order_id</c> reads the order holding it as its <c>internal_id</c>.</summary>
    public Task<ApiResponse> GetOrderCustomFieldsAsync(
        object orderInternalId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrdersUrls.OrderCustomFields,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?> { ["order_internal_id"] = orderInternalId },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrdersDto.OrderCustomFields),
            }, cancellationToken);

    /// <summary>Record a payment - <c>POST /admin/orders/{order_id}/payments</c>; <c>201</c>. The <c>payment</c>
    /// wrapper is required, and the receipt's <c>id</c> is what a payment refund takes.</summary>
    public Task<ApiResponse> CreateOrderPaymentAsync(
        object orderId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrdersUrls.OrderPayments,
            new RequestOptions
            {
                PathParams = Id(orderId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PaymentReceipt),
            }, cancellationToken);
    }

    /// <summary>List an order's refunds, newest first - <c>GET /admin/orders/{order_id}/refunds</c>, by the order's
    /// <c>order_id</c>. <c>limit</c> is 1 to 20 (above 20 is capped, <c>0</c> is <c>400</c>), with <c>offset</c> or a
    /// 1-based <c>page</c>; any other name is ignored.</summary>
    public Task<ApiResponse> ListOrderRefundsAsync(
        object orderId, IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrdersUrls.OrderRefunds,
            new RequestOptions
            {
                PathParams = Id(orderId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderRefundPage),
            }, cancellationToken);

    /// <summary>Refund against an order - <c>POST /admin/orders/{order_id}/refunds</c>; <c>201</c>. It raises
    /// <c>refunded</c>, leaves <c>paid</c> alone, and cannot pass what was paid less earlier refunds.</summary>
    public Task<ApiResponse> CreateOrderRefundAsync(
        object orderId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrdersUrls.OrderRefunds,
            new RequestOptions
            {
                PathParams = Id(orderId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderRefund),
            }, cancellationToken);
    }

    /// <summary>Refund part of a payment - <c>POST /admin/payment/{payment_id}/refunds</c>; <c>200</c>. Refused
    /// without <paramref name="idempotencyKey"/>; a repeat under one key replays the first. <paramref name="paymentId"/> is a payment receipt's <c>id</c>.</summary>
    public Task<ApiResponse> CreatePaymentRefundAsync(
        object paymentId, object payload, string idempotencyKey,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrdersUrls.PaymentRefunds,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?> { ["payment_id"] = paymentId },
                JsonBody = payload,
                Headers = new Dictionary<string, string>
                {
                    [OrdersUrls.IdempotencyKeyHeader] = idempotencyKey,
                },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PaymentRefund),
            }, cancellationToken);
    }

    /// <summary>Email the invoice - <c>POST /admin/orders/{order_internal_id}/send-invoice</c>; no body.
    /// <paramref name="orderInternalId"/> is the order's <c>internal_id</c>, as on <see cref="GetOrderCustomFieldsAsync"/>.</summary>
    public Task<ApiResponse> SendOrderInvoiceAsync(
        object orderInternalId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPostRequestAsync(OrdersUrls.OrderSendInvoice,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?> { ["order_internal_id"] = orderInternalId },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(InvoiceDispatch),
            }, cancellationToken);

    /// <summary>How far an order has shipped - <c>GET /admin/orders/{order_id}/shipment-status</c>.</summary>
    public Task<ApiResponse> GetOrderShipmentStatusAsync(
        object orderId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrdersUrls.OrderShipmentStatus,
            new RequestOptions
            {
                PathParams = Id(orderId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderShipmentStatus),
            }, cancellationToken);

    /// <summary>List an order's shipments - <c>GET /admin/orders/{order_id}/shipments</c>; unpaged.</summary>
    public Task<ApiResponse> ListOrderShipmentsAsync(
        object orderId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrdersUrls.OrderShipments,
            new RequestOptions
            {
                PathParams = Id(orderId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderShipmentList),
            }, cancellationToken);

    /// <summary>Ship order lines - <c>POST /admin/orders/{order_id}/shipments</c>; <c>201</c>. <c>method</c>,
    /// <c>shipping_date</c> and <c>shipment_items</c> are required; <c>tracking_info</c> may be left out.</summary>
    public Task<ApiResponse> CreateOrderShipmentAsync(
        object orderId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrdersUrls.OrderShipments,
            new RequestOptions
            {
                PathParams = Id(orderId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderShipment),
            }, cancellationToken);
    }

    /// <summary>Change a shipment - <c>PATCH /admin/orders/{order_id}/shipments/{shipment_id}</c>, by its <c>shipping_id</c>;
    /// a field left out is kept, and <c>shipment_items</c> needs <c>change_note</c>.</summary>
    public Task<ApiResponse> UpdateOrderShipmentAsync(
        object orderId, object shipmentId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(OrdersUrls.OrderShipment,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?>
                {
                    ["order_id"] = orderId,
                    ["shipment_id"] = shipmentId,
                },
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderShipment),
            }, cancellationToken);
    }

    /// <summary>Move an order's status - <c>PATCH /admin/orders/{order_id}/update-status</c>, answered with the flat
    /// shape. A completed or cancelled order can be moved again.</summary>
    public Task<ApiResponse> UpdateOrderStatusAsync(
        object orderId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(OrdersUrls.OrderUpdateStatus,
            new RequestOptions
            {
                PathParams = Id(orderId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(FlatOrder),
            }, cancellationToken);
    }

    /// <summary>Add a custom order status - <c>POST /admin/order/add-order-status</c>; <c>201 {"success": "true"}</c>.
    /// No endpoint lists, applies or deletes one, and the order keeps its status.</summary>
    public Task<ApiResponse> CreateOrderStatusAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrdersUrls.OrderStatusAdd,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
    }

    /// <summary>One customer's orders - <c>GET /admin/customers/{customer_id}/orders</c>, by the public <c>customer_id</c>;
    /// unpaged, no parameter read, and cancelled orders left out.</summary>
    public Task<ApiResponse> ListCustomerOrdersAsync(
        object customerId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrdersUrls.CustomerOrders,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?> { ["customer_id"] = customerId },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerOrders),
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object orderId) =>
        new() { ["order_id"] = orderId };
}
