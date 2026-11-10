using WebCommander.Client;
using WebCommander.Resources.ShippingRates.ShippingRatesDto;

namespace WebCommander.Resources.ShippingRates;

/// <summary>Shipping rates - <c>/admin/shipping_rates*</c> - as a typed resource bound to one client.</summary>
public sealed class ShippingRates : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.ShippingRates</c>.</summary>
    public ShippingRates(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the rates, newest first - <c>GET /admin/shipping_rates</c>. <c>search</c> (part of the name),
    /// <c>method</c> (such as <c>flat_rate</c> or <c>ship_by_weight</c>) and <c>handling_cost</c> (<c>with_handling</c> or
    /// <c>without_handling</c>) filter; <c>sort</c> is <c>id</c>, <c>name</c>, <c>method</c>, <c>created_at</c>,
    /// <c>updated_at</c> or <c>transaction_no</c> with <c>order</c> <c>asc</c> or <c>desc</c>; <c>limit</c> is 1 to 100 and
    /// defaults to 20, <c>offset</c> pages. An unknown value is <c>400</c>.</summary>
    public Task<ApiResponse> ListShippingRatesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingRatesUrls.ShippingRates,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRateListing),
            }, cancellationToken);

    /// <summary>HEAD the collection - <c>HEAD /admin/shipping_rates</c>; no body.</summary>
    public Task<ApiResponse> HeadShippingRatesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingRatesUrls.ShippingRates,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Count the rates - <c>GET /admin/shipping_rates/count</c>, a bare <c>{"total": N}</c>. The listing's filters
    /// narrow it.</summary>
    public Task<ApiResponse> CountShippingRatesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingRatesUrls.ShippingRatesCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRateCount),
            }, cancellationToken);

    /// <summary>HEAD the count - <c>HEAD /admin/shipping_rates/count</c>; no body.</summary>
    public Task<ApiResponse> HeadShippingRatesCountAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingRatesUrls.ShippingRatesCount,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a rate - <c>POST /admin/shipping_rates</c>, answered <c>201</c> with a <c>Location</c>. The body is
    /// wrapped in <c>shipping_rate</c> and needs <c>name</c>, unique without regard to case (<c>409</c>), and <c>method</c>.
    /// A flat rate needs <c>shipping_cost</c>; <c>ship_by_price</c>, <c>ship_by_quantity</c>, <c>ship_by_weight</c> and
    /// <c>ship_by_variation</c> take their costs per row in <c>conditions</c> and refuse a rate-level
    /// <c>shipping_cost</c>. <c>free_shipping</c> always saves <c>calculate_for_entire_order</c> false.
    /// <c>shipping_rule_id</c> attaches the rate to a rule that has none (<c>409</c> otherwise). An unknown field, an
    /// <c>id</c> or an unwrapped body is <c>400</c>. <paramref name="transactionNo"/> is sent as <c>X-Transaction-No</c>
    /// and wins over a <c>transaction_no</c> in the body.</summary>
    public Task<ApiResponse> CreateShippingRateAsync(
        object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ShippingRatesUrls.ShippingRates,
            new RequestOptions
            {
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [ShippingRatesUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRate),
            }, cancellationToken);
    }

    /// <summary>Retrieve one rate by its <c>id</c>, the row under <c>shipping_rate</c> -
    /// <c>GET /admin/shipping_rates/{shipping_rate_id}</c>. An id that is not plain digits, or unknown, is <c>404</c>.</summary>
    public Task<ApiResponse> GetShippingRateAsync(
        object shippingRateId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingRatesUrls.ShippingRate,
            new RequestOptions
            {
                PathParams = Id(shippingRateId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRate),
            }, cancellationToken);

    /// <summary>Ask whether a rate exists - <c>HEAD /admin/shipping_rates/{shipping_rate_id}</c>; <c>200</c> with no body,
    /// <c>404</c> for an unknown id.</summary>
    public Task<ApiResponse> HeadShippingRateAsync(
        object shippingRateId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingRatesUrls.ShippingRate,
            new RequestOptions
            {
                PathParams = Id(shippingRateId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Replace one rate - <c>PUT /admin/shipping_rates/{shipping_rate_id}</c>. Every field is required: <c>name</c>,
    /// <c>method</c>, <c>calculate_for_entire_order</c>, <c>price_entered_with_tax</c>, the costs the method uses and
    /// <c>additional_rule</c>; a <c>handling_cost</c> or <c>additional_rule</c> the rate does not use is sent as
    /// <c>null</c>. A field left out is <c>400</c> naming it.</summary>
    public Task<ApiResponse> UpdateShippingRateAsync(
        object shippingRateId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(ShippingRatesUrls.ShippingRate,
            new RequestOptions
            {
                PathParams = Id(shippingRateId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRate),
            }, cancellationToken);
    }

    /// <summary>Change only the fields sent - <c>PATCH /admin/shipping_rates/{shipping_rate_id}</c>; an empty
    /// <c>shipping_rate</c> is <c>200</c> and changes nothing.</summary>
    public Task<ApiResponse> PatchShippingRateAsync(
        object shippingRateId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ShippingRatesUrls.ShippingRate,
            new RequestOptions
            {
                PathParams = Id(shippingRateId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRate),
            }, cancellationToken);
    }

    /// <summary>Delete one rate - <c>DELETE /admin/shipping_rates/{shipping_rate_id}</c>, <c>204</c>. A rate a rule uses is
    /// <c>409</c> naming the rule.</summary>
    public Task<ApiResponse> DeleteShippingRateAsync(
        object shippingRateId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(ShippingRatesUrls.ShippingRate,
            new RequestOptions
            {
                PathParams = Id(shippingRateId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object shippingRateId) =>
        new() { ["shipping_rate_id"] = shippingRateId };
}
