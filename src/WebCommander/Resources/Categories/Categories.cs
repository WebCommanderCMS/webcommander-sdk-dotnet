using WebCommander.Client;
using WebCommander.Resources.Categories.CategoriesDto;

namespace WebCommander.Resources.Categories;

/// <summary>Product categories - <c>/admin/categories*</c> - as a typed resource bound to one client.</summary>
public sealed class Categories : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Categories</c>.</summary>
    public Categories(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the categories, newest update first - <c>GET /admin/categories</c>; <c>limit</c> caps at 20.
    /// <c>name</c>, <c>searchText</c> and <c>parent</c> filter, <c>sort</c> and <c>dir</c> order; unknown names are ignored.</summary>
    public Task<ApiResponse> ListCategoriesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CategoriesUrls.Categories,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CategoryPage),
            }, cancellationToken);

    /// <summary>Count the categories - <c>GET /admin/categories/count</c>; the listing's <c>name</c>,
    /// <c>searchText</c> and <c>parent</c> narrow it, and trashed rows are not counted.</summary>
    public Task<ApiResponse> CountCategoriesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CategoriesUrls.CategoriesCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CategoryCount),
            }, cancellationToken);

    /// <summary>Ask whether a name is free - <c>GET /admin/categories/check-name</c>; read <c>available</c>.
    /// Exact and case-insensitive over every level and the trash; a create refuses only a top-level row's name.</summary>
    public Task<ApiResponse> CheckCategoryNameAsync(
        string name,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CategoriesUrls.CategoriesCheckName,
            new RequestOptions
            {
                QueryParams = new Dictionary<string, object?> { ["name"] = name },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a category - <c>POST /admin/categories</c>, answered <c>201</c>. The body is wrapped in
    /// <c>category</c> and needs <c>general.name</c>; a top-level row's name, trash included, is <c>409</c>.
    /// <paramref name="transactionNo"/> is sent as <c>X-Transaction-No</c> and files the category under that number.</summary>
    public Task<ApiResponse> CreateCategoryAsync(
        object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(CategoriesUrls.Categories,
            new RequestOptions
            {
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [CategoriesUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Category),
            }, cancellationToken);
    }

    /// <summary>Retrieve one category - <c>GET /admin/categories/{category_id}</c>; a trashed one still reads
    /// <c>200</c> with <c>in_trash: true</c>. <c>field_metadata</c> adds the field schema.</summary>
    public Task<ApiResponse> GetCategoryAsync(
        object categoryId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CategoriesUrls.Category,
            new RequestOptions
            {
                PathParams = Id(categoryId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Category),
            }, cancellationToken);

    /// <summary>Replace one category - <c>PUT /admin/categories/{category_id}</c>. Omitted fields reset but
    /// <c>name</c>, <c>sku</c>, <c>url</c>, images and <c>tax_shipping</c>, though its two profiles reset.</summary>
    public Task<ApiResponse> UpdateCategoryAsync(
        object categoryId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(CategoriesUrls.Category,
            new RequestOptions
            {
                PathParams = Id(categoryId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Category),
            }, cancellationToken);
    }

    /// <summary>Merge fields into one category - <c>PATCH /admin/categories/{category_id}</c>, across as many
    /// sections as the body names; what it omits keeps its stored value.</summary>
    public Task<ApiResponse> PatchCategoryAsync(
        object categoryId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CategoriesUrls.Category,
            new RequestOptions
            {
                PathParams = Id(categoryId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Category),
            }, cancellationToken);
    }

    /// <summary>Move one category to the trash - <c>DELETE /admin/categories/{category_id}</c>; <c>204</c>.
    /// It leaves the listing and the count, and still reads <c>200</c> and holds its name.</summary>
    public Task<ApiResponse> DeleteCategoryAsync(
        object categoryId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(CategoriesUrls.Category,
            new RequestOptions
            {
                PathParams = Id(categoryId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/categories/{category_id}/{section}</c>, <paramref name="section"/>
    /// being a section name such as <c>general</c>; the answer is <c>{"category": {&lt;section&gt;: {...}}}</c>.</summary>
    public Task<ApiResponse> GetCategorySectionAsync(
        object categoryId, string section,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CategoriesUrls.CategorySection,
            new RequestOptions
            {
                PathParams = IdAndSection(categoryId, section),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Category),
            }, cancellationToken);

    /// <summary>Merge fields into one section, wrapped as <c>{"category": {&lt;section&gt;: {...}}}</c>; any other
    /// section in the body is ignored. <c>PATCH /admin/categories/{category_id}/{section}</c></summary>
    public Task<ApiResponse> PatchCategorySectionAsync(
        object categoryId, string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CategoriesUrls.CategorySection,
            new RequestOptions
            {
                PathParams = IdAndSection(categoryId, section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Category),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Id(object categoryId) =>
        new() { ["category_id"] = categoryId };

    private static Dictionary<string, object?> IdAndSection(object categoryId, string section) =>
        new() { ["category_id"] = categoryId, ["section"] = section };
}
