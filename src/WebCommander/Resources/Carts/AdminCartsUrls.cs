namespace WebCommander.Resources.Carts;

/// <summary>Path templates and the transaction number header for the admin cart and checkout endpoints.</summary>
public static class AdminCartsUrls
{
    public const string AdminCarts = "/api/{api_version}/admin/orders/carts";

    public const string AdminCart = "/api/{api_version}/admin/orders/carts/{session_id}";

    public const string AdminCartItems = "/api/{api_version}/admin/orders/carts/{session_id}/cart_items";

    public const string AdminCartItem = "/api/{api_version}/admin/orders/carts/{session_id}/cart_items/{cart_item_id}";

    public const string AdminCheckout = "/api/{api_version}/admin/orders/checkout/{session_id}";

    public const string AdminCheckoutConfirm = "/api/{api_version}/admin/orders/checkout/{session_id}/confirm";

    public const string AdminCheckoutCoupon = "/api/{api_version}/admin/orders/checkout/{session_id}/coupon";

    public const string AdminCheckoutGiftCard = "/api/{api_version}/admin/orders/checkout/{session_id}/gift_card";

    public const string AdminCheckoutDeliverySchedules =
        "/api/{api_version}/admin/orders/checkout/{session_id}/delivery_schedules";

    /// <summary>Files the order a checkout places under this transaction number; it beats <c>transaction_no</c>.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
