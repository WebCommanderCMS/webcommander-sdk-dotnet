using WebCommander.Errors;

namespace WebCommander.Client;

/// <summary>Reaching an endpoint group by its registered name; explicit, so a typo raises.</summary>
public static class ResourceBinding
{
    private static readonly object Gate = new();

    private static readonly Dictionary<string, Func<WebCommanderClient, object>> Modules =
        new(StringComparer.Ordinal)
        {
            ["admin_customers"] = client => client.AdminCustomers,
            ["aliases"] = client => client.Aliases,
            ["animation_effects"] = client => client.AnimationEffects,
            ["blog_categories"] = client => client.BlogCategories,
            ["blogs"] = client => client.Blogs,
            ["carts"] = client => client.Carts,
            ["categories"] = client => client.Categories,
            ["category_page"] = client => client.CategoryPage,
            ["checkout_page"] = client => client.CheckoutPage,
            ["cities"] = client => client.Cities,
            ["common_settings"] = client => client.CommonSettings,
            ["countries"] = client => client.Countries,
            ["currencies"] = client => client.Currencies,
            ["customer_groups"] = client => client.CustomerGroups,
            ["customer_portal"] = client => client.CustomerPortal,
            ["discount_profiles"] = client => client.DiscountProfiles,
            ["ecommerce_settings"] = client => client.EcommerceSettings,
            ["email"] = client => client.Email,
            ["inventory_locations"] = client => client.InventoryLocations,
            ["order_custom_fields"] = client => client.OrderCustomFields,
            ["order_flags"] = client => client.OrderFlags,
            ["orders"] = client => client.Orders,
            ["pages"] = client => client.Pages,
            ["payment_gateways"] = client => client.PaymentGateways,
            ["price_display_settings"] = client => client.PriceDisplaySettings,
            ["product_listing"] = client => client.ProductListing,
            ["product_page"] = client => client.ProductPage,
            ["products"] = client => client.Products,
            ["search_result_page"] = client => client.SearchResultPage,
            ["search_settings"] = client => client.SearchSettings,
            ["seo_configuration"] = client => client.SeoConfiguration,
            ["shipping_classes"] = client => client.ShippingClasses,
            ["shipping_methods"] = client => client.ShippingMethods,
            ["shipping_settings"] = client => client.ShippingSettings,
            ["shopping_cart"] = client => client.ShoppingCart,
            ["site_settings"] = client => client.SiteSettings,
            ["tax_codes"] = client => client.TaxCodes,
            ["tax_country_taxes"] = client => client.TaxCountryTaxes,
            ["tax_custom_profiles"] = client => client.TaxCustomProfiles,
            ["tax_profiles"] = client => client.TaxProfiles,
            ["tax_rules"] = client => client.TaxRules,
            ["tax_settings"] = client => client.TaxSettings,
            ["tax_zones"] = client => client.TaxZones,
            ["transactions"] = client => client.Transactions,
            ["trash"] = client => client.Trash,
            ["variations"] = client => client.Variations,
        };

    /// <summary>Register a resource factory under a lower snake_case name, refusing any collision.</summary>
    public static void RegisterResourceModule(
        string resourceName, Func<WebCommanderClient, object> factory)
    {
        if (string.IsNullOrEmpty(resourceName) || resourceName[0] == '_'
            || !resourceName.All(character =>
                character is >= 'a' and <= 'z' or >= '0' and <= '9' or '_'))
            throw new SdkUsageError(Redaction.RedactMessageText(
                $"cannot register resource '{resourceName}': a resource name must be lower "
                + "snake_case and must not start with an underscore - that is the "
                + "vocabulary the resource list is written in"));

        if (ClientMemberNames.Contains(Normalise(resourceName)))
            throw new SdkUsageError(Redaction.RedactMessageText(
                $"cannot register resource '{resourceName}': WebCommanderClient already "
                + "carries a member of that name, so a caller reaching for the group "
                + "would get the member instead - pick another name"));

        ArgumentNullException.ThrowIfNull(factory);

        lock (Gate)
        {
            if (Modules.TryGetValue(resourceName, out var registered) && registered != factory)
                throw new SdkUsageError(Redaction.RedactMessageText(
                    $"cannot register resource '{resourceName}': it is already registered "
                    + "against a different factory, and a client that resolved the old one "
                    + "keeps it - pick another name"));
            Modules[resourceName] = factory;
        }
    }

    /// <summary>Every registered resource name, ordinally sorted.</summary>
    public static IReadOnlyList<string> RegisteredResourceNames()
    {
        lock (Gate)
            return Modules.Keys.OrderBy(name => name, StringComparer.Ordinal).ToList();
    }

    /// <summary>The factory registered under a name, never <c>null</c>.</summary>
    public static Func<WebCommanderClient, object> ResolveResourceModule(string resourceName)
    {
        lock (Gate)
        {
            if (Modules.TryGetValue(resourceName, out var factory))
                return factory;
            var registered = string.Join(", ", RegisteredResourceNames());
            throw new SdkAttributeError(Redaction.RedactMessageText(
                $"no resource named '{resourceName}'; registered: {registered}"));
        }
    }

    private static readonly HashSet<string> ClientMemberNames =
        typeof(WebCommanderClient)
            .GetMembers()
            .Select(member => Normalise(member.Name))
            .ToHashSet(StringComparer.Ordinal);

    private static string Normalise(string name) =>
        name.Replace("_", string.Empty, StringComparison.Ordinal).ToLowerInvariant();
}
