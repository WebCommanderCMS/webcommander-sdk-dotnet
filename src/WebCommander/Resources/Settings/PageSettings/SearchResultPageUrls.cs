namespace WebCommander.Resources.Settings.PageSettings;

/// <summary>Path templates for the search result page settings endpoints.</summary>
public static class SearchResultPageUrls
{
    public const string SearchResultPage =
        "/api/{api_version}/admin/settings/page_settings/search_result_page";

    public const string SearchResultPageSection =
        "/api/{api_version}/admin/settings/page_settings/search_result_page/{section}";

    public const string SearchResultPageProductPromotions =
        "/api/{api_version}/admin/settings/page_settings/search_result_page/product_promotion";

    public const string SearchResultPageProductPromotion =
        "/api/{api_version}/admin/settings/page_settings/search_result_page/product_promotion/{promotion_id}";

    public const string SearchResultPageProductPromotionActive =
        "/api/{api_version}/admin/settings/page_settings/search_result_page/product_promotion/{promotion_id}/active";
}
