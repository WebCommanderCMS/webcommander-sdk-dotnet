using WebCommander.Client;
using WebCommander.Resources.Pages.PagesDto;

namespace WebCommander.Resources.Pages;

/// <summary>Content pages - <c>/admin/pages*</c> - as a typed resource bound to one client.</summary>
public sealed class Pages : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Pages</c>.</summary>
    public Pages(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the pages outside the trash, newest change first - <c>GET /admin/pages</c>. <c>search</c>,
    /// <c>active</c>, <c>has_layout</c>, <c>visibility</c>, <c>created_from</c>/<c>created_to</c>,
    /// <c>updated_from</c>/<c>updated_to</c> (<c>YYYY-MM-DD</c>), <c>author</c>, <c>store</c> and
    /// <c>not_in_store</c> filter; <c>sort</c> and <c>order</c> order; <c>limit</c> is 1 to 100 or <c>all</c>.
    /// Unknown names are <c>400</c>.</summary>
    public Task<ApiResponse> ListPagesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(PagesUrls.Pages,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PageListing),
            }, cancellationToken);

    /// <summary>HEAD the page collection - <c>HEAD /admin/pages</c>; no body.</summary>
    public Task<ApiResponse> HeadPagesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", PagesUrls.Pages,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Count the pages - <c>GET /admin/pages/count</c>, a bare <c>{"total": N}</c>. The listing's filters
    /// narrow it; <c>sort</c>, <c>order</c>, <c>limit</c> and <c>offset</c> are ignored.</summary>
    public Task<ApiResponse> CountPagesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(PagesUrls.PagesCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PageCount),
            }, cancellationToken);

    /// <summary>HEAD the count - <c>HEAD /admin/pages/count</c>; no body.</summary>
    public Task<ApiResponse> HeadPagesCountAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", PagesUrls.PagesCount,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a page - <c>POST /admin/pages</c> with <c>{"page": {...}}</c>, answered <c>201</c>.
    /// <c>name</c> and <c>visibility</c> are required; the URL is made from the name.</summary>
    public Task<ApiResponse> CreatePageAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(PagesUrls.Pages,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page),
            }, cancellationToken);
    }

    /// <summary>Retrieve one page, with an <c>ETag</c>; a page in the trash still reads, <c>in_trash</c> true.
    /// <c>GET /admin/pages/{page_id}</c></summary>
    public Task<ApiResponse> GetPageAsync(
        object pageId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(PagesUrls.Page,
            new RequestOptions
            {
                PathParams = Id(pageId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page),
            }, cancellationToken);

    /// <summary>HEAD one page - <c>HEAD /admin/pages/{page_id}</c>; no body, the <c>ETag</c> its <c>GET</c>
    /// carries.</summary>
    public Task<ApiResponse> HeadPageAsync(
        object pageId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", PagesUrls.Page,
            new RequestOptions
            {
                PathParams = Id(pageId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Replace one page - <c>PUT /admin/pages/{page_id}</c>. <c>name</c>, <c>title</c>, <c>subtitle</c>,
    /// <c>url</c>, <c>layout</c>, <c>visibility</c>, <c>body_class</c>, <c>active</c>, <c>disable_tracking</c> and
    /// <c>password_protection</c> are required, a partial body is <c>400</c>. <paramref name="ifMatch"/> is sent
    /// as <c>If-Match</c>: a stale <c>ETag</c> is <c>412</c>, checked before the body.</summary>
    public Task<ApiResponse> UpdatePageAsync(
        object pageId, object payload, string? ifMatch = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(PagesUrls.Page,
            new RequestOptions
            {
                PathParams = Id(pageId),
                JsonBody = payload,
                Headers = ifMatch is null
                    ? null
                    : new Dictionary<string, string> { [PagesUrls.IfMatchHeader] = ifMatch },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page),
            }, cancellationToken);
    }

    /// <summary>Change only the fields sent - <c>PATCH /admin/pages/{page_id}</c>; an empty <c>page</c> is
    /// <c>400</c> and a page in the trash <c>409</c>. <paramref name="ifMatch"/> is sent as <c>If-Match</c>.</summary>
    public Task<ApiResponse> PatchPageAsync(
        object pageId, object payload, string? ifMatch = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(PagesUrls.Page,
            new RequestOptions
            {
                PathParams = Id(pageId),
                JsonBody = payload,
                Headers = ifMatch is null
                    ? null
                    : new Dictionary<string, string> { [PagesUrls.IfMatchHeader] = ifMatch },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page),
            }, cancellationToken);
    }

    /// <summary>Move a page to the trash - <c>DELETE /admin/pages/{page_id}</c>; <c>204</c>. <c>force</c>
    /// <c>true</c> purges it, from the trash or live; a second plain delete is <c>400</c>. The landing page is
    /// <c>409</c>. <paramref name="ifMatch"/> is sent as <c>If-Match</c>.</summary>
    public Task<ApiResponse> DeletePageAsync(
        object pageId,
        IReadOnlyDictionary<string, object?>? queryParams = null, string? ifMatch = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(PagesUrls.Page,
            new RequestOptions
            {
                PathParams = Id(pageId),
                QueryParams = queryParams,
                Headers = ifMatch is null
                    ? null
                    : new Dictionary<string, string> { [PagesUrls.IfMatchHeader] = ifMatch },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Move a page to a new URL - <c>PUT /admin/pages/{page_id}/url</c> with a bare
    /// <c>{"url": ...}</c>; answers the page. A URL another page holds is <c>409</c>.</summary>
    public Task<ApiResponse> UpdatePageUrlAsync(
        object pageId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(PagesUrls.PageUrl,
            new RequestOptions
            {
                PathParams = Id(pageId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page),
            }, cancellationToken);
    }

    /// <summary>Make a page the store's landing page; takes no body. The previous landing page stops being it.
    /// <c>PATCH /admin/pages/{page_id}/landing</c></summary>
    public Task<ApiResponse> SetLandingPageAsync(
        object pageId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPatchRequestAsync(PagesUrls.PageLanding,
            new RequestOptions
            {
                PathParams = Id(pageId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page),
            }, cancellationToken);

    /// <summary>Whether a delete would pass and what blocks it, such as <c>landing_page</c>.
    /// <c>GET /admin/pages/{page_id}/delete-blockers</c></summary>
    public Task<ApiResponse> GetPageDeleteBlockersAsync(
        object pageId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(PagesUrls.PageDeleteBlockers,
            new RequestOptions
            {
                PathParams = Id(pageId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PageDeleteBlockers),
            }, cancellationToken);

    /// <summary>HEAD the delete check - <c>HEAD /admin/pages/{page_id}/delete-blockers</c>; no body.</summary>
    public Task<ApiResponse> HeadPageDeleteBlockersAsync(
        object pageId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", PagesUrls.PageDeleteBlockers,
            new RequestOptions
            {
                PathParams = Id(pageId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Copy a page - <c>POST /admin/pages/{page_id}/duplicate</c>, answered <c>201</c>; a body field is
    /// <c>400</c>. The copy is <c>"&lt;name&gt; - Copy"</c> at <c>&lt;url&gt;-copy</c>, then <c>- Copy 1</c> at
    /// <c>-copy-1</c>.</summary>
    public Task<ApiResponse> DuplicatePageAsync(
        object pageId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPostRequestAsync(PagesUrls.PageDuplicate,
            new RequestOptions
            {
                PathParams = Id(pageId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page),
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object pageId) =>
        new() { ["page_id"] = pageId };
}
