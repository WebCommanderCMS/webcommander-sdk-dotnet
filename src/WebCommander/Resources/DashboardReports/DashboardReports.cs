using WebCommander.Client;
using WebCommander.Resources.DashboardReports.DashboardReportsDto;

namespace WebCommander.Resources.DashboardReports;

/// <summary>Dashboard reports - <c>/admin/dashboard/reports/*</c> - as a typed resource bound to one client. Every report
/// takes the same period query: <c>duration</c> (<c>today</c>, <c>yesterday</c>, <c>last_24_hours</c>,
/// <c>last_7_days</c>, <c>last_30_days</c>, <c>this_month</c>, <c>last_month</c>, <c>last_3_months</c>,
/// <c>last_6_months</c>, <c>last_12_months</c>, <c>last_24_months</c>, <c>last_36_months</c>, <c>last_48_months</c>,
/// <c>last_60_months</c> or <c>custom</c>; <c>last_30_days</c> when left out) or <c>start_date</c> with <c>end_date</c>
/// (<c>YYYY-MM-DD</c>, sent together, not reversed and not in the future). A bad value, a repeated <c>duration</c> and a
/// lone date are <c>400</c> with every bad field listed; a name the report does not know is ignored.</summary>
public sealed class DashboardReports : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.DashboardReports</c>.</summary>
    public DashboardReports(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the quick report - <c>GET /admin/dashboard/reports/quick</c>: <c>period</c>, <c>kpis</c>, grouped
    /// totals, the most viewed pages under <c>view_report</c> and <c>trends</c>. <c>kpis</c> is a comma list of metric keys
    /// (an unknown one is <c>400</c>) that narrows <c>kpis</c> to those, in that order; <c>entity_name</c> is <c>page</c>
    /// (the default) or <c>product</c> and picks what <c>view_report</c> ranks.</summary>
    public Task<ApiResponse> GetDashboardReportQuickAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DashboardReportsUrls.DashboardReportQuick,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(QuickReport),
            }, cancellationToken);

    /// <summary>Ask whether the quick report is reachable - <c>HEAD /admin/dashboard/reports/quick</c>; the status of the
    /// read, with no body. It runs the read's query, so a bad value is <c>400</c>.</summary>
    public Task<ApiResponse> HeadDashboardReportQuickAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", DashboardReportsUrls.DashboardReportQuick,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read the sales report - <c>GET /admin/dashboard/reports/sales</c>: <c>rows</c> by day or month and
    /// <c>totals</c>. <c>breakdown</c> (<c>true</c> or <c>false</c>) adds orders, shipping, tax, discount, refunds and the
    /// top product to each row and total; <c>sort</c> is <c>date</c> (the default) or <c>sales</c> and <c>order</c> is
    /// <c>asc</c> (the default) or <c>desc</c>; any other value is <c>400</c>.</summary>
    public Task<ApiResponse> GetDashboardReportSalesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DashboardReportsUrls.DashboardReportSales,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SalesReport),
            }, cancellationToken);

    /// <summary>Ask whether the sales report is reachable - <c>HEAD /admin/dashboard/reports/sales</c>; the status of the
    /// read, with no body. It runs the read's query, so a bad value is <c>400</c>.</summary>
    public Task<ApiResponse> HeadDashboardReportSalesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", DashboardReportsUrls.DashboardReportSales,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read the summary - <c>GET /admin/dashboard/reports/summary</c>: the quick report without <c>kpis</c>,
    /// orders by month, sales rows, the most visited and best selling products and the order updates, in one answer.
    /// <c>compare_previous</c> (<c>true</c> or <c>false</c>) is checked as a boolean; any other value is
    /// <c>400</c>.</summary>
    public Task<ApiResponse> GetDashboardReportSummaryAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DashboardReportsUrls.DashboardReportSummary,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(SummaryReport),
            }, cancellationToken);

    /// <summary>Ask whether the summary is reachable - <c>HEAD /admin/dashboard/reports/summary</c>; the status of the
    /// read, with no body. It runs the read's query, so a bad value is <c>400</c>.</summary>
    public Task<ApiResponse> HeadDashboardReportSummaryAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", DashboardReportsUrls.DashboardReportSummary,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
}
