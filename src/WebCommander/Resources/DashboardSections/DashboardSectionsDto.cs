namespace WebCommander.Resources.DashboardSections.DashboardSectionsDto;

/// <summary>The period a section covers and the one before it, as dates; <c>time_zone</c> is the store's and
/// <c>currency</c> the code money figures are in.</summary>
public sealed class SectionPeriod : Dto
{
    public string? Duration { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public string? PreviousStartDate { get; set; }

    public string? PreviousEndDate { get; set; }

    public string? TimeZone { get; set; }

    public string? Currency { get; set; }
}

/// <summary>A product, page or post by id and name.</summary>
public sealed class SectionEntity : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>What needs attention now; <c>pending_fulfillment</c> is the orders waiting to ship.</summary>
public sealed class SectionOrderUpdates : Dto
{
    public long? LowStockItems { get; set; }

    public long? PendingFulfillment { get; set; }

    public long? PaymentsToCapture { get; set; }
}

/// <summary>A product and the units sold, highest first.</summary>
public sealed class SectionBestSeller : Dto
{
    public SectionEntity? Product { get; set; }

    public long? UnitsSold { get; set; }
}

/// <summary>One content item to review; <c>count</c> is an empty string on the rows measured.</summary>
public sealed class SectionContentUpdate : Dto
{
    public string? Key { get; set; }

    public string? Label { get; set; }

    public string? Count { get; set; }
}

/// <summary>A page and its views.</summary>
public sealed class SectionTopPage : Dto
{
    public SectionEntity? Page { get; set; }

    public long? Views { get; set; }
}

/// <summary>The bookings that need attention: how many, and the latest of them.</summary>
public sealed class SectionNeedsAttention : Dto
{
    public long? Count { get; set; }

    /// <summary>Empty on the store measured, so the row's shape is not known.</summary>
    public IReadOnlyList<object?>? Latest { get; set; }
}

/// <summary>A blog draft waiting to be published.</summary>
public sealed class SectionDraft : Dto
{
    public SectionEntity? Post { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The blog drafts to publish: how many, and the latest.</summary>
public sealed class SectionDrafts : Dto
{
    public long? Count { get; set; }

    public IReadOnlyList<SectionDraft?>? Latest { get; set; }
}

/// <summary>A blog post and its views.</summary>
public sealed class SectionViewedPost : Dto
{
    public SectionEntity? Post { get; set; }

    public long? Views { get; set; }
}

/// <summary>One tab of the Dashboard. <c>section</c> echoes the path value, and only the fields of that tab are
/// present. <c>ecommerce</c>: <c>order_updates</c>, <c>best_selling_products</c>. <c>content</c>:
/// <c>content_updates</c>, <c>top_pages</c>. <c>bookings</c>: <c>needs_attention</c>, <c>upcoming</c>.
/// <c>courses</c>: <c>new_enrolments</c>, <c>unpaid_orders</c>, <c>popular_courses</c>. <c>blog</c>:
/// <c>comments_to_moderate</c>, <c>drafts_to_publish</c>, <c>most_viewed_posts</c>.</summary>
public sealed class DashboardSectionReport : Dto
{
    public string? Section { get; set; }

    public SectionPeriod? Period { get; set; }

    public SectionOrderUpdates? OrderUpdates { get; set; }

    public IReadOnlyList<SectionBestSeller?>? BestSellingProducts { get; set; }

    public IReadOnlyList<SectionContentUpdate?>? ContentUpdates { get; set; }

    public IReadOnlyList<SectionTopPage?>? TopPages { get; set; }

    public SectionNeedsAttention? NeedsAttention { get; set; }

    /// <summary>Empty on the store measured, so the row's shape is not known.</summary>
    public IReadOnlyList<object?>? Upcoming { get; set; }

    public long? NewEnrolments { get; set; }

    public long? UnpaidOrders { get; set; }

    public IReadOnlyList<object?>? PopularCourses { get; set; }

    public long? CommentsToModerate { get; set; }

    public SectionDrafts? DraftsToPublish { get; set; }

    public IReadOnlyList<SectionViewedPost?>? MostViewedPosts { get; set; }
}
