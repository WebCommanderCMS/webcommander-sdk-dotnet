namespace WebCommander.Resources.Trash;

/// <summary>Path templates for the trash endpoints.</summary>
public static class TrashUrls
{
    public const string Trash = "/api/{api_version}/admin/trash";

    public const string TrashTypes = "/api/{api_version}/admin/trash/types";

    public const string TrashItem = "/api/{api_version}/admin/trash/{trash_type}/{trash_item_id}";

    public const string TrashItemRestore =
        "/api/{api_version}/admin/trash/{trash_type}/{trash_item_id}/restore";

    public const string TrashBulkRestore = "/api/{api_version}/admin/trash/bulk-restore";

    public const string TrashBulkDelete = "/api/{api_version}/admin/trash/bulk-delete";
}
