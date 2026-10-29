using WebCommander.Client;
using WebCommander.Resources.ShippingMethods.ShippingMethodsDto;

namespace WebCommander.Resources.ShippingMethods;

/// <summary>Shipping methods - <c>/admin/shipping_methods*</c> - as a typed resource bound to one client.</summary>
public sealed class ShippingMethods : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.ShippingMethods</c>.</summary>
    public ShippingMethods(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the methods - <c>GET /admin/shipping_methods</c>.</summary>
    public Task<ApiResponse> ListShippingMethodsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingMethodsUrls.ShippingMethods,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingMethodPage),
            }, cancellationToken);

    /// <summary>HEAD the method collection - <c>HEAD /admin/shipping_methods</c>.</summary>
    public Task<ApiResponse> HeadShippingMethodsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingMethodsUrls.ShippingMethods,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Count the methods - <c>GET /admin/shipping_methods/count</c>.</summary>
    public Task<ApiResponse> CountShippingMethodsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingMethodsUrls.ShippingMethodsCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingMethodCount),
            }, cancellationToken);

    /// <summary>Create a method - <c>POST /admin/shipping_methods</c>.</summary>
    public Task<ApiResponse> CreateShippingMethodAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ShippingMethodsUrls.ShippingMethods,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingMethod),
            }, cancellationToken);
    }

    /// <summary>Retrieve one method - <c>GET /admin/shipping_methods/{shipping_method_id}</c>.</summary>
    public Task<ApiResponse> GetShippingMethodAsync(
        object shippingMethodId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingMethodsUrls.ShippingMethod,
            new RequestOptions
            {
                PathParams = Id(shippingMethodId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingMethod),
            }, cancellationToken);

    /// <summary>HEAD one method - <c>HEAD /admin/shipping_methods/{shipping_method_id}</c>.</summary>
    public Task<ApiResponse> HeadShippingMethodAsync(
        object shippingMethodId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingMethodsUrls.ShippingMethod,
            new RequestOptions
            {
                PathParams = Id(shippingMethodId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Rename one method - <c>PUT /admin/shipping_methods/{shipping_method_id}</c>.</summary>
    public Task<ApiResponse> UpdateShippingMethodAsync(
        object shippingMethodId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(ShippingMethodsUrls.ShippingMethod,
            new RequestOptions
            {
                PathParams = Id(shippingMethodId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingMethod),
            }, cancellationToken);
    }

    /// <summary>Delete a method - <c>DELETE /admin/shipping_methods/{shipping_method_id}</c>.</summary>
    public Task<ApiResponse> DeleteShippingMethodAsync(
        object shippingMethodId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(ShippingMethodsUrls.ShippingMethod,
            new RequestOptions
            {
                PathParams = Id(shippingMethodId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Make one method the store default - <c>PUT /admin/shipping_methods/{shipping_method_id}/default</c>.</summary>
    public Task<ApiResponse> SetShippingMethodDefaultAsync(
        object shippingMethodId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPutRequestAsync(ShippingMethodsUrls.ShippingMethodDefault,
            new RequestOptions
            {
                PathParams = Id(shippingMethodId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingMethod),
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object shippingMethodId) =>
        new() { ["shipping_method_id"] = shippingMethodId };
}
