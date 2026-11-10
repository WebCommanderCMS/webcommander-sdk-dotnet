namespace WebCommander.Resources.Dashboards.DashboardsDto;

/// <summary>One part of the <c>seo</c> section. Read back, the title and <c>can_hide</c> come with it; a write needs
/// only <c>key</c> and <c>visible</c>. The <c>kpis</c> part cannot be hidden.</summary>
public sealed class DashboardPart : Dto
{
    public string? Key { get; set; }

    public string? Title { get; set; }

    public bool? Visible { get; set; }

    public bool? CanHide { get; set; }
}

/// <summary>One tab of the <c>section_tabs</c> section: <c>ecommerce</c>, <c>content</c>, <c>bookings</c>,
/// <c>courses</c> or <c>blog</c>. A write needs <c>key</c> and sets <c>visible</c> or <c>position</c>, counted from 1
/// among the tabs.</summary>
public sealed class DashboardTab : Dto
{
    public string? Key { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool? Visible { get; set; }

    public bool? CanHide { get; set; }

    public long? Position { get; set; }
}

/// <summary>A section of a layout. The standard ones are <c>announcement</c>, <c>performance</c>, <c>activity</c> and
/// <c>contact</c>; they only show or hide, and <c>contact</c> cannot hide. The customisable ones are
/// <c>setup_guide</c>, <c>section_tabs</c> (with <c>tabs</c>) and <c>seo</c> (with <c>parts</c>); they also take a
/// <c>position</c>. Read back, the title, description and <c>can_hide</c> come with it; a write needs only <c>key</c>
/// and what changes.</summary>
public sealed class DashboardSection : Dto
{
    public string? Key { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool? Visible { get; set; }

    public bool? CanHide { get; set; }

    public long? Position { get; set; }

    public IReadOnlyList<DashboardTab?>? Tabs { get; set; }

    public IReadOnlyList<DashboardPart?>? Parts { get; set; }
}

/// <summary>One KPI tile of a layout, in tile order. <c>key</c> is a <c>kpi_options</c> metric key.</summary>
public sealed class DashboardKpi : Dto
{
    public long? Position { get; set; }

    public string? Key { get; set; }

    public string? Label { get; set; }

    public string? Group { get; set; }
}

/// <summary>One layout. <c>id</c> is the <c>dashboard_id</c> of the other calls, a UUID. Exactly one layout is the
/// default.</summary>
public sealed class Dashboard : Dto
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public bool? Default { get; set; }

    public IReadOnlyList<DashboardSection?>? StandardSections { get; set; }

    public IReadOnlyList<DashboardSection?>? CustomisableSections { get; set; }

    public IReadOnlyList<DashboardKpi?>? Kpis { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>A layout as the listing shows it, without its sections and tiles.</summary>
public sealed class DashboardSummary : Dto
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public bool? Default { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The listing: every layout of the admin, with no paging.</summary>
public sealed class DashboardListing : Dto
{
    public IReadOnlyList<DashboardSummary?>? Dashboards { get; set; }
}

/// <summary>The write body, bare. A create needs <c>name</c>, unique among the admin's layouts (<c>409</c>); it
/// starts as a copy of the <c>copy_from</c> layout, the default one when that is left out. A patch sends only what
/// changes; the sections named are merged into the layout, the rest is kept. <c>kpis</c> is the full list of 2 to 5
/// metric keys in tile order. A standard section takes no <c>position</c> (<c>400</c>). <c>default</c> <c>true</c>
/// makes this layout the store's default layout; <c>false</c> on the default layout is <c>409</c>, as the default
/// moves only by making another layout the default.</summary>
public sealed class DashboardInput : Dto
{
    public string? Name { get; set; }

    public bool? Default { get; set; }

    public string? CopyFrom { get; set; }

    public IReadOnlyList<DashboardSection?>? StandardSections { get; set; }

    public IReadOnlyList<DashboardSection?>? CustomisableSections { get; set; }

    public IReadOnlyList<string?>? Kpis { get; set; }
}
