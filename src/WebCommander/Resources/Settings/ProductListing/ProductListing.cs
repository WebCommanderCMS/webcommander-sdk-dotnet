using WebCommander.Client;

namespace WebCommander.Resources.Settings.ProductListing;

/// <summary>Product listing - <c>/admin/settings/product_listing</c> - as a typed resource bound to one client.</summary>
public sealed class ProductListing : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.ProductListing</c>.</summary>
    public ProductListing(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/product_listing</c>.</summary>
    public Task<ApiResponse> GetProductListingAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductListingUrls.ProductListing,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductListingDto.ProductListing),
            }, cancellationToken);

    /// <summary>Write fields in one section or in several - <c>PATCH /admin/settings/product_listing</c>.</summary>
    public Task<ApiResponse> PatchProductListingAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ProductListingUrls.ProductListing,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductListingDto.ProductListing),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/product_listing</c>.</summary>
    public Task<ApiResponse> HeadProductListingAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ProductListingUrls.ProductListing,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
}
