using WebCommander.Client;

namespace WebCommander.Resources.Settings.SiteSettings;

/// <summary>Site settings - <c>/admin/settings/site_settings*</c> - as a typed resource bound to one client.</summary>
public sealed class SiteSettings : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.SiteSettings</c>.</summary>
    public SiteSettings(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/site_settings</c>.</summary>
    public Task<ApiResponse> GetSiteSettingsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SiteSettingsUrls.SiteSettings,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SiteSettingsDto.SiteSettings),
            }, cancellationToken);

    /// <summary>Write fields in one section or in several - <c>PATCH /admin/settings/site_settings</c>.</summary>
    public Task<ApiResponse> PatchSiteSettingsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(SiteSettingsUrls.SiteSettings,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SiteSettingsDto.SiteSettings),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/site_settings</c>.</summary>
    public Task<ApiResponse> HeadSiteSettingsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", SiteSettingsUrls.SiteSettings,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/site_settings/{section}</c>.</summary>
    public Task<ApiResponse> GetSiteSettingsSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SiteSettingsUrls.SiteSettingsSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SiteSettingsDto.SiteSettings),
            }, cancellationToken);

    /// <summary>Write fields in one section - <c>PATCH /admin/settings/site_settings/{section}</c>.</summary>
    public Task<ApiResponse> PatchSiteSettingsSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(SiteSettingsUrls.SiteSettingsSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SiteSettingsDto.SiteSettings),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };
}
