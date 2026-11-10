using WebCommander.Client;
using WebCommander.Resources.DashboardBanners.DashboardBannersDto;

namespace WebCommander.Resources.DashboardBanners;

/// <summary>Dashboard banners - <c>/admin/dashboard/banners*</c> and <c>/admin/dashboard/content_info</c> - as a typed
/// resource bound to one client.</summary>
public sealed class DashboardBanners : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.DashboardBanners</c>.</summary>
    public DashboardBanners(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List every banner, active or not - <c>GET /admin/dashboard/banners</c>, <c>{"banners": [...]}</c> in
    /// position order, positions counting from 1 without a gap.</summary>
    public Task<ApiResponse> ListDashboardBannersAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DashboardBannersUrls.DashboardBanners,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DashboardBannerListing),
            }, cancellationToken);

    /// <summary>Create a banner - <c>POST /admin/dashboard/banners</c>, answered <c>201</c> with the banner bare and no
    /// <c>Location</c>. The body is bare; every failing field is listed in one <c>400</c>, a <c>key</c> already used is
    /// <c>400</c> <c>duplicate</c>, an unknown field is <c>400</c>. It joins the Dashboard feed at once while
    /// <c>active</c>.</summary>
    public Task<ApiResponse> CreateDashboardBannerAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(DashboardBannersUrls.DashboardBanners,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DashboardBanner),
            }, cancellationToken);
    }

    /// <summary>Read one banner by its <c>key</c> - <c>GET /admin/dashboard/banners/{banner_key}</c>, the banner bare,
    /// hidden ones included. An unknown key is <c>404</c>.</summary>
    public Task<ApiResponse> GetDashboardBannerAsync(
        string bannerKey,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DashboardBannersUrls.DashboardBanner,
            new RequestOptions
            {
                PathParams = BannerKey(bannerKey),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DashboardBanner),
            }, cancellationToken);

    /// <summary>Change only the fields sent - <c>PATCH /admin/dashboard/banners/{banner_key}</c>, answered <c>200</c> with
    /// the banner. An empty body, a blank <c>title</c>, a <c>key</c> and an unknown field are <c>400</c>; an unknown
    /// banner is <c>404</c>. <c>learn_more</c> and <c>try_it</c> are replaced whole, and <c>try_it</c> <c>null</c> removes
    /// the second button. <c>updated_at</c> is not moved.</summary>
    public Task<ApiResponse> PatchDashboardBannerAsync(
        string bannerKey, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(DashboardBannersUrls.DashboardBanner,
            new RequestOptions
            {
                PathParams = BannerKey(bannerKey),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DashboardBanner),
            }, cancellationToken);
    }

    /// <summary>Delete one banner - <c>DELETE /admin/dashboard/banners/{banner_key}</c>, <c>204</c> with no body. The
    /// banners after it move up, so the positions stay without a gap; an unknown key is <c>404</c>.</summary>
    public Task<ApiResponse> DeleteDashboardBannerAsync(
        string bannerKey,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(DashboardBannersUrls.DashboardBanner,
            new RequestOptions
            {
                PathParams = BannerKey(bannerKey),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read what the Dashboard shows besides the figures - <c>GET /admin/dashboard/content_info</c>: the active
    /// banners under <c>banners</c>, the support card under <c>contact</c>, <c>online_store</c> and <c>member_label</c>. A
    /// hidden banner is not listed.</summary>
    public Task<ApiResponse> GetDashboardContentInfoAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DashboardBannersUrls.DashboardContentInfo,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DashboardContentInfo),
            }, cancellationToken);

    private static Dictionary<string, object?> BannerKey(string bannerKey) =>
        new() { ["banner_key"] = bannerKey };
}
