using WebCommander.Client;
using WebCommander.Errors;
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

    /// <summary>Count products - <c>GET /admin/products/count</c>; <c>{"total": n, "count": n}</c>. It takes the
    /// list's filters (<c>search_text</c>, <c>name</c>, <c>sku</c>, <c>ids</c>, <c>parent</c>, <c>product_type</c>,
    /// <c>stock</c>, <c>is_available</c>, <c>is_inventory_enabled</c>, <c>created_by</c>, <c>price_from</c>,
    /// <c>price_to</c> and the four date bounds) and refuses its paging and sorting, and <c>status</c>, as
    /// <c>400 unknown_parameter</c>. A trashed product is not counted.</summary>
    public Task<ApiResponse> CountProductsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductsUrls.ProductsCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductCount),
            }, cancellationToken);

    /// <summary>Ask whether a SKU is free - <c>GET /admin/products/check-sku</c>. The match ignores case and sees a
    /// SKU held by a trashed product or by a variant; <c>excludeProductId</c> is the <c>id</c> of a product that may
    /// hold it, as when that product is being edited. Any other query name, and <c>SKU</c> in capitals, is
    /// <c>400 unknown_parameter</c>.</summary>
    public Task<ApiResponse> CheckProductSkuAsync(
        string sku, object? excludeProductId = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        if (string.IsNullOrEmpty(sku))
            throw new SdkUsageError("sku is required");
        return Client.SendGetRequestAsync(ProductsUrls.ProductsCheckSku,
            new RequestOptions
            {
                QueryParams = new Dictionary<string, object?>
                {
                    ["sku"] = sku,
                    ["exclude_product_id"] = excludeProductId,
                },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductSkuCheck),
            }, cancellationToken);
    }

    /// <summary>List a special selection of products - <c>GET /admin/products/special</c>. <c>type</c> is required:
    /// <c>onSale</c>, <c>featured</c>, <c>new</c>, <c>topSelling</c>, <c>lastBought</c> or <c>lastViewed</c>.
    /// <c>max</c> (1 to 100, default 10) and <c>offset</c> page it. The last two are per customer and need
    /// <c>customerId</c> (the <c>customer_id</c> or <c>internal_id</c> of a customer) or <c>customer_email</c>;
    /// <c>customer_id</c> is not read, and an unknown customer is <c>400 unknown_reference</c>. A name it does not
    /// know is ignored.</summary>
    public Task<ApiResponse> ListSpecialProductsAsync(
        IReadOnlyDictionary<string, object?> queryParams,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductsUrls.ProductsSpecial,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductSpecialPage),
            }, cancellationToken);

    /// <summary>Move several products to the trash - <c>POST /admin/products/bulk-delete</c>, answered <c>200</c> even
    /// when an id failed. The body is <c>{"ids": [...]}</c> (the collection sends <c>product_ids</c>, which the store
    /// answers <c>400 required</c> for <c>ids</c>); an empty list is <c>400</c>. Each id that is not a product is
    /// counted in <c>failed</c> and listed in <c>errors</c>.</summary>
    public Task<ApiResponse> BulkDeleteProductsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ProductsUrls.ProductsBulkDelete,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductsBulkDeleteResult),
            }, cancellationToken);
    }

    /// <summary>List a product's reviews - <c>GET /admin/products/{product_id}/reviews</c>; <c>{"reviews": [...]}</c>
    /// with no paging block. <c>status</c>, <c>limit</c> and <c>offset</c> are in the collection and this store
    /// ignores them, as it does any other name.</summary>
    public Task<ApiResponse> ListProductReviewsAsync(
        object productId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductsUrls.ProductReviews,
            new RequestOptions
            {
                PathParams = Id(productId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductReviewList),
            }, cancellationToken);

    /// <summary>Read one review under <c>review</c> - <c>GET /admin/products/{product_id}/reviews/{review_id}</c>.
    /// <c>reviewId</c> is a review's <c>id</c>; an unknown one is <c>404</c>.</summary>
    public Task<ApiResponse> GetProductReviewAsync(
        object productId, object reviewId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductsUrls.ProductReview,
            new RequestOptions
            {
                PathParams = ReviewIds(productId, reviewId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductReview),
            }, cancellationToken);

    /// <summary>Change one review - <c>PATCH /admin/products/{product_id}/reviews/{review_id}</c>, a bare body answered
    /// <c>200</c> with the review under <c>review</c>. <c>status</c> is <c>pending</c> (what an imported review
    /// without one reads) or <c>approved</c>; <c>rejected</c>, which the collection uses, is
    /// <c>400 invalid_enum</c>. <c>rating</c> and <c>comment</c> change too.</summary>
    public Task<ApiResponse> PatchProductReviewAsync(
        object productId, object reviewId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ProductsUrls.ProductReview,
            new RequestOptions
            {
                PathParams = ReviewIds(productId, reviewId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductReview),
            }, cancellationToken);
    }

    /// <summary>Delete one review for good; <c>204</c>, and a second delete is <c>404</c>.
    /// <c>DELETE /admin/products/{product_id}/reviews/{review_id}</c></summary>
    public Task<ApiResponse> DeleteProductReviewAsync(
        object productId, object reviewId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(ProductsUrls.ProductReview,
            new RequestOptions
            {
                PathParams = ReviewIds(productId, reviewId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Set a product's stock figure - <c>POST /admin/products/{product_id}/inventories</c>, answered
    /// <c>201</c>. The body is <c>{"product": {"product_pricing_and_stock": {"track_inventory": true,
    /// "available_stock": n}}}</c>; <c>low_level_stock</c> sets the low level. It records a history row and a variant
    /// keeps its own figure. The header is stored as the product's <c>transaction_no</c> when it has none, and the
    /// answer echoes the value from before the write.</summary>
    public Task<ApiResponse> CreateProductInventoryAsync(
        object productId, object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ProductsUrls.ProductInventories,
            new RequestOptions
            {
                PathParams = Id(productId),
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [ProductsUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductInventoryResult),
            }, cancellationToken);
    }

    /// <summary>Store images on a product - <c>POST /admin/products/{product_id}/image</c>, answered <c>201</c> with the
    /// files just stored. The body is JSON, <c>{"product": {"image_and_video": {"images": [{"filename": ...,
    /// "base64": ...}]}}}</c>; a multipart body is <c>415</c>.</summary>
    public Task<ApiResponse> CreateProductImageAsync(
        object productId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ProductsUrls.ProductImage,
            new RequestOptions
            {
                PathParams = Id(productId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductMediaUploaded),
            }, cancellationToken);
    }

    /// <summary>Store one file on a product - <c>POST /admin/products/{product_id}/media</c>, answered <c>201</c> with
    /// the file just stored as a bare image record (<c>id</c>, <c>filename</c>, <c>position</c>, <c>primary</c>, <c>url</c>),
    /// not under <c>product</c> as <c>CreateProductImageAsync</c> answers. A bare JSON body of <c>type</c>, <c>filename</c> and <c>content</c> (base64 or a
    /// <c>data:</c> URI). Only <c>type</c> <c>images</c> is accepted; <c>videos</c> and <c>specification_file</c> are
    /// <c>400 not_supported</c>, and <c>position</c> is <c>400 unknown_field</c>.</summary>
    public Task<ApiResponse> CreateProductMediaAsync(
        object productId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ProductsUrls.ProductMedia,
            new RequestOptions
            {
                PathParams = Id(productId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductMediaImage),
            }, cancellationToken);
    }

    /// <summary>Delete one image from a product; <c>204</c>, and a second delete is <c>404</c>. <c>imageId</c> is an
    /// image's <c>id</c> on the product.
    /// <c>DELETE /admin/products/{product_id}/media/images/{image_id}</c></summary>
    public Task<ApiResponse> DeleteProductMediaImageAsync(
        object productId, object imageId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(ProductsUrls.ProductMediaImage,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?>
                {
                    ["product_id"] = productId,
                    ["image_id"] = imageId,
                },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Set the order of a product's images; the first becomes <c>primary</c>. A bare
    /// <c>{"type": "images", "ids": [...]}</c> naming every image once, answered with <c>images</c> in the new order.
    /// <c>PUT /admin/products/{product_id}/media/order</c></summary>
    public Task<ApiResponse> UpdateProductMediaOrderAsync(
        object productId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(ProductsUrls.ProductMediaOrder,
            new RequestOptions
            {
                PathParams = Id(productId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductMediaOrder),
            }, cancellationToken);
    }

    /// <summary>Move one image to a 1-based <c>position</c>; a bare <c>{"position": n}</c> is the only body, and the
    /// image is answered as one flat row. <c>imageId</c> is an image's <c>id</c> on the product.
    /// <c>PATCH /admin/products/{product_id}/media/images/{image_id}</c></summary>
    public Task<ApiResponse> PatchProductMediaImageAsync(
        object productId, object imageId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ProductsUrls.ProductMediaImage,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?>
                {
                    ["product_id"] = productId,
                    ["image_id"] = imageId,
                },
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductMediaImage),
            }, cancellationToken);
    }

    /// <summary>A product's SEO, analytics, custom code, social and AI visibility settings under <c>seo</c>.
    /// <c>GET /admin/products/{product_id}/seo</c></summary>
    public Task<ApiResponse> GetProductSeoAsync(
        object productId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductsUrls.ProductSeo,
            new RequestOptions
            {
                PathParams = Id(productId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductSeo),
            }, cancellationToken);

    /// <summary>Change a product's stock; a bare body of <c>tracking</c>, <c>locations</c>, <c>note</c> and
    /// <c>transaction_no</c>, at least one of them. Answered with the <c>stock</c> now and the <c>adjustment</c> it
    /// recorded, <c>null</c> when the figure did not change. <c>PATCH /admin/products/{product_id}/stock</c></summary>
    public Task<ApiResponse> PatchProductStockAsync(
        object productId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ProductsUrls.ProductStock,
            new RequestOptions
            {
                PathParams = Id(productId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductStockPatchResult),
            }, cancellationToken);
    }

    /// <summary>A product's stock history, newest first, paged by <c>limit</c> and <c>offset</c>.
    /// <c>GET /admin/products/{product_id}/stock/adjustments</c></summary>
    public Task<ApiResponse> ListProductStockAdjustmentsAsync(
        object productId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ProductsUrls.ProductStockAdjustments,
            new RequestOptions
            {
                PathParams = Id(productId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductStockAdjustmentPage),
            }, cancellationToken);

    private static Dictionary<string, object?> ReviewIds(object productId, object reviewId) =>
        new() { ["product_id"] = productId, ["review_id"] = reviewId };

    private static Dictionary<string, object?> Id(object productId) =>
        new() { ["product_id"] = productId };
}
