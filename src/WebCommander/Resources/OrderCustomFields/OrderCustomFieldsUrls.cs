namespace WebCommander.Resources.OrderCustomFields;

/// <summary>Path templates for the order custom field endpoints.</summary>
public static class OrderCustomFieldsUrls
{
    public const string OrderCustomFields = "/api/{api_version}/admin/order_custom_fields";

    public const string OrderCustomFieldsCount =
        "/api/{api_version}/admin/order_custom_fields/count";

    public const string OrderCustomField =
        "/api/{api_version}/admin/order_custom_fields/{order_custom_field_id}";

    public const string OrderCustomFieldValidateTimeslot =
        "/api/{api_version}/admin/order_custom_fields/{order_custom_field_id}/validate_timeslot";
}
