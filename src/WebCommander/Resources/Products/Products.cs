using WebCommander.Client;
using WebCommander.Resources.Products.ProductsDto;

namespace WebCommander.Resources.Products;

/// <summary>Products, their reviews and stock - <c>/admin/products*</c> - as a typed resource bound to one client.</summary>
public sealed class Products : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Products</c>.</summary>
    public Products(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List products, the most recently updated first - <c>GET /admin/products</c>; each row is the full
    /// record the row read answers, and a trashed product is not listed. <c>limit</c> is 1 to 100 (default 20, above
    /// 100 is <c>400</c>), with <c>offset</c> or a 1-based <c>page</c>. <c>search_text</c> matches <c>name</c> or
    /// <c>sku</c>; <c>name</c>, <c>sku</c>, <c>ids</c> (repeat the name for more; a comma list is <c>500</c>),
    /// <c>parent</c> (a category id), <c>product_type</c>, <c>stock</c> (<c>in</c>, <c>out</c> or <c>low</c>),
    /// <c>is_available</c>, <c>is_inventory_enabled</c>, <c>created_by</c>, <c>price_from</c>, <c>price_to</c>,
    /// <c>created_from</c>, <c>created_to</c>, <c>updated_from</c> and <c>updated_to</c> narrow it. <c>sort</c> takes
    /// <c>name</c>, <c>isAvailable</c>, <c>sku</c>, <c>parent</c>, <c>basePrice</c>, <c>isInventoryEnabled</c>,
    /// <c>createdBy</c>, <c>created</c>, <c>updated</c> or <c>id</c>, with <c>dir</c>; <c>field_metadata</c> adds a
    /// schema. Any other name is <c>400</c>.</summary>
    public Task<ApiResponse> ListProductsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductsUrls.Products,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductPage),
            }, cancellationToken);

    /// <summary>Create a product - <c>POST /admin/products</c>, answered <c>201</c>. A bare record: <c>name</c> and
    /// <c>base_price</c> are required, <c>sku</c> must be free, and the store makes one when left out.</summary>
    public Task<ApiResponse> CreateProductAsync(
        object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ProductsUrls.Products,
            new RequestOptions
            {
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [ProductsUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Product),
            }, cancellationToken);
    }

    /// <summary>Retrieve one product - <c>GET /admin/products/{product_id}</c>. <c>field_metadata=true</c> adds a
    /// field schema beside <c>product</c>; any other name is <c>400 unknown_parameter</c>.</summary>
    public Task<ApiResponse> GetProductAsync(
        object productId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductsUrls.Product,
            new RequestOptions
            {
                PathParams = Id(productId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Product),
            }, cancellationToken);

    /// <summary>Update one product - <c>PUT /admin/products/{product_id}</c>. A bare record merged at every
    /// level; it must carry <c>name</c>, <c>sku</c>, <c>base_price</c>, <c>product_type</c>, <c>available</c>,
    /// <c>is_on_sale</c>, <c>available_on_date_range</c>, <c>available_on_weekdays</c>, <c>available_for</c> and
    /// <c>selected_customers</c> (<c>400 required</c> otherwise), and fields left out keep their values.
    /// <c>{"product": {...}}</c> is <c>400 unknown_field</c>.</summary>
    public Task<ApiResponse> UpdateProductAsync(
        object productId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(ProductsUrls.Product,
            new RequestOptions
            {
                PathParams = Id(productId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Product),
            }, cancellationToken);
    }

    /// <summary>Merge into one product - <c>PATCH /admin/products/{product_id}</c>, the same bare record the
    /// <c>PUT</c> takes; a product in the trash is <c>409</c>.</summary>
    public Task<ApiResponse> PatchProductAsync(
        object productId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ProductsUrls.Product,
            new RequestOptions
            {
                PathParams = Id(productId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Product),
            }, cancellationToken);
    }

    /// <summary>Move a product to the trash - <c>DELETE /admin/products/{product_id}</c>; <c>204</c>. It still
    /// reads back, <c>status</c> <c>trashed</c>, and a second delete is <c>404</c>.</summary>
    public Task<ApiResponse> DeleteProductAsync(
        object productId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(ProductsUrls.Product,
            new RequestOptions
            {
                PathParams = Id(productId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Add reviews to a product - <c>POST /admin/products/{product_id}/reviews</c>, answered <c>201</c> with
    /// every review it holds. The body is <c>{"product": {"reviews": [...]}}</c>.</summary>
    public Task<ApiResponse> ImportProductReviewsAsync(
        object productId, object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ProductsUrls.ProductReviews,
            new RequestOptions
            {
                PathParams = Id(productId),
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [ProductsUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductReviews),
            }, cancellationToken);
    }

    /// <summary>One product's stock at each location - <c>GET /admin/products/{product_id}/stock</c>.</summary>
    public Task<ApiResponse> GetProductStockAsync(
        object productId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductsUrls.ProductStock,
            new RequestOptions
            {
                PathParams = Id(productId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductStock),
            }, cancellationToken);

    /// <summary>Set the product's stock - <c>PUT /admin/products/{product_id}/stock</c>. <c>locations</c> replaces
    /// the whole set: a location left out is unassigned, so send every row that is to stay.</summary>
    public Task<ApiResponse> UpdateProductStockAsync(
        object productId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(ProductsUrls.ProductStock,
            new RequestOptions
            {
                PathParams = Id(productId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductStock),
            }, cancellationToken);
    }

    /// <summary>Unassign one location from the product; <c>204</c>. The location must hold <c>0</c> units and not be
    /// the product's default, or it is <c>409</c> -
    /// <c>DELETE /admin/products/{product_id}/stock/locations/{inventory_location_id}</c>.</summary>
    public Task<ApiResponse> DeleteProductStockLocationAsync(
        object productId, object inventoryLocationId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(ProductsUrls.ProductStockLocation,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?>
                {
                    ["product_id"] = productId,
                    ["inventory_location_id"] = inventoryLocationId,
                },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object productId) =>
        new() { ["product_id"] = productId };
}
