using WebCommander.Client;

namespace WebCommander.Resources.TaxSettings;

/// <summary>The tax settings - <c>/admin/settings/tax</c> - as a typed resource bound to one client.</summary>
public sealed class TaxSettings : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.TaxSettings</c>.</summary>
    public TaxSettings(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the tax settings - <c>GET /admin/settings/tax</c>.</summary>
    public Task<ApiResponse> GetTaxSettingsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TaxSettingsUrls.TaxSettings,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxSettingsDto.TaxSettings),
            }, cancellationToken);

    /// <summary>Write the tax settings - <c>PUT /admin/settings/tax</c>, a merge wrapped in <c>tax_settings</c>.</summary>
    public Task<ApiResponse> PutTaxSettingsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(TaxSettingsUrls.TaxSettings,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxSettingsDto.TaxSettings),
            }, cancellationToken);
    }

    /// <summary>Write one field or several - <c>PATCH /admin/settings/tax</c>, wrapped in <c>tax_settings</c>.</summary>
    public Task<ApiResponse> PatchTaxSettingsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(TaxSettingsUrls.TaxSettings,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TaxSettingsDto.TaxSettings),
            }, cancellationToken);
    }

    /// <summary>Ask whether the tax settings are reachable - <c>HEAD /admin/settings/tax</c>; no body.</summary>
    public Task<ApiResponse> HeadTaxSettingsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TaxSettingsUrls.TaxSettings,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
}
