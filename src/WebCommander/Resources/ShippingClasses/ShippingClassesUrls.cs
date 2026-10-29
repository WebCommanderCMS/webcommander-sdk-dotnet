namespace WebCommander.Resources.ShippingClasses;

/// <summary>Path templates for the shipping class endpoints.</summary>
public static class ShippingClassesUrls
{
    public const string ShippingClasses = "/api/{api_version}/admin/shipping_classes";

    public const string ShippingClassesCount = "/api/{api_version}/admin/shipping_classes/count";

    public const string ShippingClass =
        "/api/{api_version}/admin/shipping_classes/{shipping_class_id}";

    public const string ShippingClassDefault =
        "/api/{api_version}/admin/shipping_classes/{shipping_class_id}/default";
}
