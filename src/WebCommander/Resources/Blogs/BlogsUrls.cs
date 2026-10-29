namespace WebCommander.Resources.Blogs;

/// <summary>Path templates for the blog endpoints.</summary>
public static class BlogsUrls
{
    public const string Blogs = "/api/{api_version}/admin/blogs";

    public const string BlogsCount = "/api/{api_version}/admin/blogs/count";

    public const string Blog = "/api/{api_version}/admin/blogs/{blog_id}";

    public const string BlogComments = "/api/{api_version}/admin/blogs/{blog_id}/comments";

    public const string BlogComment =
        "/api/{api_version}/admin/blogs/{blog_id}/comments/{blog_comment_id}";

    public const string BlogCommentApprove =
        "/api/{api_version}/admin/blogs/{blog_id}/comments/{blog_comment_id}/approve";

    public const string BlogCommentMarkSpam =
        "/api/{api_version}/admin/blogs/{blog_id}/comments/{blog_comment_id}/mark-spam";
}
