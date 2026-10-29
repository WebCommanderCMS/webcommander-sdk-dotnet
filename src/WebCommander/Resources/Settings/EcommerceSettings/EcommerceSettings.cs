using WebCommander.Client;

namespace WebCommander.Resources.Settings.EcommerceSettings;

/// <summary>Ecommerce settings - <c>/admin/settings/ecommerce_settings*</c> - as a typed resource bound to one client.</summary>
public sealed class EcommerceSettings : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.EcommerceSettings</c>.</summary>
    public EcommerceSettings(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/ecommerce_settings</c>.</summary>
    public Task<ApiResponse> GetEcommerceSettingsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(EcommerceSettingsUrls.EcommerceSettings,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(EcommerceSettingsDto.EcommerceSettings),
            }, cancellationToken);

    /// <summary>Write fields in one section or in several - <c>PATCH /admin/settings/ecommerce_settings</c>.</summary>
    public Task<ApiResponse> PatchEcommerceSettingsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(EcommerceSettingsUrls.EcommerceSettings,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(EcommerceSettingsDto.EcommerceSettings),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/ecommerce_settings</c>.</summary>
    public Task<ApiResponse> HeadEcommerceSettingsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", EcommerceSettingsUrls.EcommerceSettings,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/ecommerce_settings/{section}</c>.</summary>
    public Task<ApiResponse> GetEcommerceSettingsSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(EcommerceSettingsUrls.EcommerceSettingsSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(EcommerceSettingsDto.EcommerceSettings),
            }, cancellationToken);

    /// <summary>Write fields in one section - <c>PATCH /admin/settings/ecommerce_settings/{section}</c>.</summary>
    public Task<ApiResponse> PatchEcommerceSettingsSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(EcommerceSettingsUrls.EcommerceSettingsSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(EcommerceSettingsDto.EcommerceSettings),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };
}
