namespace WebCommander.Resources.DashboardKpis.DashboardKpisDto;

/// <summary>The default layout the KPI tiles belong to; <c>id</c> is its <c>dashboard_id</c>.</summary>
public sealed class DashboardRef : Dto
{
    public string? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>The period a figure covers and the one before it, as dates; <c>time_zone</c> is the store's and
/// <c>currency</c> the code money figures are in.</summary>
public sealed class KpiPeriod : Dto
{
    public string? Duration { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public string? PreviousStartDate { get; set; }

    public string? PreviousEndDate { get; set; }

    public string? TimeZone { get; set; }

    public string? Currency { get; set; }
}

/// <summary>One metric a tile can show. <c>key</c> is what a selection takes; <c>code</c> is the report's own name
/// for it and differs for some, such as <c>aov</c> and <c>average_order_value</c>.</summary>
public sealed class KpiMetric : Dto
{
    public string? Key { get; set; }

    public string? Code { get; set; }

    public string? Label { get; set; }

    public string? Format { get; set; }

    public bool? InUse { get; set; }
}

/// <summary>A group of metrics: <c>ecommerce</c>, <c>content</c>, <c>blog</c>, <c>bookings</c> or
/// <c>courses</c>.</summary>
public sealed class KpiGroup : Dto
{
    public string? Key { get; set; }

    public string? Label { get; set; }

    public IReadOnlyList<KpiMetric?>? Metrics { get; set; }
}

/// <summary>Every metric a tile can show. <c>min</c> and <c>max</c> bound the number of tiles, and <c>selected</c>
/// lists the keys in use, in tile order.</summary>
public sealed class KpiOptions : Dto
{
    public DashboardRef? Dashboard { get; set; }

    public long? Min { get; set; }

    public long? Max { get; set; }

    public IReadOnlyList<string?>? Selected { get; set; }

    public IReadOnlyList<KpiGroup?>? Groups { get; set; }
}

/// <summary>One selected tile with its figure. <c>format</c> is <c>money</c>, <c>count</c> or <c>percent</c> and
/// <c>direction</c> is <c>up</c>, <c>down</c> or <c>flat</c>; <c>change_percent</c> is <c>null</c> when the previous
/// period was zero. Every count is a float on the wire.</summary>
public sealed class KpiTile : Dto
{
    public long? Position { get; set; }

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

/// <summary>The tiles in use with their figures for the period; the read and the save answer alike.</summary>
public sealed class KpiSelection : Dto
{
    public DashboardRef? Dashboard { get; set; }

    public bool? OnlineStore { get; set; }

    public KpiPeriod? Period { get; set; }

    public IReadOnlyList<KpiTile?>? Selected { get; set; }
}

/// <summary>The save body, bare: <c>metrics</c> is the full list of 2 to 5 distinct metric keys, in tile order. It
/// replaces the default layout's tiles.</summary>
public sealed class KpiSelectionInput : Dto
{
    public IReadOnlyList<string?>? Metrics { get; set; }
}
