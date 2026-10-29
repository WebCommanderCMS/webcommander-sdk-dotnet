using WebCommander.Client;

namespace WebCommander.Resources.TaxSettings;

/// <summary>The flat tax settings facade - <c>/admin/tax/settings</c> - as a typed resource bound to one client.</summary>
public sealed class TaxSettings : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.TaxSettings</c>.</summary>
    public TaxSettings(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the tax settings - <c>GET /admin/tax/settings</c>.</summary>
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

    /// <summary>Write the tax settings - <c>PUT /admin/tax/settings</c>, a merge wrapped in <c>settings</c>.</summary>
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
}
