using WebCommander.Client;
using WebCommander.Resources.Aliases.AliasesDto;

namespace WebCommander.Resources.Aliases;

/// <summary>Domain aliases - <c>/admin/aliases*</c> - as a typed resource bound to one client.</summary>
public sealed class Aliases : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Aliases</c>.</summary>
    public Aliases(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the aliases - <c>GET /admin/aliases</c>.</summary>
    public Task<ApiResponse> ListAliasesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(AliasesUrls.Aliases,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AliasPage),
            }, cancellationToken);

    /// <summary>HEAD the alias collection - <c>HEAD /admin/aliases</c>.</summary>
    public Task<ApiResponse> HeadAliasesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", AliasesUrls.Aliases,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Create an alias - <c>POST /admin/aliases</c>, a bare <c>{"alias": "&lt;hostname&gt;"}</c>.</summary>
    public Task<ApiResponse> CreateAliasAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(AliasesUrls.Aliases,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DomainAlias),
            }, cancellationToken);
    }

    /// <summary>Delete one alias - <c>DELETE /admin/aliases/{alias_id}</c>.</summary>
    public Task<ApiResponse> DeleteAliasAsync(
        object aliasId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(AliasesUrls.Alias,
            new RequestOptions
            {
                PathParams = Id(aliasId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object aliasId) =>
        new() { ["alias_id"] = aliasId };
}
