using WebCommander.Client;

namespace WebCommander.Resources.Settings.CustomerPortal;

/// <summary>Customer portal - <c>/admin/settings/customer_portal*</c> - as a typed resource bound to one client.</summary>
public sealed class CustomerPortal : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.CustomerPortal</c>.</summary>
    public CustomerPortal(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/customer_portal</c>.</summary>
    public Task<ApiResponse> GetCustomerPortalAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CustomerPortalUrls.CustomerPortal,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerPortalDto.CustomerPortal),
            }, cancellationToken);

    /// <summary>Write fields in one or more writable sections - <c>PATCH /admin/settings/customer_portal</c>.</summary>
    public Task<ApiResponse> PatchCustomerPortalAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CustomerPortalUrls.CustomerPortal,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerPortalDto.CustomerPortal),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/customer_portal</c>.</summary>
    public Task<ApiResponse> HeadCustomerPortalAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CustomerPortalUrls.CustomerPortal,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/customer_portal/{section}</c>.</summary>
    public Task<ApiResponse> GetCustomerPortalSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CustomerPortalUrls.CustomerPortalSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerPortalDto.CustomerPortal),
            }, cancellationToken);

    /// <summary>Write fields in one writable section - <c>PATCH /admin/settings/customer_portal/{section}</c>.</summary>
    public Task<ApiResponse> PatchCustomerPortalSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CustomerPortalUrls.CustomerPortalSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerPortalDto.CustomerPortal),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };
}
