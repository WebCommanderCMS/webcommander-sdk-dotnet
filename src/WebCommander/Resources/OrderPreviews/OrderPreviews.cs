using WebCommander.Client;
using WebCommander.Resources.OrderPreviews.OrderPreviewsDto;
using WebCommander.Resources.Orders.OrdersDto;

namespace WebCommander.Resources.OrderPreviews;

/// <summary>Order previews - <c>/admin/orders/preview*</c> - as a typed resource bound to one client.</summary>
public sealed class OrderPreviews : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.OrderPreviews</c>.</summary>
    public OrderPreviews(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Start an order preview - <c>POST /admin/orders/preview</c>, <c>201</c>, a priced session that places no
    /// order. <c>products</c> is required; read <c>order.session.session_id</c>, which every other call takes as
    /// <c>previewSessionId</c>. The session lasts 30 minutes.</summary>
    public Task<ApiResponse> CreateOrderPreviewAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrderPreviewsUrls.OrderPreviews,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderPreview),
            }, cancellationToken);
    }

    /// <summary>Read an order preview - <c>GET /admin/orders/preview/{preview_session_id}</c>, where the slot takes the
    /// <c>session_id</c> the start answered. A read does not extend the session. An unknown id is <c>404</c>, and a
    /// session already confirmed is <c>410</c> <c>session_confirmed</c>.</summary>
    public Task<ApiResponse> GetOrderPreviewAsync(
        string previewSessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrderPreviewsUrls.OrderPreview,
            new RequestOptions
            {
                PathParams = SessionId(previewSessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderPreview),
            }, cancellationToken);

    /// <summary>Place the order - <c>POST /admin/orders/preview/{preview_session_id}/confirm</c>; <c>201</c> with the
    /// created order, and the body optional. This creates a real order and closes the session, so a second confirm, or
    /// any other call on it, is <c>410</c> <c>session_confirmed</c>. Without a <c>payment</c> the order is unpaid, and so
    /// it is after a <c>payment_gateway</c> alone.</summary>
    public Task<ApiResponse> ConfirmOrderPreviewAsync(
        string previewSessionId, object? payload = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPostRequestAsync(OrderPreviewsUrls.OrderPreviewConfirm,
            new RequestOptions
            {
                PathParams = SessionId(previewSessionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(FlatOrder),
            }, cancellationToken);

    /// <summary>Apply a coupon - <c>POST /admin/orders/preview/{preview_session_id}/coupon</c>, <c>200</c> with
    /// <c>session</c>, <c>discounts</c> and <c>totals</c>. One coupon at a time: a second is <c>409</c>, and a code the
    /// store does not accept is <c>422</c> <c>invalid_coupon</c>.</summary>
    public Task<ApiResponse> ApplyOrderPreviewCouponAsync(
        string previewSessionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrderPreviewsUrls.OrderPreviewCoupon,
            new RequestOptions
            {
                PathParams = SessionId(previewSessionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderPreview),
            }, cancellationToken);
    }

    /// <summary>Remove the coupon - <c>DELETE /admin/orders/preview/{preview_session_id}/coupon</c>, <c>204</c> with no
    /// body. With none applied it is <c>404</c> <c>coupon_not_applied</c>.</summary>
    public Task<ApiResponse> RemoveOrderPreviewCouponAsync(
        string previewSessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(OrderPreviewsUrls.OrderPreviewCoupon,
            new RequestOptions
            {
                PathParams = SessionId(previewSessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Apply a gift card - <c>POST /admin/orders/preview/{preview_session_id}/gift-card</c>, <c>200</c> with
    /// <c>session</c>, <c>discounts</c> and <c>totals</c>. The card must be the previewed customer's, active, paid, in
    /// date and with a balance, else <c>422</c> <c>invalid_gift_card</c>; a second card is <c>409</c>. It lowers
    /// <c>due</c> and spends nothing until an order is placed with it applied.</summary>
    public Task<ApiResponse> ApplyOrderPreviewGiftCardAsync(
        string previewSessionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrderPreviewsUrls.OrderPreviewGiftCard,
            new RequestOptions
            {
                PathParams = SessionId(previewSessionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderPreview),
            }, cancellationToken);
    }

    /// <summary>Remove the gift card - <c>DELETE /admin/orders/preview/{preview_session_id}/gift-card</c>, <c>204</c>
    /// with no body. With none applied it is <c>404</c> <c>gift_card_not_applied</c>.</summary>
    public Task<ApiResponse> RemoveOrderPreviewGiftCardAsync(
        string previewSessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(OrderPreviewsUrls.OrderPreviewGiftCard,
            new RequestOptions
            {
                PathParams = SessionId(previewSessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> SessionId(string previewSessionId) =>
        new() { ["preview_session_id"] = previewSessionId };
}
