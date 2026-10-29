namespace WebCommander.Resources.PaymentGateways;

/// <summary>Path templates for the payment gateway endpoints.</summary>
public static class PaymentGatewaysUrls
{
    public const string PaymentGateways = "/api/{api_version}/admin/payment_gateways";

    public const string PaymentGatewaysCount = "/api/{api_version}/admin/payment_gateways/count";

    public const string PaymentGateway = "/api/{api_version}/admin/payment_gateways/{payment_gateway_id}";
}
