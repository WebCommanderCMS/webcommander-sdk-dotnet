using WebCommander.Client;
using WebCommander.Resources.CommonSettings.CommonSettingsDto;

namespace WebCommander.Resources.CommonSettings;

/// <summary>The store-wide settings and the registration form - <c>/settings</c> and <c>/create-customer-fields</c> -
/// as a typed resource bound to one client.</summary>
public sealed class CommonSettings : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.CommonSettings</c>.</summary>
    public CommonSettings(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the store's settings - <c>GET /settings</c>, <c>{"settings": {group: [{"config_key", "value"}]}}</c>.
    /// <c>type</c> names one group, an unknown one <c>404</c>; <c>config_key</c> one key, with or without <c>type</c>.
    /// A comma list of keys matches nothing and answers <c>{"settings": {}}</c>. Secrets read <c>***</c>. Other
    /// names, paging included, are <c>400</c>. Without <c>type</c> the answer is every group, about 2 MB, and can
    /// take 30 seconds or more.</summary>
    public Task<ApiResponse> GetSettingsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CommonSettingsUrls.Settings,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CommonSettingsDto.CommonSettings),
            }, cancellationToken);

    /// <summary>HEAD the settings - <c>HEAD /settings</c>; no body. It runs the read's query, so pass a
    /// <c>type</c>: an unknown one is <c>404</c>.</summary>
    public Task<ApiResponse> HeadSettingsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CommonSettingsUrls.Settings,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Save settings - <c>PUT /settings</c> with <c>{"settings": {group: ...}}</c>; <c>200</c> and every
    /// group of the store. Only the keys sent change, and one bad entry refuses the whole request with
    /// <c>400</c>. A write was measured at 5 to 53 seconds, past the default 30 of <c>timeoutSeconds</c> at the
    /// slow end: build the client with a longer one.</summary>
    public Task<ApiResponse> UpdateSettingsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(CommonSettingsUrls.Settings,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CommonSettingsDto.CommonSettings),
            }, cancellationToken);
    }

    /// <summary>Save settings - <c>POST /settings</c>, the <c>PUT</c>'s body, behaviour and timing, answered
    /// <c>201</c>. A <c>POST</c> is not resent after a timeout, so read the keys back before retrying.</summary>
    public Task<ApiResponse> SaveSettingsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(CommonSettingsUrls.Settings,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CommonSettingsDto.CommonSettings),
            }, cancellationToken);
    }

    /// <summary>Read the customer registration form - <c>GET /create-customer-fields</c>, the active <c>fields</c>
    /// in order and every field's <c>fields_configs</c>. Any query name is <c>400</c>.</summary>
    public Task<ApiResponse> GetCreateCustomerFieldsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CommonSettingsUrls.CreateCustomerFields,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CustomerFields),
            }, cancellationToken);

    /// <summary>HEAD the registration form - <c>HEAD /create-customer-fields</c>; no body.</summary>
    public Task<ApiResponse> HeadCreateCustomerFieldsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CommonSettingsUrls.CreateCustomerFields,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
}
