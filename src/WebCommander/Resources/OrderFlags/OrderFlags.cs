using WebCommander.Client;
using WebCommander.Resources.OrderFlags.OrderFlagsDto;

namespace WebCommander.Resources.OrderFlags;

/// <summary>Order flag endpoints - <c>/admin/order_flags*</c> - as a typed resource bound to one client.</summary>
public sealed class OrderFlags : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.OrderFlags</c>.</summary>
    public OrderFlags(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the flags - <c>GET /admin/order_flags</c>.</summary>
    public Task<ApiResponse> ListOrderFlagsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrderFlagsUrls.OrderFlags,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderFlagPage),
            }, cancellationToken);

    /// <summary>HEAD the flag collection - <c>HEAD /admin/order_flags</c>.</summary>
    public Task<ApiResponse> HeadOrderFlagsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", OrderFlagsUrls.OrderFlags,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a flag - <c>POST /admin/order_flags</c>.</summary>
    public Task<ApiResponse> CreateOrderFlagAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrderFlagsUrls.OrderFlags,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderFlag),
            }, cancellationToken);
    }

    /// <summary>Retrieve one flag - <c>GET /admin/order_flags/{order_flag_id}</c>.</summary>
    public Task<ApiResponse> GetOrderFlagAsync(
        object orderFlagId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrderFlagsUrls.OrderFlag,
            new RequestOptions
            {
                PathParams = Id(orderFlagId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderFlag),
            }, cancellationToken);

    /// <summary>HEAD one flag - <c>HEAD /admin/order_flags/{order_flag_id}</c>.</summary>
    public Task<ApiResponse> HeadOrderFlagAsync(
        object orderFlagId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", OrderFlagsUrls.OrderFlag,
            new RequestOptions
            {
                PathParams = Id(orderFlagId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Replace one flag - <c>PUT /admin/order_flags/{order_flag_id}</c>; send every field to keep.</summary>
    public Task<ApiResponse> UpdateOrderFlagAsync(
        object orderFlagId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(OrderFlagsUrls.OrderFlag,
            new RequestOptions
            {
                PathParams = Id(orderFlagId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderFlag),
            }, cancellationToken);
    }

    /// <summary>Delete a flag - <c>DELETE /admin/order_flags/{order_flag_id}</c>.</summary>
    public Task<ApiResponse> DeleteOrderFlagAsync(
        object orderFlagId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(OrderFlagsUrls.OrderFlag,
            new RequestOptions
            {
                PathParams = Id(orderFlagId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object orderFlagId) =>
        new() { ["order_flag_id"] = orderFlagId };
}
