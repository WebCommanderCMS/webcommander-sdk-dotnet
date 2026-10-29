using WebCommander.Client;

namespace WebCommander.Resources.Settings.SearchSettings;

/// <summary>Search settings - <c>/admin/settings/search_settings*</c> - as a typed resource bound to one client.</summary>
public sealed class SearchSettings : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.SearchSettings</c>.</summary>
    public SearchSettings(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/search_settings</c>.</summary>
    public Task<ApiResponse> GetSearchSettingsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SearchSettingsUrls.SearchSettings,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SearchSettingsDto.SearchSettings),
            }, cancellationToken);

    /// <summary>Write fields in one section or in several - <c>PATCH /admin/settings/search_settings</c>.</summary>
    public Task<ApiResponse> PatchSearchSettingsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(SearchSettingsUrls.SearchSettings,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SearchSettingsDto.SearchSettings),
            }, cancellationToken);
    }

    /// <summary>Send the whole area - <c>PUT /admin/settings/search_settings</c>.</summary>
    public Task<ApiResponse> PutSearchSettingsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(SearchSettingsUrls.SearchSettings,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SearchSettingsDto.SearchSettings),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/search_settings</c>.</summary>
    public Task<ApiResponse> HeadSearchSettingsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", SearchSettingsUrls.SearchSettings,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/search_settings/{section}</c>.</summary>
    public Task<ApiResponse> GetSearchSettingsSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SearchSettingsUrls.SearchSettingsSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SearchSettingsDto.SearchSettings),
            }, cancellationToken);

    /// <summary>Write fields in one section - <c>PATCH /admin/settings/search_settings/{section}</c>.</summary>
    public Task<ApiResponse> PatchSearchSettingsSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(SearchSettingsUrls.SearchSettingsSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SearchSettingsDto.SearchSettings),
            }, cancellationToken);
    }

    /// <summary>Replace one section - <c>PUT /admin/settings/search_settings/{section}</c>; send the whole section.</summary>
    public Task<ApiResponse> PutSearchSettingsSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(SearchSettingsUrls.SearchSettingsSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SearchSettingsDto.SearchSettings),
            }, cancellationToken);
    }

    /// <summary>Read where the reindex stands - <c>GET /admin/settings/search_settings/reindex_status</c>.</summary>
    public Task<ApiResponse> GetSearchSettingsReindexStatusAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SearchSettingsUrls.SearchSettingsReindexStatus,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SearchSettingsDto.ReindexStatus),
            }, cancellationToken);

    /// <summary>Ask whether the reindex status is reachable; no body -
    /// <c>HEAD /admin/settings/search_settings/reindex_status</c>.</summary>
    public Task<ApiResponse> HeadSearchSettingsReindexStatusAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", SearchSettingsUrls.SearchSettingsReindexStatus,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };
}
