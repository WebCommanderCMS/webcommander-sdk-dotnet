using WebCommander.Client;
using WebCommander.Resources.Variations.VariationsDto;

namespace WebCommander.Resources.Variations;

/// <summary>Variation types and product variants - <c>/admin/variations*</c> and
/// <c>/admin/products/{product_id}/variations*</c> - as a typed resource bound to one client.</summary>
public sealed class Variations : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Variations</c>.</summary>
    public Variations(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the types by ascending id - <c>GET /admin/variations</c>. Accepts <c>search</c> (name),
    /// <c>standard</c>, <c>limit</c> (1 to 100), <c>offset</c> or <c>page</c>, and <c>field_metadata</c>.</summary>
    public Task<ApiResponse> ListVariationsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(VariationsUrls.Variations,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(VariationPage),
            }, cancellationToken);

    /// <summary>HEAD the type collection - <c>HEAD /admin/variations</c>; no body.</summary>
    public Task<ApiResponse> HeadVariationsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", VariationsUrls.Variations,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a type - <c>POST /admin/variations</c>, answered <c>201</c>. The body is
    /// <c>{"variation": {...}}</c> with <c>name</c>, <c>standard</c> and at least one option.</summary>
    public Task<ApiResponse> CreateVariationAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(VariationsUrls.Variations,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Variation),
            }, cancellationToken);
    }

    /// <summary>Retrieve one type; <c>field_metadata</c> adds the schema beside it.
    /// <c>GET /admin/variations/{variation_id}</c></summary>
    public Task<ApiResponse> GetVariationAsync(
        object variationId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(VariationsUrls.Variation,
            new RequestOptions
            {
                PathParams = Id(variationId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Variation),
            }, cancellationToken);

    /// <summary>HEAD one type; no body, the row read's <c>ETag</c>. <c>HEAD /admin/variations/{variation_id}</c></summary>
    public Task<ApiResponse> HeadVariationAsync(
        object variationId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", VariationsUrls.Variation,
            new RequestOptions
            {
                PathParams = Id(variationId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Replace one type - <c>PUT /admin/variations/{variation_id}</c>. <c>name</c> and
    /// <c>options</c> are required, and an option left out of the list is deleted.</summary>
    public Task<ApiResponse> UpdateVariationAsync(
        object variationId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(VariationsUrls.Variation,
            new RequestOptions
            {
                PathParams = Id(variationId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Variation),
            }, cancellationToken);
    }

    /// <summary>Merge into one type - <c>PATCH /admin/variations/{variation_id}</c>. Only the options sent
    /// are added, or changed where an <c>id</c> names one; the rest are kept.</summary>
    public Task<ApiResponse> PatchVariationAsync(
        object variationId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(VariationsUrls.Variation,
            new RequestOptions
            {
                PathParams = Id(variationId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Variation),
            }, cancellationToken);
    }

    /// <summary>Delete one type - <c>DELETE /admin/variations/{variation_id}</c>; <c>204</c>. A type with
    /// options is <c>409</c> unless <c>cascade</c> is <c>true</c>, which deletes them with it.</summary>
    public Task<ApiResponse> DeleteVariationAsync(
        object variationId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(VariationsUrls.Variation,
            new RequestOptions
            {
                PathParams = Id(variationId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Delete one option of a type; <c>204</c>, and a type's last option is <c>409</c>.
    /// <c>DELETE /admin/variations/{variation_id}/options/{variation_option_id}</c></summary>
    public Task<ApiResponse> DeleteVariationOptionAsync(
        object variationId, object variationOptionId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(VariationsUrls.VariationOption,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?>
                {
                    ["variation_id"] = variationId,
                    ["variation_option_id"] = variationOptionId,
                },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>One product's variants, grouped, with no paging; a tax-inclusive product's prices read
    /// net of tax. <c>GET /admin/products/{product_id}/variations</c></summary>
    public Task<ApiResponse> ListProductVariationsAsync(
        object productId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(VariationsUrls.ProductVariations,
            new RequestOptions
            {
                PathParams = ProductId(productId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductVariations),
            }, cancellationToken);

    /// <summary>Add or change a product's variants by <c>key</c>; <c>201</c>. A <c>product_key</c> naming other types
    /// than the product's is <c>409</c>. <c>POST /admin/products/{product_id}/variations</c></summary>
    public Task<ApiResponse> ImportProductVariationsAsync(
        object productId, object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(VariationsUrls.ProductVariations,
            new RequestOptions
            {
                PathParams = ProductId(productId),
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [VariationsUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductVariationsImportResult),
            }, cancellationToken);
    }

    /// <summary>Set fields on several variants of one product; a bare record, no envelope. <c>response</c>
    /// is <c>slim</c> or <c>full</c>. <c>PATCH /admin/products/{product_id}/variations/bulk</c></summary>
    public Task<ApiResponse> UpdateProductVariationsAsync(
        object productId, object payload,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(VariationsUrls.ProductVariationsBulk,
            new RequestOptions
            {
                PathParams = ProductId(productId),
                QueryParams = queryParams,
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ProductVariationsBulkResult),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Id(object variationId) =>
        new() { ["variation_id"] = variationId };

    private static Dictionary<string, object?> ProductId(object productId) =>
        new() { ["product_id"] = productId };
}
