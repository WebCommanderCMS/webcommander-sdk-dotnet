using WebCommander.Client;
using WebCommander.Resources.TaxRules.TaxRulesDto;

namespace WebCommander.Resources.TaxRules;

/// <summary>Tax rules - <c>/admin/tax_rules*</c> - as a typed resource bound to one client.</summary>
public sealed class TaxRules : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.TaxRules</c>.</summary>
    public TaxRules(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the tax rules - <c>GET /admin/tax_rules</c>. <c>limit</c> is capped at 100.</summary>
    public Task<ApiResponse> ListTaxRulesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxRulesUrls.TaxRules,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxRulePage),
            }, cancellationToken);

    /// <summary>HEAD the tax rule collection - <c>HEAD /admin/tax_rules</c>.</summary>
    public Task<ApiResponse> HeadTaxRulesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TaxRulesUrls.TaxRules,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a tax rule - <c>POST /admin/tax_rules</c>; only name and description are read.</summary>
    public Task<ApiResponse> CreateTaxRuleAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(TaxRulesUrls.TaxRules,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxRule),
            }, cancellationToken);
    }

    /// <summary>Retrieve one tax rule - <c>GET /admin/tax_rules/{tax_rule_id}</c>.</summary>
    public Task<ApiResponse> GetTaxRuleAsync(
        object taxRuleId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxRulesUrls.TaxRule,
            new RequestOptions
            {
                PathParams = Id(taxRuleId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxRule),
            }, cancellationToken);

    /// <summary>HEAD one tax rule - <c>HEAD /admin/tax_rules/{tax_rule_id}</c>.</summary>
    public Task<ApiResponse> HeadTaxRuleAsync(
        object taxRuleId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TaxRulesUrls.TaxRule,
            new RequestOptions
            {
                PathParams = Id(taxRuleId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Update one tax rule - <c>PUT /admin/tax_rules/{tax_rule_id}</c>, a merge.</summary>
    public Task<ApiResponse> UpdateTaxRuleAsync(
        object taxRuleId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(TaxRulesUrls.TaxRule,
            new RequestOptions
            {
                PathParams = Id(taxRuleId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxRule),
            }, cancellationToken);
    }

    /// <summary>Delete one tax rule - <c>DELETE /admin/tax_rules/{tax_rule_id}</c>.</summary>
    public Task<ApiResponse> DeleteTaxRuleAsync(
        object taxRuleId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(TaxRulesUrls.TaxRule,
            new RequestOptions
            {
                PathParams = Id(taxRuleId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object taxRuleId) =>
        new() { ["tax_rule_id"] = taxRuleId };
}
