using WebCommander.Client;

namespace WebCommander.Resources.Settings.PriceDisplaySettings;

/// <summary>Price display settings - <c>/admin/settings/price_display_settings*</c> -
/// as a typed resource bound to one client.</summary>
public sealed class PriceDisplaySettings : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.PriceDisplaySettings</c>.</summary>
    public PriceDisplaySettings(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/price_display_settings</c>.</summary>
    public Task<ApiResponse> GetPriceDisplaySettingsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(PriceDisplaySettingsUrls.PriceDisplaySettings,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PriceDisplaySettingsDto.PriceDisplaySettings),
            }, cancellationToken);

    /// <summary>Write fields in one section or both - <c>PATCH /admin/settings/price_display_settings</c>.</summary>
    public Task<ApiResponse> PatchPriceDisplaySettingsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(PriceDisplaySettingsUrls.PriceDisplaySettings,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PriceDisplaySettingsDto.PriceDisplaySettings),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/price_display_settings</c>.</summary>
    public Task<ApiResponse> HeadPriceDisplaySettingsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", PriceDisplaySettingsUrls.PriceDisplaySettings,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/price_display_settings/{section}</c>.</summary>
    public Task<ApiResponse> GetPriceDisplaySettingsSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(PriceDisplaySettingsUrls.PriceDisplaySettingsSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PriceDisplaySettingsDto.PriceDisplaySettings),
            }, cancellationToken);

    /// <summary>Write fields in one section - <c>PATCH /admin/settings/price_display_settings/{section}</c>.</summary>
    public Task<ApiResponse> PatchPriceDisplaySettingsSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(PriceDisplaySettingsUrls.PriceDisplaySettingsSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PriceDisplaySettingsDto.PriceDisplaySettings),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };
}
