using WebCommander.Client;
using WebCommander.Resources.Dashboards.DashboardsDto;

namespace WebCommander.Resources.Dashboards;

/// <summary>Dashboard layouts - <c>/admin/dashboards*</c> - as a typed resource bound to one client.</summary>
public sealed class Dashboards : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Dashboards</c>.</summary>
    public Dashboards(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the admin's layouts - <c>GET /admin/dashboards</c>, <c>{"dashboards": [...]}</c> with no paging. Each
    /// row is <c>id</c>, <c>name</c>, <c>default</c> and the two timestamps, and exactly one row is the default.</summary>
    public Task<ApiResponse> ListDashboardsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DashboardsUrls.Dashboards,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DashboardListing),
            }, cancellationToken);

    /// <summary>Create a layout - <c>POST /admin/dashboards</c>, answered <c>201</c> with the whole layout, bare and with
    /// no <c>Location</c>. The body is bare and needs <c>name</c>, unique among the admin's layouts (<c>409</c>
    /// <c>name_taken</c>); an empty body is <c>400</c>. It starts as a copy of the layout named in <c>copy_from</c> or of
    /// the default one, then the sections and <c>kpis</c> sent are applied. Unknown fields are ignored.</summary>
    public Task<ApiResponse> CreateDashboardAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(DashboardsUrls.Dashboards,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Dashboard),
            }, cancellationToken);
    }

    /// <summary>Read one layout by its <c>id</c> - <c>GET /admin/dashboards/{dashboard_id}</c>, the layout bare. An
    /// unknown id is <c>404</c> <c>not_found</c> on <c>dashboard_id</c>.</summary>
    public Task<ApiResponse> GetDashboardAsync(
        string dashboardId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DashboardsUrls.Dashboard,
            new RequestOptions
            {
                PathParams = DashboardId(dashboardId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Dashboard),
            }, cancellationToken);

    /// <summary>Change only the fields sent - <c>PATCH /admin/dashboards/{dashboard_id}</c>, answered <c>200</c> with the
    /// whole layout. An empty body is <c>400</c>; unknown fields are ignored. A blank <c>name</c> or one over 60
    /// characters is <c>400</c>, a section key the layout does not have is <c>400</c>, a standard section with a
    /// <c>position</c> is <c>400</c> and hiding <c>contact</c> is <c>400</c>. <c>default</c> <c>true</c> moves the store's
    /// default layout to this one.</summary>
    public Task<ApiResponse> PatchDashboardAsync(
        string dashboardId, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(DashboardsUrls.Dashboard,
            new RequestOptions
            {
                PathParams = DashboardId(dashboardId),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Dashboard),
            }, cancellationToken);
    }

    /// <summary>Delete one layout - <c>DELETE /admin/dashboards/{dashboard_id}</c>, <c>204</c> with no body; the layout
    /// then reads <c>404</c>.</summary>
    public Task<ApiResponse> DeleteDashboardAsync(
        string dashboardId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(DashboardsUrls.Dashboard,
            new RequestOptions
            {
                PathParams = DashboardId(dashboardId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> DashboardId(string dashboardId) =>
        new() { ["dashboard_id"] = dashboardId };
}
