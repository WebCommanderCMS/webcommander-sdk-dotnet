namespace WebCommander.Resources.Customers;

/// <summary>Path templates and the transaction number header for the admin customer endpoints.</summary>
public static class AdminCustomersUrls
{
    public const string AdminCustomers = "/api/{api_version}/admin/customers";

    public const string AdminCustomer = "/api/{api_version}/admin/customers/{customer_id}";

    public const string AdminCustomersCount = "/api/{api_version}/admin/customers/count";

    public const string AdminCustomersCheckEmail = "/api/{api_version}/admin/customers/check-email";

    public const string AdminCustomersExport = "/api/{api_version}/admin/customers/export";

    public const string AdminCustomersImport = "/api/{api_version}/admin/customers/import";

    public const string AdminCustomersBulkResetPassword = "/api/{api_version}/admin/customers/bulk-reset-password";

    public const string AdminCustomerAddresses = "/api/{api_version}/admin/customers/{customer_id}/addresses";

    public const string AdminCustomerResetPassword = "/api/{api_version}/admin/customers/{customer_id}/reset-password";

    public const string AdminCustomerChangePassword = "/api/{api_version}/admin/customers/{customer_id}/change-password";

    public const string AdminCustomerStoreCreditAdjustments =
        "/api/{api_version}/admin/customers/{customer_id}/store-credit/adjustments";

    /// <summary>Files the new customer under this transaction number; a different <c>transaction_no</c> beside
    /// <c>customer</c> is refused with <c>400 header_body_mismatch</c>, and one inside <c>customer</c> is
    /// ignored.</summary>
    public const string TransactionNoHeader = "X-Transaction-No";
}
