namespace WebCommander.Resources.CustomerGroups;

/// <summary>Path templates for the customer group endpoints.</summary>
public static class CustomerGroupsUrls
{
    public const string CustomerGroups = "/api/{api_version}/admin/customer_groups";

    public const string CustomerGroupsCount = "/api/{api_version}/admin/customer_groups/count";

    public const string CustomerGroupsAssign = "/api/{api_version}/admin/customer_groups/assign";

    public const string CustomerGroup =
        "/api/{api_version}/admin/customer_groups/{customer_group_id}";

    public const string CustomerGroupCustomers =
        "/api/{api_version}/admin/customer_groups/{customer_group_id}/customers";

    public const string CustomerGroupCustomer =
        "/api/{api_version}/admin/customer_groups/{customer_group_id}/customers/{customer_id}";
}
