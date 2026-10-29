using WebCommander.Client;
using WebCommander.Http;
using WebCommander.Resources.Carts.CartsDto;

namespace WebCommander.Resources.Carts;

/// <summary>The storefront cart - <c>/carts*</c>, not under <c>/admin</c> - and the <c>/admin</c> write
/// that turns one into an order, as a typed resource bound to one client.</summary>
public sealed class Carts : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Carts</c>.</summary>
    public Carts(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Add a product to the cart <c>session_id</c> names, creating it - <c>POST /carts</c>, <c>201</c>;
    /// re-adding raises the line's quantity. <c>customer_id</c> is the customer's <c>internal_id</c>.</summary>
    public Task<ApiResponse> AddCartItemAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(CartsUrls.Carts,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Cart),
            }, cancellationToken);
    }

    /// <summary>Retrieve the cart - <c>GET /carts/{session_id}</c>, with <c>group_meta</c> beside it. A cart
    /// with no line is <c>400</c> <c>Your shopping cart is empty</c>.</summary>
    public Task<ApiResponse> GetCartAsync(
        string sessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CartsUrls.Cart,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Cart),
            }, cancellationToken);

    /// <summary>Ask which session id to use - <c>PUT /carts/{session_id}</c>; nothing is stored. Answers the
    /// body's <c>sessionId</c>, the HTTP session's id when that is empty or null, else the path's.</summary>
    public Task<ApiResponse> ResolveCartSessionAsync(
        string sessionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(CartsUrls.Cart,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                JsonBody = payload,
                Headers = BareJsonContentType,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CartSession),
            }, cancellationToken);
    }

    /// <summary>Change one line's quantity - <c>PATCH /carts/{session_id}</c>. <c>cart_item_id</c> and
    /// <c>quantity</c> are required; the addresses are optional.</summary>
    public Task<ApiResponse> UpdateCartItemAsync(
        string sessionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CartsUrls.Cart,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Cart),
            }, cancellationToken);
    }

    /// <summary>Empty the cart - <c>DELETE /carts/{session_id}</c>; <c>204</c>, after which a read is <c>400</c>.</summary>
    public Task<ApiResponse> DeleteCartAsync(
        string sessionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(CartsUrls.Cart,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Remove one line - <c>DELETE /carts/{session_id}/cart_items/{cart_item_id}</c>; <c>204</c>.
    /// <c>cart_item_id</c> is a line's <c>id</c>; removing the last line empties the cart.</summary>
    public Task<ApiResponse> DeleteCartItemAsync(
        string sessionId, object cartItemId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(CartsUrls.CartItem,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?>
                {
                    ["session_id"] = sessionId,
                    ["cart_item_id"] = cartItemId,
                },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Turn the cart into an order - <c>POST /admin/orders/carts/{session_id}</c>, <c>201</c>, and the
    /// cart is gone. <c>customer_id</c> is the customer's <c>customer_id</c>, not its <c>internal_id</c>.
    /// <paramref name="transactionNo"/> is sent as <c>X-Transaction-No</c> and files the order under that number.</summary>
    public Task<ApiResponse> CreateOrderFromCartAsync(
        string sessionId, object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(CartsUrls.CartOrder,
            new RequestOptions
            {
                PathParams = Session(sessionId),
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [CartsUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CartOrder),
            }, cancellationToken);
    }

    // No charset: the session write needs bare application/json.
    private static readonly IReadOnlyDictionary<string, string> BareJsonContentType =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Content-Type"] = MediaType.JsonMediaType,
        };

    private static Dictionary<string, object?> Session(string sessionId) =>
        new() { ["session_id"] = sessionId };
}
