namespace WebCommander.Resources.OrderFlags;

/// <summary>Path templates for the order flag endpoints.</summary>
public static class OrderFlagsUrls
{
    public const string OrderFlags = "/api/{api_version}/admin/order_flags";

    public const string OrderFlag = "/api/{api_version}/admin/order_flags/{order_flag_id}";
}
