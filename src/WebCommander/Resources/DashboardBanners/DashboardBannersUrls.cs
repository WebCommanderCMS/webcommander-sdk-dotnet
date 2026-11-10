namespace WebCommander.Resources.DashboardBanners;

/// <summary>Path templates for the dashboard banner endpoints.</summary>
public static class DashboardBannersUrls
{
    public const string DashboardBanners = "/api/{api_version}/admin/dashboard/banners";

    public const string DashboardBanner = "/api/{api_version}/admin/dashboard/banners/{banner_key}";

    public const string DashboardContentInfo = "/api/{api_version}/admin/dashboard/content_info";
}
