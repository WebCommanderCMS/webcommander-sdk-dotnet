using WebCommander.Client;
using WebCommander.Resources.ShippingRules.ShippingRulesDto;

namespace WebCommander.Resources.ShippingRules;

/// <summary>Shipping rules - <c>/admin/shipping_rules*</c> - as a typed resource bound to one client.</summary>
public sealed class ShippingRules : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.ShippingRules</c>.</summary>
    public ShippingRules(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the rules, newest first - <c>GET /admin/shipping_rules</c>. <c>search</c> (part of the name),
    /// <c>shipping_class_id</c>, <c>zone_id</c>, <c>method</c> (the method of the rule's rate) and <c>handling_cost</c>
    /// (<c>with_handling</c> or <c>without_handling</c>) filter, each rule listed once; <c>sort</c> is <c>id</c>,
    /// <c>name</c>, <c>description</c>, <c>created_at</c>, <c>updated_at</c> or <c>transaction_no</c> with <c>order</c>
    /// <c>asc</c> or <c>desc</c>; <c>limit</c> is 1 to 100 and defaults to 20, <c>offset</c> pages. An unknown value, or
    /// a class or zone id that names no record, is <c>400</c>.</summary>
    public Task<ApiResponse> ListShippingRulesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingRulesUrls.ShippingRules,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRuleListing),
            }, cancellationToken);

    /// <summary>HEAD the collection - <c>HEAD /admin/shipping_rules</c>; no body.</summary>
    public Task<ApiResponse> HeadShippingRulesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingRulesUrls.ShippingRules,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Count the rules - <c>GET /admin/shipping_rules/count</c>, a bare <c>{"total": N}</c>. The listing's filters
    /// narrow it.</summary>
    public Task<ApiResponse> CountShippingRulesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingRulesUrls.ShippingRulesCount,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRuleCount),
            }, cancellationToken);

    /// <summary>HEAD the count - <c>HEAD /admin/shipping_rules/count</c>; no body.</summary>
    public Task<ApiResponse> HeadShippingRulesCountAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingRulesUrls.ShippingRulesCount,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create a rule - <c>POST /admin/shipping_rules</c>, answered <c>201</c> with a <c>Location</c>. The body is
    /// wrapped in <c>shipping_rule</c> and needs <c>name</c>, unique without regard to case (<c>409</c>).
    /// <c>shipping_class</c>, <c>shipping_rate</c> and <c>zones</c> name records by id, bare or as <c>{"id": ...}</c>;
    /// <c>profile_id</c> adds the rule to a profile. An unknown field, an <c>id</c> or an unwrapped body is <c>400</c>.
    /// <paramref name="transactionNo"/> is sent as <c>X-Transaction-No</c> and wins over a <c>transaction_no</c> in the
    /// body.</summary>
    public Task<ApiResponse> CreateShippingRuleAsync(
        object payload, string? transactionNo = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(ShippingRulesUrls.ShippingRules,
            new RequestOptions
            {
                JsonBody = payload,
                Headers = transactionNo is null
                    ? null
                    : new Dictionary<string, string> { [ShippingRulesUrls.TransactionNoHeader] = transactionNo },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRule),
            }, cancellationToken);
    }

    /// <summary>Retrieve one rule by its <c>id</c>, the row under <c>shipping_rule</c> -
    /// <c>GET /admin/shipping_rules/{shipping_rule_id}</c>. An id that is not plain digits, or unknown, is <c>404</c>.</summary>
    public Task<ApiResponse> GetShippingRuleAsync(
        object shippingRuleId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(ShippingRulesUrls.ShippingRule,
            new RequestOptions
            {
                PathParams = Id(shippingRuleId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRule),
            }, cancellationToken);

    /// <summary>Ask whether a rule exists - <c>HEAD /admin/shipping_rules/{shipping_rule_id}</c>; <c>200</c> with no body,
    /// <c>404</c> for an unknown id.</summary>
    public Task<ApiResponse> HeadShippingRuleAsync(
        object shippingRuleId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", ShippingRulesUrls.ShippingRule,
            new RequestOptions
            {
                PathParams = Id(shippingRuleId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Replace one rule - <c>PUT /admin/shipping_rules/{shipping_rule_id}</c>. <c>name</c>, <c>description</c>,
    /// <c>shipping_class</c>, <c>shipping_rate</c> and <c>zones</c> are all required; the first three may be <c>null</c>
    /// and <c>zones</c> <c>[]</c>.</summary>
    public Task<ApiResponse> UpdateShippingRuleAsync(
        object shippingRuleId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(ShippingRulesUrls.ShippingRule,
            new RequestOptions
            {
                PathParams = Id(shippingRuleId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRule),
            }, cancellationToken);
    }

    /// <summary>Change only the fields sent - <c>PATCH /admin/shipping_rules/{shipping_rule_id}</c>. <c>zones</c> is the full
    /// list; <c>shipping_rate</c> or <c>shipping_class</c> <c>null</c> takes it off the rule, and the rate and zones stay.</summary>
    public Task<ApiResponse> PatchShippingRuleAsync(
        object shippingRuleId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(ShippingRulesUrls.ShippingRule,
            new RequestOptions
            {
                PathParams = Id(shippingRuleId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(ShippingRule),
            }, cancellationToken);
    }

    /// <summary>Delete one rule - <c>DELETE /admin/shipping_rules/{shipping_rule_id}</c>, <c>204</c>. Its rate and zones
    /// stay. A rule on a profile is <c>409</c> naming the profile.</summary>
    public Task<ApiResponse> DeleteShippingRuleAsync(
        object shippingRuleId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(ShippingRulesUrls.ShippingRule,
            new RequestOptions
            {
                PathParams = Id(shippingRuleId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object shippingRuleId) =>
        new() { ["shipping_rule_id"] = shippingRuleId };
}
