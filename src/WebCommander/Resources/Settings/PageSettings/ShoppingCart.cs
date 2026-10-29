using WebCommander.Client;

namespace WebCommander.Resources.Settings.PageSettings;

/// <summary>Shopping cart settings - <c>/admin/settings/page_settings/shopping_cart*</c> -
/// as a typed resource bound to one client.</summary>
public sealed class ShoppingCart : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.ShoppingCart</c>.</summary>
    public ShoppingCart(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/page_settings/shopping_cart</c>.</summary>
    public Task<ApiResponse> GetShoppingCartAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShoppingCartUrls.ShoppingCart,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShoppingCartDto.ShoppingCart),
            }, cancellationToken);

    /// <summary>Write fields in one section or both - <c>PATCH /admin/settings/page_settings/shopping_cart</c>.</summary>
    public Task<ApiResponse> PatchShoppingCartAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ShoppingCartUrls.ShoppingCart,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShoppingCartDto.ShoppingCart),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/page_settings/shopping_cart</c>.</summary>
    public Task<ApiResponse> HeadShoppingCartAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShoppingCartUrls.ShoppingCart,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/page_settings/shopping_cart/{section}</c>.</summary>
    public Task<ApiResponse> GetShoppingCartSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShoppingCartUrls.ShoppingCartSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShoppingCartDto.ShoppingCart),
            }, cancellationToken);

    /// <summary>Write fields in one section - <c>PATCH /admin/settings/page_settings/shopping_cart/{section}</c>.</summary>
    public Task<ApiResponse> PatchShoppingCartSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ShoppingCartUrls.ShoppingCartSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShoppingCartDto.ShoppingCart),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };
}
