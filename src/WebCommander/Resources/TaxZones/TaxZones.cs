using WebCommander.Client;
using WebCommander.Resources.TaxZones.TaxZonesDto;

namespace WebCommander.Resources.TaxZones;

/// <summary>Tax zones - <c>/admin/tax_zones</c> - as a typed resource bound to one client.</summary>
public sealed class TaxZones : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.TaxZones</c>.</summary>
    public TaxZones(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the tax zones - <c>GET /admin/tax_zones</c>.</summary>
    public Task<ApiResponse> ListTaxZonesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxZonesUrls.TaxZones,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxZonePage),
            }, cancellationToken);

    /// <summary>HEAD the tax zone collection - <c>HEAD /admin/tax_zones</c>.</summary>
    public Task<ApiResponse> HeadTaxZonesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TaxZonesUrls.TaxZones,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a tax zone - <c>POST /admin/tax_zones</c>.</summary>
    public Task<ApiResponse> CreateTaxZoneAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(TaxZonesUrls.TaxZones,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxZone),
            }, cancellationToken);
    }

    /// <summary>Retrieve one tax zone - <c>GET /admin/tax_zones/{tax_zone_id}</c>.</summary>
    public Task<ApiResponse> GetTaxZoneAsync(
        object taxZoneId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxZonesUrls.TaxZone,
            new RequestOptions
            {
                PathParams = Id(taxZoneId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxZone),
            }, cancellationToken);

    /// <summary>HEAD one tax zone - <c>HEAD /admin/tax_zones/{tax_zone_id}</c>.</summary>
    public Task<ApiResponse> HeadTaxZoneAsync(
        object taxZoneId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TaxZonesUrls.TaxZone,
            new RequestOptions
            {
                PathParams = Id(taxZoneId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Update one tax zone - <c>PUT /admin/tax_zones/{tax_zone_id}</c>; it replaces the lists.</summary>
    public Task<ApiResponse> UpdateTaxZoneAsync(
        object taxZoneId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(TaxZonesUrls.TaxZone,
            new RequestOptions
            {
                PathParams = Id(taxZoneId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxZone),
            }, cancellationToken);
    }

    /// <summary>Delete one tax zone - <c>DELETE /admin/tax_zones/{tax_zone_id}</c>.</summary>
    public Task<ApiResponse> DeleteTaxZoneAsync(
        object taxZoneId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(TaxZonesUrls.TaxZone,
            new RequestOptions
            {
                PathParams = Id(taxZoneId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object taxZoneId) =>
        new() { ["tax_zone_id"] = taxZoneId };
}
