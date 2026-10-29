using WebCommander.Client;
using WebCommander.Resources.Trash.TrashDto;

namespace WebCommander.Resources.Trash;

/// <summary>The trash - <c>/admin/trash*</c> - as a typed resource bound to one client.</summary>
public sealed class Trash : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Trash</c>.</summary>
    public Trash(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List trashed rows, newest change first - <c>GET /admin/trash</c>. <c>type</c> (a <c>types</c> value,
    /// any case, or a comma list), <c>search</c> or <c>searchText</c>, <c>sort</c> <c>name</c>, <c>type</c>,
    /// <c>updated</c> or <c>removed_at</c> with <c>order</c> or <c>dir</c>, <c>limit</c> or <c>max</c> (above 100 is
    /// <c>400</c>), <c>offset</c>, <c>page</c>, <c>removed_from</c> and <c>removed_to</c>; any other name is <c>400</c>.
    /// Pass a <c>type</c>: an untyped page can outlast the default timeout.</summary>
    public Task<ApiResponse> ListTrashAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TrashUrls.Trash,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TrashPage),
            }, cancellationToken);

    /// <summary>HEAD the listing - <c>HEAD /admin/trash</c>; no body. It takes the listing's query names and runs
    /// the same query, so pass a <c>type</c> as on the <c>GET</c>.</summary>
    public Task<ApiResponse> HeadTrashAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TrashUrls.Trash,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>List the types the trash holds - <c>GET /admin/trash/types</c>, snake_case names such as
    /// <c>category</c>, <c>customer</c> and <c>blog_post</c>. Any query name is <c>400</c>.</summary>
    public Task<ApiResponse> ListTrashTypesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TrashUrls.TrashTypes,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TrashTypeList),
            }, cancellationToken);

    /// <summary>HEAD the type list - <c>HEAD /admin/trash/types</c>; no body.</summary>
    public Task<ApiResponse> HeadTrashTypesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TrashUrls.TrashTypes,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Restore one trashed row - <c>POST /admin/trash/{trash_type}/{trash_item_id}/restore</c>, answered
    /// <c>200</c> with the row under <c>item</c>; sends no body. A live row is <c>409</c>, an id the type does not
    /// hold <c>404</c>.</summary>
    public Task<ApiResponse> RestoreTrashItemAsync(
        string trashType, object trashItemId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendPostRequestAsync(TrashUrls.TrashItemRestore,
            new RequestOptions
            {
                PathParams = Item(trashType, trashItemId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TrashItem),
            }, cancellationToken);

    /// <summary>Delete one trashed row for good - <c>DELETE /admin/trash/{trash_type}/{trash_item_id}</c>;
    /// <c>204</c>. A live row is <c>409</c>; once purged, every read and a second delete are <c>404</c>.</summary>
    public Task<ApiResponse> DeleteTrashItemAsync(
        string trashType, object trashItemId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(TrashUrls.TrashItem,
            new RequestOptions
            {
                PathParams = Item(trashType, trashItemId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Restore up to 100 rows - <c>POST /admin/trash/bulk-restore</c> with <c>{"items": [{"type", "id"}]}</c>.
    /// <c>200</c> when every item is restored, <c>207</c> with the same body when any fails.</summary>
    public Task<ApiResponse> BulkRestoreTrashAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(TrashUrls.TrashBulkRestore,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TrashBulkRestoreResult),
            }, cancellationToken);
    }

    /// <summary>Delete up to 100 trashed rows for good - <c>POST /admin/trash/bulk-delete</c>, the bulk restore's
    /// body. <c>200</c> when every item is deleted, <c>207</c> with the same body when any fails.</summary>
    public Task<ApiResponse> BulkDeleteTrashAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(TrashUrls.TrashBulkDelete,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TrashBulkDeleteResult),
            }, cancellationToken);
    }

    private static Dictionary<string, object?> Item(string trashType, object trashItemId) =>
        new() { ["trash_type"] = trashType, ["trash_item_id"] = trashItemId };
}
