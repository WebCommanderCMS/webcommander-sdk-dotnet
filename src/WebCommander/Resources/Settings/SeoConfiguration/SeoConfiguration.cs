using WebCommander.Client;
using WebCommander.Errors;
using WebCommander.Resources.Customers.AdminCustomersDto;
using WebCommander.Resources.Settings.SeoConfiguration.SeoConfigurationDto;

namespace WebCommander.Resources.Settings.SeoConfiguration;

/// <summary>SEO configuration - <c>/admin/settings/seo_configuration*</c> - as a typed resource bound to one client.</summary>
public sealed class SeoConfiguration : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.SeoConfiguration</c>.</summary>
    public SeoConfiguration(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/seo_configuration</c>.</summary>
    public Task<ApiResponse> GetSeoConfigurationAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SeoConfigurationUrls.SeoConfiguration,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SeoConfigurationDto.SeoConfiguration),
            }, cancellationToken);

    /// <summary>Write fields in one section or several - <c>PATCH /admin/settings/seo_configuration</c>.</summary>
    public Task<ApiResponse> PatchSeoConfigurationAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(SeoConfigurationUrls.SeoConfiguration,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SeoConfigurationDto.SeoConfiguration),
            }, cancellationToken);
    }

    /// <summary>Replace the sections a body names - <c>PUT /admin/settings/seo_configuration</c>; send each one whole.</summary>
    public Task<ApiResponse> PutSeoConfigurationAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(SeoConfigurationUrls.SeoConfiguration,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SeoConfigurationDto.SeoConfiguration),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/seo_configuration</c>.</summary>
    public Task<ApiResponse> HeadSeoConfigurationAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", SeoConfigurationUrls.SeoConfiguration,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/seo_configuration/{section}</c>.</summary>
    public Task<ApiResponse> GetSeoConfigurationSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SeoConfigurationUrls.SeoConfigurationSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SeoConfigurationDto.SeoConfiguration),
            }, cancellationToken);

    /// <summary>Write fields in one section - <c>PATCH /admin/settings/seo_configuration/{section}</c>.</summary>
    public Task<ApiResponse> PatchSeoConfigurationSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(SeoConfigurationUrls.SeoConfigurationSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SeoConfigurationDto.SeoConfiguration),
            }, cancellationToken);
    }

    /// <summary>Replace one section - <c>PUT /admin/settings/seo_configuration/{section}</c>; send the section whole.</summary>
    public Task<ApiResponse> PutSeoConfigurationSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(SeoConfigurationUrls.SeoConfigurationSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SeoConfigurationDto.SeoConfiguration),
            }, cancellationToken);
    }

    /// <summary>List the document root's files - <c>GET /admin/settings/seo_configuration/document_root</c>.</summary>
    public Task<ApiResponse> GetSeoConfigurationDocumentRootAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SeoConfigurationUrls.SeoConfigurationDocumentRoot,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page<DocumentRootFile>),
            }, cancellationToken);

    /// <summary>Ask whether the document root listing is reachable; no body -
    /// <c>HEAD /admin/settings/seo_configuration/document_root</c>.</summary>
    public Task<ApiResponse> HeadSeoConfigurationDocumentRootAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", SeoConfigurationUrls.SeoConfigurationDocumentRoot,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Add a file to the document root - <c>POST /admin/settings/seo_configuration/document_root</c>.
    /// The part is named <c>file</c>, and a <c>string</c> or <c>byte[]</c> is its contents, not a path.</summary>
    public Task<ApiResponse> UploadSeoConfigurationDocumentRootAsync(
        IReadOnlyDictionary<string, object?> files,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequireFiles(files);
        return Client.SendPostRequestAsync(SeoConfigurationUrls.SeoConfigurationDocumentRoot,
            new RequestOptions
            {
                Files = files,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DocumentRootFile),
            }, cancellationToken);
    }

    /// <summary>Stop serving one file - <c>DELETE /admin/settings/seo_configuration/document_root/{config_key}</c>.
    /// <paramref name="configKey"/> is the store's id for the file, not its name.</summary>
    public Task<ApiResponse> DeleteSeoConfigurationDocumentRootFileAsync(
        string configKey,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(SeoConfigurationUrls.SeoConfigurationDocumentRootFile,
            new RequestOptions
            {
                PathParams = ConfigKey(configKey),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>List the redirects - <c>GET /admin/settings/seo_configuration/redirects</c>.
    /// The rows are under <c>items</c>.</summary>
    public Task<ApiResponse> GetSeoConfigurationRedirectsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SeoConfigurationUrls.SeoConfigurationRedirects,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page<Redirect>),
            }, cancellationToken);

    /// <summary>Ask whether the redirects listing is reachable; no body -
    /// <c>HEAD /admin/settings/seo_configuration/redirects</c>.</summary>
    public Task<ApiResponse> HeadSeoConfigurationRedirectsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", SeoConfigurationUrls.SeoConfigurationRedirects,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Add a redirect - <c>POST /admin/settings/seo_configuration/redirects</c>.</summary>
    public Task<ApiResponse> CreateSeoConfigurationRedirectAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(SeoConfigurationUrls.SeoConfigurationRedirects,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Redirect),
            }, cancellationToken);
    }

    /// <summary>Read one redirect - <c>GET /admin/settings/seo_configuration/redirects/{redirect_id}</c>.</summary>
    public Task<ApiResponse> GetSeoConfigurationRedirectAsync(
        object redirectId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SeoConfigurationUrls.SeoConfigurationRedirect,
            new RequestOptions
            {
                PathParams = RedirectId(redirectId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Redirect),
            }, cancellationToken);

    /// <summary>Ask whether one redirect is there; no body, and <c>404</c> for an unknown id -
    /// <c>HEAD /admin/settings/seo_configuration/redirects/{redirect_id}</c>.</summary>
    public Task<ApiResponse> HeadSeoConfigurationRedirectAsync(
        object redirectId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", SeoConfigurationUrls.SeoConfigurationRedirect,
            new RequestOptions
            {
                PathParams = RedirectId(redirectId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Change one redirect - <c>PATCH /admin/settings/seo_configuration/redirects/{redirect_id}</c>.</summary>
    public Task<ApiResponse> PatchSeoConfigurationRedirectAsync(
        object redirectId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(SeoConfigurationUrls.SeoConfigurationRedirect,
            new RequestOptions
            {
                PathParams = RedirectId(redirectId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Redirect),
            }, cancellationToken);
    }

    /// <summary>Remove one redirect - <c>DELETE /admin/settings/seo_configuration/redirects/{redirect_id}</c>.</summary>
    public Task<ApiResponse> DeleteSeoConfigurationRedirectAsync(
        object redirectId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(SeoConfigurationUrls.SeoConfigurationRedirect,
            new RequestOptions
            {
                PathParams = RedirectId(redirectId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };

    private static Dictionary<string, object?> ConfigKey(string configKey) =>
        new() { ["config_key"] = configKey };

    private static Dictionary<string, object?> RedirectId(object redirectId) =>
        new() { ["redirect_id"] = redirectId };

    private static void RequireFiles(IReadOnlyDictionary<string, object?> files)
    {
        if (files is null || files.Count == 0)
            throw new SdkUsageError("files is required and must carry the upload: this endpoint's "
                + "whole body is the multipart part named 'file', and an empty one sends no "
                + "multipart body at all, which the store answers 400 invalid_request with field "
                + "'file', code 'required'");
    }
}
