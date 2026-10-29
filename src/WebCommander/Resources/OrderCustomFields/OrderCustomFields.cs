using WebCommander.Client;
using WebCommander.Resources.OrderCustomFields.OrderCustomFieldsDto;

namespace WebCommander.Resources.OrderCustomFields;

/// <summary>Order custom fields - <c>/admin/order_custom_fields*</c> - as a typed resource bound to one client.</summary>
public sealed class OrderCustomFields : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.OrderCustomFields</c>.</summary>
    public OrderCustomFields(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the fields - <c>GET /admin/order_custom_fields</c>. <c>limit</c> caps at 20; <c>sort</c> takes
    /// <c>id</c>, <c>label</c>, <c>name</c>, <c>type</c>, <c>orderNo</c>, <c>isActive</c>, <c>created</c> or <c>updated</c>.</summary>
    public Task<ApiResponse> ListOrderCustomFieldsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrderCustomFieldsUrls.OrderCustomFields,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderCustomFieldPage),
            }, cancellationToken);

    /// <summary>How many fields the store holds - <c>GET /admin/order_custom_fields/count</c>. <c>.Map</c> is
    /// <c>{"count": N}</c>; <c>filterBy</c> narrows it as it narrows the listing.</summary>
    public Task<ApiResponse> CountOrderCustomFieldsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrderCustomFieldsUrls.OrderCustomFieldsCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Retrieve one field, with nine fields its listing row leaves out; <c>field_metadata</c> adds the schema
    /// beside it - <c>GET /admin/order_custom_fields/{order_custom_field_id}</c>.</summary>
    public Task<ApiResponse> GetOrderCustomFieldAsync(
        object orderCustomFieldId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(OrderCustomFieldsUrls.OrderCustomField,
            new RequestOptions
            {
                PathParams = Id(orderCustomFieldId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(OrderCustomField),
            }, cancellationToken);

    /// <summary>Check a timeslot against an order limit; <c>201 {"valid", "message"}</c>, and nothing stored -
    /// <c>POST /admin/order_custom_fields/{order_custom_field_id}/validate_timeslot</c>, with a bare-record body.</summary>
    public Task<ApiResponse> ValidateOrderCustomFieldTimeslotAsync(
        object orderCustomFieldId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(OrderCustomFieldsUrls.OrderCustomFieldValidateTimeslot,
            new RequestOptions
            {
                PathParams = Id(orderCustomFieldId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Id(object orderCustomFieldId) =>
        new() { ["order_custom_field_id"] = orderCustomFieldId };
}
