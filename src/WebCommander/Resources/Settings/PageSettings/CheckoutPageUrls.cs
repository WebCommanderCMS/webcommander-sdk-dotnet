namespace WebCommander.Resources.Settings.PageSettings;

/// <summary>Path templates for the checkout page settings endpoints.</summary>
public static class CheckoutPageUrls
{
    public const string CheckoutPage =
        "/api/{api_version}/admin/settings/page_settings/checkout_page";

    public const string CheckoutPageSection =
        "/api/{api_version}/admin/settings/page_settings/checkout_page/{section}";
}
