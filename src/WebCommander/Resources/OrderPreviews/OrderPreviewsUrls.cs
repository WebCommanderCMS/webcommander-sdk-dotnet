namespace WebCommander.Resources.OrderPreviews;

/// <summary>Path templates for the order preview endpoints.</summary>
public static class OrderPreviewsUrls
{
    public const string OrderPreviews = "/api/{api_version}/admin/orders/preview";

    public const string OrderPreview = "/api/{api_version}/admin/orders/preview/{preview_session_id}";

    public const string OrderPreviewConfirm = "/api/{api_version}/admin/orders/preview/{preview_session_id}/confirm";

    public const string OrderPreviewCoupon = "/api/{api_version}/admin/orders/preview/{preview_session_id}/coupon";

    public const string OrderPreviewGiftCard = "/api/{api_version}/admin/orders/preview/{preview_session_id}/gift-card";
}
