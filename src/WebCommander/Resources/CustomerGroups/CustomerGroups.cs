using WebCommander.Client;
using WebCommander.Resources.CustomerGroups.CustomerGroupsDto;

namespace WebCommander.Resources.CustomerGroups;

/// <summary>Customer groups - <c>/admin/customer_groups*</c> - as a typed resource bound to one client.</summary>
public sealed class CustomerGroups : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.CustomerGroups</c>.</summary>
    public CustomerGroups(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the groups, by <c>name</c> - <c>GET /admin/customer_groups</c>. <c>limit</c> 1 to 100, default
    /// 20; <c>search_text</c> or <c>name</c> match a name, <c>status</c> takes <c>A</c>, <c>I</c> or their words.</summary>
    public Task<ApiResponse> ListCustomerGroupsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CustomerGroupsUrls.CustomerGroups,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerGroupPage),
            }, cancellationToken);

    /// <summary>How many groups match - <c>GET /admin/customer_groups/count</c>; <c>{"total": N}</c>, narrowed by
    /// the listing's <c>search_text</c>, <c>name</c> and <c>status</c> and not by its paging.</summary>
    public Task<ApiResponse> CountCustomerGroupsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CustomerGroupsUrls.CustomerGroupsCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerGroupCount),
            }, cancellationToken);

    /// <summary>Create a group - <c>POST /admin/customer_groups</c>, answered <c>201</c>. The body is
    /// <c>{"customer_group": {...}}</c> with <c>name</c> required, unique in any letter case.</summary>
    public Task<ApiResponse> CreateCustomerGroupAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(CustomerGroupsUrls.CustomerGroups,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerGroup),
            }, cancellationToken);
    }

    /// <summary>Retrieve one group with <c>customer_count</c> and its <c>customers</c>; <c>field_metadata=true</c>
    /// adds the schema - <c>GET /admin/customer_groups/{customer_group_id}</c>.</summary>
    public Task<ApiResponse> GetCustomerGroupAsync(
        object customerGroupId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CustomerGroupsUrls.CustomerGroup,
            new RequestOptions
            {
                PathParams = Id(customerGroupId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerGroup),
            }, cancellationToken);

    /// <summary>Change only the fields sent, nested ones included, in the <c>customer_group</c> wrapper; <c>PUT</c>
    /// is <c>405</c> - <c>PATCH /admin/customer_groups/{customer_group_id}</c>.</summary>
    public Task<ApiResponse> PatchCustomerGroupAsync(
        object customerGroupId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CustomerGroupsUrls.CustomerGroup,
            new RequestOptions
            {
                PathParams = Id(customerGroupId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerGroup),
            }, cancellationToken);
    }

    /// <summary>Delete one group and its memberships - <c>DELETE /admin/customer_groups/{customer_group_id}</c>;
    /// <c>204</c>, and the members stay as customers.</summary>
    public Task<ApiResponse> DeleteCustomerGroupAsync(
        object customerGroupId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(CustomerGroupsUrls.CustomerGroup,
            new RequestOptions
            {
                PathParams = Id(customerGroupId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Replace the member list with <c>{"customers": [customer_id, ...]}</c>; <c>[]</c> empties it -
    /// <c>PATCH /admin/customer_groups/{customer_group_id}/customers</c>.</summary>
    public Task<ApiResponse> UpdateCustomerGroupCustomersAsync(
        object customerGroupId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CustomerGroupsUrls.CustomerGroupCustomers,
            new RequestOptions
            {
                PathParams = Id(customerGroupId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerGroup),
            }, cancellationToken);
    }

    /// <summary>Read one member by its <c>customer_id</c>; a customer outside the group is <c>404</c> -
    /// <c>GET /admin/customer_groups/{customer_group_id}/customers/{customer_id}</c>.</summary>
    public Task<ApiResponse> GetCustomerGroupCustomerAsync(
        object customerGroupId, object customerId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CustomerGroupsUrls.CustomerGroupCustomer,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?>
                {
                    ["customer_group_id"] = customerGroupId,
                    ["customer_id"] = customerId,
                },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerGroupMember),
            }, cancellationToken);

    /// <summary>Add one customer to one group, keeping the other members - <c>POST /admin/customer_groups/assign</c>;
    /// <c>201</c> with <c>added</c> <c>false</c> for an existing member.</summary>
    public Task<ApiResponse> AssignCustomerGroupAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(CustomerGroupsUrls.CustomerGroupsAssign,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerGroupAssignment),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Id(object customerGroupId) =>
        new() { ["customer_group_id"] = customerGroupId };
}
