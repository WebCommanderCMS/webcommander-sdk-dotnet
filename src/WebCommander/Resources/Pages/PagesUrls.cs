namespace WebCommander.Resources.Pages;

/// <summary>Path templates and the <c>If-Match</c> header for the page endpoints.</summary>
public static class PagesUrls
{
    public const string Pages = "/api/{api_version}/admin/pages";

    public const string PagesCount = "/api/{api_version}/admin/pages/count";

    public const string Page = "/api/{api_version}/admin/pages/{page_id}";

    public const string PageUrl = "/api/{api_version}/admin/pages/{page_id}/url";

    public const string PageLanding = "/api/{api_version}/admin/pages/{page_id}/landing";

    public const string PageDeleteBlockers = "/api/{api_version}/admin/pages/{page_id}/delete-blockers";

    public const string PageDuplicate = "/api/{api_version}/admin/pages/{page_id}/duplicate";

    /// <summary>Carries the <c>ETag</c> a read answered; a stale one makes the write <c>412</c>.</summary>
    public const string IfMatchHeader = "If-Match";
}
