using WebCommander.Client;
using WebCommander.Resources.InventoryLocations.InventoryLocationsDto;

namespace WebCommander.Resources.InventoryLocations;

/// <summary>Inventory locations - <c>/admin/inventory_locations*</c> - as a typed resource bound to one client.</summary>
public sealed class InventoryLocations : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.InventoryLocations</c>.</summary>
    public InventoryLocations(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the locations - <c>GET /admin/inventory_locations</c>.</summary>
    public Task<ApiResponse> ListInventoryLocationsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(InventoryLocationsUrls.InventoryLocations,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(InventoryLocationPage),
            }, cancellationToken);

    /// <summary>HEAD the location collection - <c>HEAD /admin/inventory_locations</c>.</summary>
    public Task<ApiResponse> HeadInventoryLocationsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", InventoryLocationsUrls.InventoryLocations,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a location - <c>POST /admin/inventory_locations</c>, with a bare-record body.</summary>
    public Task<ApiResponse> CreateInventoryLocationAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(InventoryLocationsUrls.InventoryLocations,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(InventoryLocation),
            }, cancellationToken);
    }

    /// <summary>Retrieve one location - <c>GET /admin/inventory_locations/{inventory_location_id}</c>.</summary>
    public Task<ApiResponse> GetInventoryLocationAsync(
        object inventoryLocationId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(InventoryLocationsUrls.InventoryLocation,
            new RequestOptions
            {
                PathParams = Id(inventoryLocationId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(InventoryLocation),
            }, cancellationToken);

    /// <summary>HEAD one location - <c>HEAD /admin/inventory_locations/{inventory_location_id}</c>.</summary>
    public Task<ApiResponse> HeadInventoryLocationAsync(
        object inventoryLocationId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", InventoryLocationsUrls.InventoryLocation,
            new RequestOptions
            {
                PathParams = Id(inventoryLocationId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Replace one location, every field - <c>PUT /admin/inventory_locations/{inventory_location_id}</c>.</summary>
    public Task<ApiResponse> UpdateInventoryLocationAsync(
        object inventoryLocationId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(InventoryLocationsUrls.InventoryLocation,
            new RequestOptions
            {
                PathParams = Id(inventoryLocationId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(InventoryLocation),
            }, cancellationToken);
    }

    /// <summary>Merge fields into one location - <c>PATCH /admin/inventory_locations/{inventory_location_id}</c>; what the body leaves out keeps its stored value.</summary>
    public Task<ApiResponse> PatchInventoryLocationAsync(
        object inventoryLocationId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(InventoryLocationsUrls.InventoryLocation,
            new RequestOptions
            {
                PathParams = Id(inventoryLocationId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(InventoryLocation),
            }, cancellationToken);
    }

    /// <summary>Delete a location - <c>DELETE /admin/inventory_locations/{inventory_location_id}</c>.</summary>
    public Task<ApiResponse> DeleteInventoryLocationAsync(
        object inventoryLocationId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(InventoryLocationsUrls.InventoryLocation,
            new RequestOptions
            {
                PathParams = Id(inventoryLocationId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>What one location holds - <c>GET /admin/inventory_locations/{inventory_location_id}/stock</c>.</summary>
    public Task<ApiResponse> GetInventoryLocationStockAsync(
        object inventoryLocationId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(InventoryLocationsUrls.InventoryLocationStock,
            new RequestOptions
            {
                PathParams = Id(inventoryLocationId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(InventoryLocationStock),
            }, cancellationToken);

    /// <summary>Make one location the default - <c>PUT /admin/inventory_locations/{inventory_location_id}/default</c>.</summary>
    public Task<ApiResponse> SetInventoryLocationDefaultAsync(
        object inventoryLocationId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPutRequestAsync(InventoryLocationsUrls.InventoryLocationDefault,
            new RequestOptions
            {
                PathParams = Id(inventoryLocationId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(InventoryLocation),
            }, cancellationToken);

    /// <summary>Set one location's active flag - <c>PUT /admin/inventory_locations/{inventory_location_id}/status</c>.</summary>
    public Task<ApiResponse> UpdateInventoryLocationStatusAsync(
        object inventoryLocationId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(InventoryLocationsUrls.InventoryLocationStatus,
            new RequestOptions
            {
                PathParams = Id(inventoryLocationId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(InventoryLocation),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Id(object inventoryLocationId) =>
        new() { ["inventory_location_id"] = inventoryLocationId };
}
