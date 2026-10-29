namespace WebCommander.Resources.Settings.SearchSettings;

/// <summary>Path templates for the search settings endpoints.</summary>
public static class SearchSettingsUrls
{
    public const string SearchSettings =
        "/api/{api_version}/admin/settings/search_settings";

    public const string SearchSettingsSection =
        "/api/{api_version}/admin/settings/search_settings/{section}";

    /// <summary>The read-only sub-resource beside the sections, reached by a path of its own.</summary>
    public const string SearchSettingsReindexStatus =
        "/api/{api_version}/admin/settings/search_settings/reindex_status";
}
