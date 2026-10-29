namespace WebCommander.Resources.Settings.CustomerPortal;

/// <summary>Path templates for the customer portal endpoints.</summary>
public static class CustomerPortalUrls
{
    public const string CustomerPortal =
        "/api/{api_version}/admin/settings/customer_portal";

    public const string CustomerPortalSection =
        "/api/{api_version}/admin/settings/customer_portal/{section}";
}
