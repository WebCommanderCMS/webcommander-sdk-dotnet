using WebCommander.Client;
using WebCommander.Resources.DashboardKpis.DashboardKpisDto;

namespace WebCommander.Resources.DashboardKpis;

/// <summary>Dashboard KPI tiles - <c>/admin/dashboard/kpi_options</c> and <c>/admin/dashboard/kpi_selection</c> - as a
/// typed resource bound to one client.</summary>
public sealed class DashboardKpis : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.DashboardKpis</c>.</summary>
    public DashboardKpis(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the metrics a tile can show - <c>GET /admin/dashboard/kpi_options</c>, grouped, with <c>min</c> 2 and
    /// <c>max</c> 5 tiles and the keys now in use under <c>selected</c>. It reads the same tiles as
    /// <c>kpi_selection</c>.</summary>
    public Task<ApiResponse> GetDashboardKpiOptionsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DashboardKpisUrls.DashboardKpiOptions,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(KpiOptions),
            }, cancellationToken);

    /// <summary>Read the tiles in use with their figures - <c>GET /admin/dashboard/kpi_selection</c>, the tiles of the
    /// default layout under <c>selected</c> in position order. <c>duration</c> (<c>last_30_days</c> when left out) or
    /// <c>start_date</c> with <c>end_date</c> (<c>YYYY-MM-DD</c>, sent together) choose the period; a bad value is
    /// <c>400</c>.</summary>
    public Task<ApiResponse> GetDashboardKpiSelectionAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DashboardKpisUrls.DashboardKpiSelection,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(KpiSelection),
            }, cancellationToken);

    /// <summary>Save the tiles - <c>PUT /admin/dashboard/kpi_selection</c> with <c>{"metrics": [...]}</c>, answered
    /// <c>200</c> like the read. The list replaces the default layout's tiles, which is store-wide state, and is read
    /// back at once by that layout and by <c>kpi_options</c>. Fewer than 2 or more than 5, a repeat, an unknown key, a
    /// missing <c>metrics</c> and a non-list are <c>400</c>. <c>duration</c> or the dates in the query set the period of
    /// the figures in the answer.</summary>
    public Task<ApiResponse> UpdateDashboardKpiSelectionAsync(
        object payload, IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPutRequestAsync(DashboardKpisUrls.DashboardKpiSelection,
            new RequestOptions
            {
                JsonBody = payload,
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(KpiSelection),
            }, cancellationToken);
    }
}
