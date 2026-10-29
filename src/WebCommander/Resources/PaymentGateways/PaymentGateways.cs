using WebCommander.Client;
using WebCommander.Resources.PaymentGateways.PaymentGatewaysDto;

namespace WebCommander.Resources.PaymentGateways;

/// <summary>Payment gateways - <c>/admin/payment_gateways*</c> - as a typed resource bound to one client.</summary>
public sealed class PaymentGateways : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.PaymentGateways</c>.</summary>
    public PaymentGateways(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the store's gateways by ascending <c>id</c> - <c>GET /admin/payment_gateways</c>, rows
    /// without <c>settings</c>. <c>limit</c> is 1 to 100, default 20; <c>0</c> or above 100 is <c>400</c>
    /// <c>out_of_range</c>. <c>offset</c> or <c>page</c>; <c>search</c> a substring of the name; <c>exclude</c> a
    /// comma list of codes; <c>promotional</c> and <c>added</c> filter. Other names, <c>sort</c> included, are
    /// ignored.</summary>
    public Task<ApiResponse> ListPaymentGatewaysAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(PaymentGatewaysUrls.PaymentGateways,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PaymentGatewayPage),
            }, cancellationToken);

    /// <summary>HEAD the gateway collection - <c>HEAD /admin/payment_gateways</c>; no body.</summary>
    public Task<ApiResponse> HeadPaymentGatewaysAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", PaymentGatewaysUrls.PaymentGateways,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Count the gateways - <c>GET /admin/payment_gateways/count</c>, a bare <c>{"total": N}</c>.
    /// The listing's filters narrow it; <c>limit</c> is checked as on the listing, <c>0</c> or above 100
    /// <c>400</c> <c>out_of_range</c>, but changes nothing.</summary>
    public Task<ApiResponse> CountPaymentGatewaysAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(PaymentGatewaysUrls.PaymentGatewaysCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PaymentGatewayCount),
            }, cancellationToken);

    /// <summary>HEAD the count - <c>HEAD /admin/payment_gateways/count</c>; no body.</summary>
    public Task<ApiResponse> HeadPaymentGatewaysCountAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", PaymentGatewaysUrls.PaymentGatewaysCount,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Retrieve one gateway with its <c>settings</c>; secrets read <c>********</c> when set.
    /// <c>GET /admin/payment_gateways/{payment_gateway_id}</c></summary>
    public Task<ApiResponse> GetPaymentGatewayAsync(
        object paymentGatewayId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(PaymentGatewaysUrls.PaymentGateway,
            new RequestOptions
            {
                PathParams = Id(paymentGatewayId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PaymentGateway),
            }, cancellationToken);

    /// <summary>HEAD one gateway - <c>HEAD /admin/payment_gateways/{payment_gateway_id}</c>; no body.</summary>
    public Task<ApiResponse> HeadPaymentGatewayAsync(
        object paymentGatewayId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", PaymentGatewaysUrls.PaymentGateway,
            new RequestOptions
            {
                PathParams = Id(paymentGatewayId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Write one gateway - <c>PUT /admin/payment_gateways/{payment_gateway_id}</c>. A bare record
    /// merged like the <c>PATCH</c>: the fields left out keep their value.</summary>
    public Task<ApiResponse> UpdatePaymentGatewayAsync(
        object paymentGatewayId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(PaymentGatewaysUrls.PaymentGateway,
            new RequestOptions
            {
                PathParams = Id(paymentGatewayId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PaymentGateway),
            }, cancellationToken);
    }

    /// <summary>Change only the fields sent - <c>PATCH /admin/payment_gateways/{payment_gateway_id}</c>.
    /// A bare record; an empty body is <c>400</c>.</summary>
    public Task<ApiResponse> PatchPaymentGatewayAsync(
        object paymentGatewayId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(PaymentGatewaysUrls.PaymentGateway,
            new RequestOptions
            {
                PathParams = Id(paymentGatewayId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(PaymentGateway),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Id(object paymentGatewayId) =>
        new() { ["payment_gateway_id"] = paymentGatewayId };
}
