using WebCommander.Client;
using WebCommander.Resources.ShippingClasses.ShippingClassesDto;

namespace WebCommander.Resources.ShippingClasses;

/// <summary>Shipping classes - <c>/admin/shipping_classes*</c> - as a typed resource bound to one client.</summary>
public sealed class ShippingClasses : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.ShippingClasses</c>.</summary>
    public ShippingClasses(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the classes - <c>GET /admin/shipping_classes</c>.</summary>
    public Task<ApiResponse> ListShippingClassesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingClassesUrls.ShippingClasses,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingClassPage),
            }, cancellationToken);

    /// <summary>HEAD the class collection - <c>HEAD /admin/shipping_classes</c>.</summary>
    public Task<ApiResponse> HeadShippingClassesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingClassesUrls.ShippingClasses,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Count the classes - <c>GET /admin/shipping_classes/count</c>.</summary>
    public Task<ApiResponse> CountShippingClassesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingClassesUrls.ShippingClassesCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingClassCount),
            }, cancellationToken);

    /// <summary>Create a class - <c>POST /admin/shipping_classes</c>.</summary>
    public Task<ApiResponse> CreateShippingClassAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ShippingClassesUrls.ShippingClasses,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingClass),
            }, cancellationToken);
    }

    /// <summary>Retrieve one class - <c>GET /admin/shipping_classes/{shipping_class_id}</c>.</summary>
    public Task<ApiResponse> GetShippingClassAsync(
        object shippingClassId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingClassesUrls.ShippingClass,
            new RequestOptions
            {
                PathParams = Id(shippingClassId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingClass),
            }, cancellationToken);

    /// <summary>HEAD one class - <c>HEAD /admin/shipping_classes/{shipping_class_id}</c>.</summary>
    public Task<ApiResponse> HeadShippingClassAsync(
        object shippingClassId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingClassesUrls.ShippingClass,
            new RequestOptions
            {
                PathParams = Id(shippingClassId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Replace one class - <c>PUT /admin/shipping_classes/{shipping_class_id}</c>.</summary>
    public Task<ApiResponse> UpdateShippingClassAsync(
        object shippingClassId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(ShippingClassesUrls.ShippingClass,
            new RequestOptions
            {
                PathParams = Id(shippingClassId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingClass),
            }, cancellationToken);
    }

    /// <summary>Delete one class - <c>DELETE /admin/shipping_classes/{shipping_class_id}</c>.</summary>
    public Task<ApiResponse> DeleteShippingClassAsync(
        object shippingClassId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(ShippingClassesUrls.ShippingClass,
            new RequestOptions
            {
                PathParams = Id(shippingClassId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Make one class the default - <c>PUT /admin/shipping_classes/{shipping_class_id}/default</c>.</summary>
    public Task<ApiResponse> SetShippingClassDefaultAsync(
        object shippingClassId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPutRequestAsync(ShippingClassesUrls.ShippingClassDefault,
            new RequestOptions
            {
                PathParams = Id(shippingClassId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingClass),
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object shippingClassId) =>
        new() { ["shipping_class_id"] = shippingClassId };
}
