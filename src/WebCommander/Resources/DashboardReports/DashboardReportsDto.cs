namespace WebCommander.Resources.DashboardReports.DashboardReportsDto;

/// <summary>The period a report covers and the one before it, as dates; <c>time_zone</c> is the store's and
/// <c>currency</c> the code money figures are in. <c>duration</c> is <c>custom</c> for a date range.</summary>
public sealed class ReportPeriod : Dto
{
    public string? Duration { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public string? PreviousStartDate { get; set; }

    public string? PreviousEndDate { get; set; }

    public string? TimeZone { get; set; }

    public string? Currency { get; set; }
}

/// <summary>One figure with the previous period's. <c>format</c> is <c>money</c>, <c>count</c> or <c>percent</c> and
/// <c>direction</c> is <c>up</c>, <c>down</c> or <c>flat</c>; <c>change_percent</c> is <c>null</c> when the previous
/// value was zero. In <c>trends</c> there is no <c>group</c>.</summary>
public sealed class ReportKpi : Dto
{
    public string? Key { get; set; }

    public string? Code { get; set; }

    public string? Label { get; set; }

    public string? Group { get; set; }

    public string? Format { get; set; }

    public double? Value { get; set; }

    public double? PreviousValue { get; set; }

    public double? ChangePercent { get; set; }

    public string? Direction { get; set; }
}

/// <summary>Order counts of the period.</summary>
public sealed class ReportOrders : Dto
{
    public long? Count { get; set; }

    public long? Pending { get; set; }

    public long? Cancelled { get; set; }
}

/// <summary>Sales figures of the period, rounded to 2 decimals.</summary>
public sealed class ReportSales : Dto
{
    public double? Subtotal { get; set; }

    public double? Total { get; set; }

    public long? ItemsSold { get; set; }

    public double? AverageOrderValue { get; set; }
}

/// <summary>Customers registered in the period.</summary>
public sealed class ReportCustomers : Dto
{
    public long? New { get; set; }
}

/// <summary>Products added in the period, and those out of stock now.</summary>
public sealed class ReportProducts : Dto
{
    public long? New { get; set; }

    public long? OutOfStock { get; set; }
}

/// <summary>Page views of the period.</summary>
public sealed class ReportPageViews : Dto
{
    public long? Total { get; set; }

    public long? Unique { get; set; }
}

/// <summary>A page, product or post by id and name.</summary>
public sealed class ReportEntity : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>One row of the most viewed list: a <c>page</c> or, with <c>entity_name</c> <c>product</c>, a
/// <c>product</c>.</summary>
public sealed class ReportViewItem : Dto
{
    public ReportEntity? Page { get; set; }

    public ReportEntity? Product { get; set; }

    public long? Views { get; set; }
}

/// <summary>The most viewed pages, or products when the query's <c>entity_name</c> is <c>product</c>; <c>entity</c>
/// says which.</summary>
public sealed class ReportViews : Dto
{
    public string? Entity { get; set; }

    public IReadOnlyList<ReportViewItem?>? Items { get; set; }
}

/// <summary>The quick report. <c>kpis</c> holds the 27 metrics, or only the ones the query's <c>kpis</c> names, in
/// that order. Inside a summary only the grouped totals and <c>view_report</c> are present.</summary>
public sealed class QuickReport : Dto
{
    public ReportPeriod? Period { get; set; }

    public IReadOnlyList<ReportKpi?>? Kpis { get; set; }

    public ReportOrders? Orders { get; set; }

    public ReportSales? Sales { get; set; }

    public ReportCustomers? Customers { get; set; }

    public ReportProducts? Products { get; set; }

    public ReportPageViews? PageViews { get; set; }

    public ReportViews? ViewReport { get; set; }

    public IReadOnlyList<ReportKpi?>? Trends { get; set; }
}

/// <summary>One row of the sales report: a day for a period up to a month, else a month. Without <c>breakdown</c>
/// only the date fields, <c>label</c>, <c>month_name</c>, <c>sales</c> and <c>total_sales</c> are present; <c>day</c>
/// is present on day rows only.</summary>
public sealed class SalesRow : Dto
{
    public string? Date { get; set; }

    public long? Year { get; set; }

    public long? Month { get; set; }

    public long? Day { get; set; }

    public string? Label { get; set; }

    public string? MonthName { get; set; }

    public double? Sales { get; set; }

    public double? TotalSales { get; set; }

    public long? Orders { get; set; }

    public double? OrdersValue { get; set; }

    public double? GrossSales { get; set; }

    public double? Shipping { get; set; }

    public double? Handling { get; set; }

    public double? Tax { get; set; }

    public double? Discount { get; set; }

    public double? Refunds { get; set; }

    public double? AverageOrderValue { get; set; }

    public ReportEntity? TopProduct { get; set; }
}

/// <summary>The rows added up; <c>sales</c> always, the rest with <c>breakdown</c>.</summary>
public sealed class SalesTotals : Dto
{
    public double? Sales { get; set; }

    public long? Orders { get; set; }

    public double? OrdersValue { get; set; }

    public double? GrossSales { get; set; }

    public double? Shipping { get; set; }

    public double? Handling { get; set; }

    public double? Tax { get; set; }

    public double? Discount { get; set; }

    public double? Refunds { get; set; }

    public double? AverageOrderValue { get; set; }
}

/// <summary>The sales report. <c>group_by</c> is <c>day</c> for a period up to a month and <c>month</c> for a longer
/// one; <c>sort</c> (<c>date</c> or <c>sales</c>) and <c>order</c> (<c>asc</c> or <c>desc</c>) echo what was applied.
/// A period with no sales has no rows.</summary>
public sealed class SalesReport : Dto
{
    public ReportPeriod? Period { get; set; }

    public string? GroupBy { get; set; }

    public string? Sort { get; set; }

    public string? Order { get; set; }

    public IReadOnlyList<SalesRow?>? Rows { get; set; }

    public SalesTotals? Totals { get; set; }
}

/// <summary>Orders placed in one month.</summary>
public sealed class OrderCount : Dto
{
    public string? Date { get; set; }

    public long? Year { get; set; }

    public long? Month { get; set; }

    public long? Count { get; set; }
}

/// <summary>A product and its views.</summary>
public sealed class MostVisitedProduct : Dto
{
    public ReportEntity? Product { get; set; }

    public long? Views { get; set; }
}

/// <summary>A product and the units sold, highest first.</summary>
public sealed class BestSellingProduct : Dto
{
    public ReportEntity? Product { get; set; }

    public long? UnitsSold { get; set; }
}

/// <summary>What needs attention now; <c>pending_fulfillment</c> is the orders waiting to ship.</summary>
public sealed class OrderUpdates : Dto
{
    public long? LowStockItems { get; set; }

    public long? PendingFulfillment { get; set; }

    public long? PaymentsToCapture { get; set; }
}

/// <summary>The summary: every section of the Dashboard in one answer. <c>quick_report</c> has no
/// <c>kpis</c>.</summary>
public sealed class SummaryReport : Dto
{
    public ReportPeriod? Period { get; set; }

    public QuickReport? QuickReport { get; set; }

    public IReadOnlyList<OrderCount?>? OrderByMonth { get; set; }

    public IReadOnlyList<OrderCount?>? OrdersOverTime { get; set; }

    public IReadOnlyList<SalesRow?>? SalesReport { get; set; }

    public IReadOnlyList<MostVisitedProduct?>? MostVisitedProducts { get; set; }

    public IReadOnlyList<BestSellingProduct?>? BestSellingProducts { get; set; }

    public OrderUpdates? OrderUpdates { get; set; }
}
