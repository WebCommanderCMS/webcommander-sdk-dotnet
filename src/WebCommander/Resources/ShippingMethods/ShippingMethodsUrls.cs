namespace WebCommander.Resources.ShippingMethods;

/// <summary>Path templates for the shipping method endpoints.</summary>
public static class ShippingMethodsUrls
{
    public const string ShippingMethods = "/api/{api_version}/admin/shipping_methods";

    /// <summary>The count sub-path.</summary>
    public const string ShippingMethodsCount = "/api/{api_version}/admin/shipping_methods/count";

    public const string ShippingMethod =
        "/api/{api_version}/admin/shipping_methods/{shipping_method_id}";

    public const string ShippingMethodDefault =
        "/api/{api_version}/admin/shipping_methods/{shipping_method_id}/default";
}
