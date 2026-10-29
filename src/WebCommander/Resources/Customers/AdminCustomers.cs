using WebCommander.Client;
using WebCommander.Errors;
using WebCommander.Resources.Customers.AdminCustomersDto;

namespace WebCommander.Resources.Customers;

/// <summary>Admin customer endpoints - <c>/admin/customers*</c> - as a typed resource bound to one client.</summary>
public sealed class AdminCustomers : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.AdminCustomers</c>.</summary>
    public AdminCustomers(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List customers - <c>GET /admin/customers</c>.</summary>
    public Task<ApiResponse> GetCustomersAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(AdminCustomersUrls.AdminCustomers,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page<Customer>),
            }, cancellationToken);

    /// <summary>One customer by id - <c>GET /admin/customers/{customer_id}</c>.</summary>
    public Task<ApiResponse> GetCustomerAsync(
        object customerId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(AdminCustomersUrls.AdminCustomer,
            new RequestOptions
            {
                PathParams = Id(customerId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Customer),
            }, cancellationToken);

    /// <summary>Count matching customers - <c>GET /admin/customers/count</c>; the key is <c>total</c>.</summary>
    public Task<ApiResponse> GetCustomerCountAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(AdminCustomersUrls.AdminCustomersCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Whether an email is registered - <c>GET /admin/customers/check-email</c>; trashed ones count.</summary>
    public Task<ApiResponse> GetCustomerEmailExistsAsync(
        string email,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new SdkUsageError("email is required and identifies the lookup, so a null or "
                + "blank one is refused here rather than sent as a different question");
        return Client.SendGetRequestAsync(AdminCustomersUrls.AdminCustomersCheckEmail,
            new RequestOptions
            {
                QueryParams = new Dictionary<string, object?> { ["email"] = email },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
    }

    /// <summary>Download every customer as XLSX - <c>GET /admin/customers/export</c>; it filters no rows.</summary>
    public Task<ApiResponse> GetCustomerExportAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(AdminCustomersUrls.AdminCustomersExport,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a customer - <c>POST /admin/customers</c>; keep the answer's <c>customer_id</c>.
    /// <paramref name="transactionNo"/> is sent as <c>X-Transaction-No</c> and files the customer under that number.</summary>
    public Task<ApiResponse> CreateCustomerAsync(
        object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(AdminCustomersUrls.AdminCustomers,
            new RequestOptions
            {
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [AdminCustomersUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Customer),
            }, cancellationToken);
    }

    /// <summary>Update a customer - <c>PUT /admin/customers/{customer_id}</c>.</summary>
    public Task<ApiResponse> UpdateCustomerAsync(
        object customerId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(AdminCustomersUrls.AdminCustomer,
            new RequestOptions
            {
                PathParams = Id(customerId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Customer),
            }, cancellationToken);
    }

    /// <summary>Delete a customer - <c>DELETE /admin/customers/{customer_id}</c>; the delete is soft.</summary>
    public Task<ApiResponse> DeleteCustomerAsync(
        object customerId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(AdminCustomersUrls.AdminCustomer,
            new RequestOptions
            {
                PathParams = Id(customerId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Add addresses - <c>POST /admin/customers/{customer_id}/addresses</c>.</summary>
    public Task<ApiResponse> CreateCustomerAddressesAsync(
        object customerId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(AdminCustomersUrls.AdminCustomerAddresses,
            new RequestOptions
            {
                PathParams = Id(customerId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Customer),
            }, cancellationToken);
    }

    /// <summary>Set a password - <c>POST /admin/customers/{customer_id}/reset-password</c>.</summary>
    public Task<ApiResponse> ResetCustomerPasswordAsync(
        object customerId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(AdminCustomersUrls.AdminCustomerResetPassword,
            new RequestOptions
            {
                PathParams = Id(customerId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
    }

    /// <summary>Change a password - <c>POST /admin/customers/{customer_id}/change-password</c>.</summary>
    public Task<ApiResponse> ChangeCustomerPasswordAsync(
        object customerId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(AdminCustomersUrls.AdminCustomerChangePassword,
            new RequestOptions
            {
                PathParams = Id(customerId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
    }

    /// <summary>Queue resets for many customers - <c>POST /admin/customers/bulk-reset-password</c>.</summary>
    public Task<ApiResponse> BulkResetCustomerPasswordsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(AdminCustomersUrls.AdminCustomersBulkResetPassword,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
    }

    /// <summary>Import a spreadsheet of customers - <c>POST /admin/customers/import</c>.</summary>
    public Task<ApiResponse> ImportCustomersAsync(
        IReadOnlyDictionary<string, object?> files,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequireFiles(files);
        return Client.SendPostRequestAsync(AdminCustomersUrls.AdminCustomersImport,
            new RequestOptions
            {
                Files = files,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
    }

    /// <summary>List store-credit movements - <c>GET /admin/customers/{customer_id}/store-credit/adjustments</c>.</summary>
    public Task<ApiResponse> GetCustomerStoreCreditAdjustmentsAsync(
        object customerId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(AdminCustomersUrls.AdminCustomerStoreCreditAdjustments,
            new RequestOptions
            {
                PathParams = Id(customerId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Page<StoreCreditAdjustment>),
            }, cancellationToken);

    /// <summary>Adjust store credit - <c>POST /admin/customers/{customer_id}/store-credit/adjustments</c>.</summary>
    public Task<ApiResponse> CreateCustomerStoreCreditAdjustmentAsync(
        object customerId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(AdminCustomersUrls.AdminCustomerStoreCreditAdjustments,
            new RequestOptions
            {
                PathParams = Id(customerId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(StoreCreditAdjustment),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Id(object customerId) => new() { ["customer_id"] = customerId };

    private static void RequireFiles(IReadOnlyDictionary<string, object?> files)
    {
        if (files is null || files.Count == 0)
            throw new SdkUsageError("files must carry the 'file' upload, a .xlsx, .xls or .csv filename");
    }
}
