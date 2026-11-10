namespace WebCommander.Resources.DashboardSections;

/// <summary>Path templates for the dashboard section endpoints.</summary>
public static class DashboardSectionsUrls
{
    public const string DashboardSection = "/api/{api_version}/admin/dashboard/sections/{section}";

    /// <summary>The five <c>{section}</c> path values, the tabs of the Dashboard's <c>section_tabs</c> section.</summary>
    public static readonly IReadOnlyList<string> DashboardSections =
        new[] { "ecommerce", "content", "bookings", "courses", "blog" };
}
