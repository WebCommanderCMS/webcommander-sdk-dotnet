using WebCommander.Client;

namespace WebCommander.Resources.Settings.PageSettings;

/// <summary>Category page settings - <c>/admin/settings/page_settings/category_page*</c> -
/// as a typed resource bound to one client.</summary>
public sealed class CategoryPage : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.CategoryPage</c>.</summary>
    public CategoryPage(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/page_settings/category_page</c>.</summary>
    public Task<ApiResponse> GetCategoryPageAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CategoryPageUrls.CategoryPage,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CategoryPageDto.CategoryPage),
            }, cancellationToken);

    /// <summary>Write fields in one section or both - <c>PATCH /admin/settings/page_settings/category_page</c>.</summary>
    public Task<ApiResponse> PatchCategoryPageAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CategoryPageUrls.CategoryPage,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CategoryPageDto.CategoryPage),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/page_settings/category_page</c>.</summary>
    public Task<ApiResponse> HeadCategoryPageAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CategoryPageUrls.CategoryPage,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/page_settings/category_page/{section}</c>.</summary>
    public Task<ApiResponse> GetCategoryPageSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CategoryPageUrls.CategoryPageSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CategoryPageDto.CategoryPage),
            }, cancellationToken);

    /// <summary>Write fields in one section - <c>PATCH /admin/settings/page_settings/category_page/{section}</c>.</summary>
    public Task<ApiResponse> PatchCategoryPageSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CategoryPageUrls.CategoryPageSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CategoryPageDto.CategoryPage),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };
}
