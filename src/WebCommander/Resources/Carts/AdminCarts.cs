using WebCommander.Client;
using WebCommander.Resources.Carts.AdminCartsDto;
using WebCommander.Resources.Orders.OrdersDto;

namespace WebCommander.Resources.Carts;

/// <summary>The admin cart and checkout - <c>/admin/orders/carts*</c> and <c>/admin/orders/checkout*</c> - as a typed
/// resource bound to one client. Storefront carts are <see cref="Carts"/>.</summary>
public sealed class AdminCarts : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.AdminCarts</c>.</summary>
    public AdminCarts(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Start a cart for a customer - <c>POST /admin/orders/carts</c>, <c>201</c>. <c>customer_id</c> is the
    /// customer's <c>customer_id</c> and <c>products</c> (<c>product_id</c> and <c>quantity</c>) is required. Keep
    /// <c>cart.session.session_id</c>: every other call takes it as <c>sessionId</c>. The session lasts 30 minutes from the
    /// last write.</summary>
    public Task<ApiResponse> CreateAdminCartAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(AdminCartsUrls.AdminCarts,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCart),
            }, cancellationToken);
    }

    /// <summary>Read the cart - <c>GET /admin/orders/carts/{session_id}</c>, where the slot takes
    /// <c>cart.session.session_id</c>. A deleted cart is <c>410</c> <c>session_deleted</c> and one an order was placed
    /// from is <c>410</c> <c>session_confirmed</c>.</summary>
    public Task<ApiResponse> GetAdminCartAsync(
        string sessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(AdminCartsUrls.AdminCart,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCart),
            }, cancellationToken);

    /// <summary>Replace the cart's customer and lines - <c>PUT /admin/orders/carts/{session_id}</c>, <c>200</c>.
    /// <c>customer_id</c> and <c>products</c> are both sent. The lines are put on again, so each gets a new
    /// <c>cart_item_id</c>.</summary>
    public Task<ApiResponse> ReplaceAdminCartAsync(
        string sessionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(AdminCartsUrls.AdminCart,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCart),
            }, cancellationToken);
    }

    /// <summary>Change the cart's customer - <c>PATCH /admin/orders/carts/{session_id}</c>, <c>200</c>. Only
    /// <c>customer_id</c> is taken, and it is required: <c>products</c> is <c>400</c> <c>unknown_field</c>.</summary>
    public Task<ApiResponse> UpdateAdminCartAsync(
        string sessionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(AdminCartsUrls.AdminCart,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCart),
            }, cancellationToken);
    }

    /// <summary>Delete the cart - <c>DELETE /admin/orders/carts/{session_id}</c>; <c>204</c> with no body, after which
    /// every call on the session is <c>410</c>.</summary>
    public Task<ApiResponse> DeleteAdminCartAsync(
        string sessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(AdminCartsUrls.AdminCart,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Put products on the cart - <c>POST /admin/orders/carts/{session_id}/cart_items</c>, <c>201</c> with the
    /// whole cart. <c>products</c> is required; one already on the cart raises that line's quantity.</summary>
    public Task<ApiResponse> AddAdminCartItemsAsync(
        string sessionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(AdminCartsUrls.AdminCartItems,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCart),
            }, cancellationToken);
    }

    /// <summary>Take every line off the cart - <c>DELETE /admin/orders/carts/{session_id}/cart_items</c>, <c>200</c>
    /// with the empty cart. The session stays.</summary>
    public Task<ApiResponse> ClearAdminCartItemsAsync(
        string sessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(AdminCartsUrls.AdminCartItems,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCart),
            }, cancellationToken);

    /// <summary>Set one line's quantity - <c>PATCH /admin/orders/carts/{session_id}/cart_items/{cart_item_id}</c>,
    /// <c>200</c> with the cart. <c>cart_item_id</c> is a line's <c>cart_item_id</c> and <c>quantity</c> is required. A line
    /// the cart does not hold is <c>404</c>.</summary>
    public Task<ApiResponse> UpdateAdminCartItemAsync(
        string sessionId, object cartItemId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(AdminCartsUrls.AdminCartItem,
            new RequestOptions
            {
                PathParams = Session(sessionId, cartItemId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCart),
            }, cancellationToken);
    }

    /// <summary>Take one line off the cart - <c>DELETE /admin/orders/carts/{session_id}/cart_items/{cart_item_id}</c>,
    /// <c>200</c> with the cart. <c>cart_item_id</c> is a line's <c>cart_item_id</c>; one the cart does not hold is
    /// <c>404</c>.</summary>
    public Task<ApiResponse> DeleteAdminCartItemAsync(
        string sessionId, object cartItemId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(AdminCartsUrls.AdminCartItem,
            new RequestOptions
            {
                PathParams = Session(sessionId, cartItemId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCart),
            }, cancellationToken);

    /// <summary>Read the checkout of a cart - <c>GET /admin/orders/checkout/{session_id}</c>. <c>issues</c> lists what still
    /// blocks a confirm, <c>delivery_type</c> <c>required</c> until one is set; the addresses start as the customer's.</summary>
    public Task<ApiResponse> GetCheckoutAsync(
        string sessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(AdminCartsUrls.AdminCheckout,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCheckout),
            }, cancellationToken);

    /// <summary>Set the delivery and addresses - <c>PATCH /admin/orders/checkout/{session_id}</c>, <c>200</c> with the priced
    /// checkout. Only <c>delivery_type</c>, <c>billing_address</c> and <c>shipping_address</c> are taken. The shipping and
    /// handling cost follow the shipping address, so set it with <c>delivery_type</c>.</summary>
    public Task<ApiResponse> UpdateCheckoutAsync(
        string sessionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(AdminCartsUrls.AdminCheckout,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCheckout),
            }, cancellationToken);
    }

    /// <summary>Place the order - <c>POST /admin/orders/checkout/{session_id}/confirm</c>; <c>201</c> with the order, and the
    /// body optional. This creates a real order and closes the session, so every later call on it is <c>410</c>
    /// <c>session_confirmed</c>. A checkout with <c>issues</c> is <c>422</c> <c>checkout_incomplete</c>. Without a
    /// <c>payment</c> the order is unpaid. <paramref name="transactionNo"/> is sent as <c>X-Transaction-No</c> and files the
    /// order under that number.</summary>
    public Task<ApiResponse> ConfirmCheckoutAsync(
        string sessionId, object? payload = null, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPostRequestAsync(AdminCartsUrls.AdminCheckoutConfirm,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [AdminCartsUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(FlatOrder),
            }, cancellationToken);

    /// <summary>Apply a coupon - <c>POST /admin/orders/checkout/{session_id}/coupon</c>, <c>200</c>. <c>code</c> is required.
    /// One coupon at a time: a second is <c>409</c>, and a code the store does not accept is <c>422</c>
    /// <c>invalid_coupon</c>.</summary>
    public Task<ApiResponse> ApplyCheckoutCouponAsync(
        string sessionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(AdminCartsUrls.AdminCheckoutCoupon,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCheckout),
            }, cancellationToken);
    }

    /// <summary>Remove the coupon - <c>DELETE /admin/orders/checkout/{session_id}/coupon</c>. With none applied it is
    /// <c>404</c> <c>coupon_not_applied</c>.</summary>
    public Task<ApiResponse> RemoveCheckoutCouponAsync(
        string sessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(AdminCartsUrls.AdminCheckoutCoupon,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCheckout),
            }, cancellationToken);

    /// <summary>Apply a gift card - <c>POST /admin/orders/checkout/{session_id}/gift_card</c>, <c>200</c>. <c>code</c> is
    /// required. The card must be usable by the cart's customer, else <c>422</c> <c>invalid_gift_card</c>; a second card is
    /// <c>409</c>. It lowers <c>due</c> and spends nothing until an order is placed.</summary>
    public Task<ApiResponse> ApplyCheckoutGiftCardAsync(
        string sessionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(AdminCartsUrls.AdminCheckoutGiftCard,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCheckout),
            }, cancellationToken);
    }

    /// <summary>Remove the gift card - <c>DELETE /admin/orders/checkout/{session_id}/gift_card</c>. With none applied it
    /// is <c>404</c> <c>gift_card_not_applied</c>.</summary>
    public Task<ApiResponse> RemoveCheckoutGiftCardAsync(
        string sessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(AdminCartsUrls.AdminCheckoutGiftCard,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCheckout),
            }, cancellationToken);

    /// <summary>Read the chosen delivery schedule - <c>GET /admin/orders/checkout/{session_id}/delivery_schedules</c>,
    /// with <c>session</c> and <c>delivery_schedule</c>, which is <c>null</c> until one is set.</summary>
    public Task<ApiResponse> GetCheckoutDeliverySchedulesAsync(
        string sessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(AdminCartsUrls.AdminCheckoutDeliverySchedules,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCheckout),
            }, cancellationToken);

    /// <summary>Choose a delivery schedule and date - <c>PUT /admin/orders/checkout/{session_id}/delivery_schedules</c>.
    /// <c>id</c> and <c>date</c> are both required. Only a <c>delivery_type</c> of <c>delivery</c> takes one (else <c>422</c>),
    /// an unknown <c>id</c> is <c>404</c> and one that does not apply to the order is <c>422</c>
    /// <c>schedule_not_applicable</c>.</summary>
    public Task<ApiResponse> SetCheckoutDeliveryScheduleAsync(
        string sessionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(AdminCartsUrls.AdminCheckoutDeliverySchedules,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AdminCheckout),
            }, cancellationToken);
    }

    /// <summary>Clear the delivery schedule - <c>DELETE /admin/orders/checkout/{session_id}/delivery_schedules</c>;
    /// <c>204</c> with no body, and also when none was set.</summary>
    public Task<ApiResponse> ClearCheckoutDeliveryScheduleAsync(
        string sessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(AdminCartsUrls.AdminCheckoutDeliverySchedules,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Session(string sessionId) =>
        new() { ["session_id"] = sessionId };

    private static Dictionary<string, object?> Session(string sessionId, object cartItemId) =>
        new() { ["session_id"] = sessionId, ["cart_item_id"] = cartItemId };
}
