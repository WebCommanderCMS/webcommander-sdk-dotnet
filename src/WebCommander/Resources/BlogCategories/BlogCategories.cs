using WebCommander.Client;
using WebCommander.Resources.BlogCategories.BlogCategoriesDto;

namespace WebCommander.Resources.BlogCategories;

/// <summary>Blog categories - <c>/admin/blog_categories*</c> - as a typed resource bound to one client.</summary>
public sealed class BlogCategories : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.BlogCategories</c>.</summary>
    public BlogCategories(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the categories, ordered by <c>name</c> - <c>GET /admin/blog_categories</c>. <c>limit</c> (capped
    /// at 20), <c>offset</c>, <c>page</c>, <c>sort</c>, <c>dir</c>, <c>field_metadata</c>; other names are <c>400</c>.</summary>
    public Task<ApiResponse> ListBlogCategoriesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(BlogCategoriesUrls.BlogCategories,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(BlogCategoryPage),
            }, cancellationToken);

    /// <summary>Create a category - <c>POST /admin/blog_categories</c>, wrapped in <c>blog_category</c>, <c>201</c>.
    /// <c>name</c> is required and unique; a <c>url</c> another row holds is stored with <c>-1</c> appended.</summary>
    public Task<ApiResponse> CreateBlogCategoryAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(BlogCategoriesUrls.BlogCategories,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(BlogCategory),
            }, cancellationToken);
    }

    /// <summary>Retrieve one category - <c>GET /admin/blog_categories/{blog_category_id}</c>.</summary>
    public Task<ApiResponse> GetBlogCategoryAsync(
        object blogCategoryId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(BlogCategoriesUrls.BlogCategory,
            new RequestOptions
            {
                PathParams = Id(blogCategoryId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(BlogCategory),
            }, cancellationToken);

    /// <summary>Update a category - <c>PUT /admin/blog_categories/{blog_category_id}</c>, wrapped as the create is.
    /// <c>name</c> is required; an omitted <c>description</c> is blanked, <c>url</c> and <c>image</c> kept.</summary>
    public Task<ApiResponse> UpdateBlogCategoryAsync(
        object blogCategoryId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(BlogCategoriesUrls.BlogCategory,
            new RequestOptions
            {
                PathParams = Id(blogCategoryId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(BlogCategory),
            }, cancellationToken);
    }

    /// <summary>Delete a category - <c>DELETE /admin/blog_categories/{blog_category_id}</c>; <c>204</c>, and the
    /// blogs linked to it are kept.</summary>
    public Task<ApiResponse> DeleteBlogCategoryAsync(
        object blogCategoryId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(BlogCategoriesUrls.BlogCategory,
            new RequestOptions
            {
                PathParams = Id(blogCategoryId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object blogCategoryId) =>
        new() { ["blog_category_id"] = blogCategoryId };
}
