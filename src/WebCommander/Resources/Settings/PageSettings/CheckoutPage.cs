using WebCommander.Client;

namespace WebCommander.Resources.Settings.PageSettings;

/// <summary>Checkout page settings - <c>/admin/settings/page_settings/checkout_page*</c> -
/// as a typed resource bound to one client.</summary>
public sealed class CheckoutPage : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.CheckoutPage</c>.</summary>
    public CheckoutPage(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/page_settings/checkout_page</c>.</summary>
    public Task<ApiResponse> GetCheckoutPageAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CheckoutPageUrls.CheckoutPage,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CheckoutPageDto.CheckoutPage),
            }, cancellationToken);

    /// <summary>Write fields in one section or several - <c>PATCH /admin/settings/page_settings/checkout_page</c>.</summary>
    public Task<ApiResponse> PatchCheckoutPageAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CheckoutPageUrls.CheckoutPage,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CheckoutPageDto.CheckoutPage),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/page_settings/checkout_page</c>.</summary>
    public Task<ApiResponse> HeadCheckoutPageAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CheckoutPageUrls.CheckoutPage,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/page_settings/checkout_page/{section}</c>.</summary>
    public Task<ApiResponse> GetCheckoutPageSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CheckoutPageUrls.CheckoutPageSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CheckoutPageDto.CheckoutPage),
            }, cancellationToken);

    /// <summary>Write fields in one section - <c>PATCH /admin/settings/page_settings/checkout_page/{section}</c>.</summary>
    public Task<ApiResponse> PatchCheckoutPageSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(CheckoutPageUrls.CheckoutPageSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CheckoutPageDto.CheckoutPage),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };
}
