using WebCommander.Client;
using WebCommander.Resources.Blogs.BlogsDto;

namespace WebCommander.Resources.Blogs;

/// <summary>Blogs and their comments - <c>/admin/blogs*</c> - as a typed resource bound to one client.</summary>
public sealed class Blogs : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Blogs</c>.</summary>
    public Blogs(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the blogs, latest <c>date</c> first - <c>GET /admin/blogs</c>; <c>limit</c> is capped at 20.</summary>
    public Task<ApiResponse> ListBlogsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(BlogsUrls.Blogs,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(BlogPage),
            }, cancellationToken);

    /// <summary>How many blogs are live - <c>GET /admin/blogs/count</c>, narrowed by the listing's filters.</summary>
    public Task<ApiResponse> CountBlogsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(BlogsUrls.BlogsCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(BlogCount),
            }, cancellationToken);

    /// <summary>Create a blog - <c>POST /admin/blogs</c>, answered <c>201</c>; the body is <c>{"blog": {...}}</c>.</summary>
    public Task<ApiResponse> CreateBlogAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(BlogsUrls.Blogs,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Blog),
            }, cancellationToken);
    }

    /// <summary>Retrieve one blog - <c>GET /admin/blogs/{blog_id}</c>; a trashed blog is <c>404</c>.</summary>
    public Task<ApiResponse> GetBlogAsync(
        object blogId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(BlogsUrls.Blog,
            new RequestOptions
            {
                PathParams = Id(blogId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Blog),
            }, cancellationToken);

    /// <summary>Replace one blog - <c>PUT /admin/blogs/{blog_id}</c>; every field left out but the image and <c>url</c> is reset.</summary>
    public Task<ApiResponse> UpdateBlogAsync(
        object blogId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(BlogsUrls.Blog,
            new RequestOptions
            {
                PathParams = Id(blogId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Blog),
            }, cancellationToken);
    }

    /// <summary>Change only the fields sent - <c>PATCH /admin/blogs/{blog_id}</c>.</summary>
    public Task<ApiResponse> PatchBlogAsync(
        object blogId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(BlogsUrls.Blog,
            new RequestOptions
            {
                PathParams = Id(blogId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Blog),
            }, cancellationToken);
    }

    /// <summary>Move one blog to the trash - <c>DELETE /admin/blogs/{blog_id}</c>; its title stays taken.</summary>
    public Task<ApiResponse> DeleteBlogAsync(
        object blogId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(BlogsUrls.Blog,
            new RequestOptions
            {
                PathParams = Id(blogId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Every comment on one blog, oldest first and unpaged - <c>GET /admin/blogs/{blog_id}/comments</c>.</summary>
    public Task<ApiResponse> ListBlogCommentsAsync(
        object blogId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(BlogsUrls.BlogComments,
            new RequestOptions
            {
                PathParams = Id(blogId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(BlogCommentList),
            }, cancellationToken);

    /// <summary>Add a comment, <c>pending</c> - <c>POST /admin/blogs/{blog_id}/comments</c>; the body is <c>{"blog": {"comment": {...}}}</c>.</summary>
    public Task<ApiResponse> CreateBlogCommentAsync(
        object blogId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(BlogsUrls.BlogComments,
            new RequestOptions
            {
                PathParams = Id(blogId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(BlogCommentEnvelope),
            }, cancellationToken);
    }

    /// <summary>Retrieve one comment - <c>GET /admin/blogs/{blog_id}/comments/{blog_comment_id}</c>; the top-level <c>id</c> is the blog's.</summary>
    public Task<ApiResponse> GetBlogCommentAsync(
        object blogId, object blogCommentId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(BlogsUrls.BlogComment,
            new RequestOptions
            {
                PathParams = CommentId(blogId, blogCommentId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(BlogCommentEnvelope),
            }, cancellationToken);

    /// <summary>Approve one comment - <c>POST /admin/blogs/{blog_id}/comments/{blog_comment_id}/approve</c>, no body; <c>spam</c> is left as it was.</summary>
    public Task<ApiResponse> ApproveBlogCommentAsync(
        object blogId, object blogCommentId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPostRequestAsync(BlogsUrls.BlogCommentApprove,
            new RequestOptions
            {
                PathParams = CommentId(blogId, blogCommentId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(BlogCommentEnvelope),
            }, cancellationToken);

    /// <summary>Mark one comment as spam - <c>POST /admin/blogs/{blog_id}/comments/{blog_comment_id}/mark-spam</c>, no body.</summary>
    public Task<ApiResponse> MarkBlogCommentSpamAsync(
        object blogId, object blogCommentId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPostRequestAsync(BlogsUrls.BlogCommentMarkSpam,
            new RequestOptions
            {
                PathParams = CommentId(blogId, blogCommentId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(BlogCommentEnvelope),
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object blogId) =>
        new() { ["blog_id"] = blogId };

    private static Dictionary<string, object?> CommentId(object blogId, object blogCommentId) =>
        new() { ["blog_id"] = blogId, ["blog_comment_id"] = blogCommentId };
}
