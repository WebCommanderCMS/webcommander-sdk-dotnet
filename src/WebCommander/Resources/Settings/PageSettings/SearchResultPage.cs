using WebCommander.Client;
using WebCommander.Resources.Customers.AdminCustomersDto;
using WebCommander.Resources.Settings.PageSettings.SearchResultPageDto;

namespace WebCommander.Resources.Settings.PageSettings;

/// <summary>Search result page settings - <c>/admin/settings/page_settings/search_result_page*</c> -
/// as a typed resource bound to one client.</summary>
public sealed class SearchResultPage : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.SearchResultPage</c>.</summary>
    public SearchResultPage(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/page_settings/search_result_page</c>.</summary>
    public Task<ApiResponse> GetSearchResultPageAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SearchResultPageUrls.SearchResultPage,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SearchResultPageDto.SearchResultPage),
            }, cancellationToken);

    /// <summary>Write fields in the writable section - <c>PATCH /admin/settings/page_settings/search_result_page</c>.</summary>
    public Task<ApiResponse> PatchSearchResultPageAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(SearchResultPageUrls.SearchResultPage,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SearchResultPageDto.SearchResultPage),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/page_settings/search_result_page</c>.</summary>
    public Task<ApiResponse> HeadSearchResultPageAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", SearchResultPageUrls.SearchResultPage,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/page_settings/search_result_page/{section}</c>.</summary>
    public Task<ApiResponse> GetSearchResultPageSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SearchResultPageUrls.SearchResultPageSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SearchResultPageDto.SearchResultPage),
            }, cancellationToken);

    /// <summary>Write fields in one section - <c>PATCH /admin/settings/page_settings/search_result_page/{section}</c>.</summary>
    public Task<ApiResponse> PatchSearchResultPageSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(SearchResultPageUrls.SearchResultPageSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SearchResultPageDto.SearchResultPage),
            }, cancellationToken);
    }

    /// <summary>List the promotions - <c>GET /admin/settings/page_settings/search_result_page/product_promotion</c>.
    /// The rows are under <c>product_promotions</c>.</summary>
    public Task<ApiResponse> GetSearchResultPageProductPromotionsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SearchResultPageUrls.SearchResultPageProductPromotions,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page<ProductPromotion>),
            }, cancellationToken);

    /// <summary>Ask whether the promotions collection is reachable; no body -
    /// <c>HEAD /admin/settings/page_settings/search_result_page/product_promotion</c>.</summary>
    public Task<ApiResponse> HeadSearchResultPageProductPromotionsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", SearchResultPageUrls.SearchResultPageProductPromotions,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Add a promotion - <c>POST /admin/settings/page_settings/search_result_page/product_promotion</c>.</summary>
    public Task<ApiResponse> CreateSearchResultPageProductPromotionAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(SearchResultPageUrls.SearchResultPageProductPromotions,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductPromotion),
            }, cancellationToken);
    }

    /// <summary>Read one promotion -
    /// <c>GET /admin/settings/page_settings/search_result_page/product_promotion/{promotion_id}</c>.</summary>
    public Task<ApiResponse> GetSearchResultPageProductPromotionAsync(
        object promotionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(SearchResultPageUrls.SearchResultPageProductPromotion,
            new RequestOptions
            {
                PathParams = PromotionId(promotionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductPromotion),
            }, cancellationToken);

    /// <summary>Ask whether one promotion is there; no body, an <c>ETag</c>, and <c>404</c> for an unknown id -
    /// <c>HEAD /admin/settings/page_settings/search_result_page/product_promotion/{promotion_id}</c>.</summary>
    public Task<ApiResponse> HeadSearchResultPageProductPromotionAsync(
        object promotionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", SearchResultPageUrls.SearchResultPageProductPromotion,
            new RequestOptions
            {
                PathParams = PromotionId(promotionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Change one promotion -
    /// <c>PATCH /admin/settings/page_settings/search_result_page/product_promotion/{promotion_id}</c>.</summary>
    public Task<ApiResponse> PatchSearchResultPageProductPromotionAsync(
        object promotionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(SearchResultPageUrls.SearchResultPageProductPromotion,
            new RequestOptions
            {
                PathParams = PromotionId(promotionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductPromotion),
            }, cancellationToken);
    }

    /// <summary>Turn one promotion on or off -
    /// <c>PATCH /admin/settings/page_settings/search_result_page/product_promotion/{promotion_id}/active</c>.</summary>
    public Task<ApiResponse> PatchSearchResultPageProductPromotionActiveAsync(
        object promotionId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(
            SearchResultPageUrls.SearchResultPageProductPromotionActive,
            new RequestOptions
            {
                PathParams = PromotionId(promotionId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductPromotion),
            }, cancellationToken);
    }

    /// <summary>Remove one promotion -
    /// <c>DELETE /admin/settings/page_settings/search_result_page/product_promotion/{promotion_id}</c>.</summary>
    public Task<ApiResponse> DeleteSearchResultPageProductPromotionAsync(
        object promotionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(SearchResultPageUrls.SearchResultPageProductPromotion,
            new RequestOptions
            {
                PathParams = PromotionId(promotionId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };

    private static Dictionary<string, object?> PromotionId(object promotionId) =>
        new() { ["promotion_id"] = promotionId };
}
