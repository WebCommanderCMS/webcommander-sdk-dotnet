using WebCommander.Client;

namespace WebCommander.Resources.Settings.PageSettings;

/// <summary>Product page settings - <c>/admin/settings/page_settings/product_page*</c> -
/// as a typed resource bound to one client.</summary>
public sealed class ProductPage : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.ProductPage</c>.</summary>
    public ProductPage(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/page_settings/product_page</c>.</summary>
    public Task<ApiResponse> GetProductPageAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductPageUrls.ProductPage,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductPageDto.ProductPage),
            }, cancellationToken);

    /// <summary>Write fields in one section or several - <c>PATCH /admin/settings/page_settings/product_page</c>.</summary>
    public Task<ApiResponse> PatchProductPageAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ProductPageUrls.ProductPage,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductPageDto.ProductPage),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/page_settings/product_page</c>.</summary>
    public Task<ApiResponse> HeadProductPageAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ProductPageUrls.ProductPage,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/page_settings/product_page/{section}</c>.</summary>
    public Task<ApiResponse> GetProductPageSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductPageUrls.ProductPageSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductPageDto.ProductPage),
            }, cancellationToken);

    /// <summary>Write fields in one section - <c>PATCH /admin/settings/page_settings/product_page/{section}</c>.</summary>
    public Task<ApiResponse> PatchProductPageSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ProductPageUrls.ProductPageSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductPageDto.ProductPage),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };
}
