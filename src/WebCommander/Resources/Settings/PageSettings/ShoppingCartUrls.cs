namespace WebCommander.Resources.Settings.PageSettings;

/// <summary>Path templates for the shopping cart settings endpoints.</summary>
public static class ShoppingCartUrls
{
    public const string ShoppingCart =
        "/api/{api_version}/admin/settings/page_settings/shopping_cart";

    public const string ShoppingCartSection =
        "/api/{api_version}/admin/settings/page_settings/shopping_cart/{section}";
}
