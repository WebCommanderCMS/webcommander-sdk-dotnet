using WebCommander.Client;

namespace WebCommander.Resources.Settings.ShippingSettings;

/// <summary>Shipping settings - <c>/admin/settings/shipping_settings</c> - as a typed resource bound to one client.</summary>
public sealed class ShippingSettings : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.ShippingSettings</c>.</summary>
    public ShippingSettings(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/shipping_settings</c>.</summary>
    public Task<ApiResponse> GetShippingSettingsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingSettingsUrls.ShippingSettings,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingSettingsDto.ShippingSettings),
            }, cancellationToken);

    /// <summary>Write one field or several - <c>PATCH /admin/settings/shipping_settings</c>.</summary>
    public Task<ApiResponse> PatchShippingSettingsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ShippingSettingsUrls.ShippingSettings,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingSettingsDto.ShippingSettings),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/shipping_settings</c>.</summary>
    public Task<ApiResponse> HeadShippingSettingsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingSettingsUrls.ShippingSettings,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
}
