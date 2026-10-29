namespace WebCommander.Resources.Carts;

/// <summary>Path templates, body keys and the transaction number header for the cart endpoints.</summary>
public static class CartsUrls
{
    public const string Carts = "/api/{api_version}/carts";

    public const string Cart = "/api/{api_version}/carts/{session_id}";

    public const string CartItem = "/api/{api_version}/carts/{session_id}/cart_items/{cart_item_id}";

    /// <summary>The one cart path under <c>/admin</c>.</summary>
    public const string CartOrder = "/api/{api_version}/admin/orders/carts/{session_id}";

    /// <summary>The key the session write takes and answers under, spelled as the store spells it.</summary>
    public const string CartSessionKey = "sessionId";

    /// <summary>Files the order a cart becomes under this transaction number; it beats <c>transaction_no</c>.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
