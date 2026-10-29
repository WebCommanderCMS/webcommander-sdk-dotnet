namespace WebCommander.Resources.Pages.PagesDto;

/// <summary>An <c>{id, name}</c> pair: the layout, an access-list customer or group, the navigation and the
/// author. A write names one by <c>{"id": ...}</c>; <see cref="Name"/> is read-only.</summary>
public sealed class PageReference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>The password gate. A listing row masks the password as <c>***</c>; the row read and the writes
/// answer it as saved, and switching it off keeps the stored password.</summary>
public sealed class PagePasswordProtection : Dto
{
    public bool? Enabled { get; set; }

    public string? Password { get; set; }
}

/// <summary>Whether the page is linked from a navigation; <see cref="Navigation"/> is <c>null</c> when it is
/// not, and must be <c>{"id": ...}</c> when a write sends <see cref="Enabled"/> true.</summary>
public sealed class PageMenuPlacement : Dto
{
    public bool? Enabled { get; set; }

    public PageReference? Navigation { get; set; }
}

/// <summary>The draft and approval state; the draft and published ids are <c>null</c>, never <c>0</c>.</summary>
public sealed class PagePublishing : Dto
{
    public bool? Draft { get; set; }

    public long? DraftPageId { get; set; }

    public long? PublishedPageId { get; set; }

    public bool? PendingApproval { get; set; }

    public long? PendingApprovalCount { get; set; }

    public IReadOnlyList<long?>? PendingApprovalIds { get; set; }

    public bool? Disposable { get; set; }
}

/// <summary>One page: a listing row, the row read and every write's answer, the same 23 keys.
/// <see cref="Visibility"/> is <c>open</c>, <c>hidden</c> or <c>restricted</c>; <see cref="VisibleTo"/> is
/// <c>all</c> or <c>selected</c> on a restricted page and <c>null</c> otherwise.</summary>
public sealed class Page : Dto
{
    protected override string? EnvelopeKey => "page";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Subtitle { get; set; }

    public string? Url { get; set; }

    public string? FullUrl { get; set; }

    public PageReference? Layout { get; set; }

    public string? Visibility { get; set; }

    public string? VisibleTo { get; set; }

    public IReadOnlyList<PageReference?>? Customers { get; set; }

    public IReadOnlyList<PageReference?>? CustomerGroups { get; set; }

    public string? BodyClass { get; set; }

    public PagePasswordProtection? PasswordProtection { get; set; }

    public PageMenuPlacement? MenuPlacement { get; set; }

    public bool? Active { get; set; }

    public bool? DisableTracking { get; set; }

    public bool? Landing { get; set; }

    public bool? InTrash { get; set; }

    public PagePublishing? Publishing { get; set; }

    /// <summary><c>object</c>: <c>null</c> on every page the store holds, so no store-bound page could be read
    /// to type it.</summary>
    public object? Store { get; set; }

    public PageReference? Author { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The writable fields, wrapped in <c>page</c>, for the create, the <c>PUT</c> replace and the
/// <c>PATCH</c>. A <c>null</c> field is not sent: to clear one, pass a map.</summary>
public sealed class PageInput : Dto
{
    protected override string? EnvelopeKey => "page";

    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Subtitle { get; set; }

    public string? Url { get; set; }

    public PageReference? Layout { get; set; }

    public string? Visibility { get; set; }

    public string? VisibleTo { get; set; }

    /// <summary>Public <c>customer_id</c> values, bare or as a <see cref="PageReference"/>.</summary>
    public IReadOnlyList<object?>? Customers { get; set; }

    /// <summary>Group ids, bare or as a <see cref="PageReference"/>.</summary>
    public IReadOnlyList<object?>? CustomerGroups { get; set; }

    public string? BodyClass { get; set; }

    public PagePasswordProtection? PasswordProtection { get; set; }

    public PageMenuPlacement? MenuPlacement { get; set; }

    public bool? Active { get; set; }

    public bool? DisableTracking { get; set; }
}

/// <summary>The URL write's bare body, <c>{"url": ...}</c>; a <c>page</c> wrapper is <c>400</c>.</summary>
public sealed class PageUrlInput : Dto
{
    public string? Url { get; set; }
}

/// <summary>The five-key <c>pagination</c> block; <see cref="Limit"/> is the page size applied, the total
/// under <c>limit=all</c>.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the listing: the rows, outside the trash, and where they sit.</summary>
public sealed class PageListing : Dto
{
    public IReadOnlyList<Page?>? Pages { get; set; }

    public Pagination? Pagination { get; set; }
}

/// <summary>The count sub-path's body, a bare <c>{"total": N}</c>.</summary>
public sealed class PageCount : Dto
{
    public long? Total { get; set; }
}

/// <summary>One reason the page cannot be deleted, such as <c>landing_page</c>.</summary>
public sealed class PageDeleteBlocker : Dto
{
    public string? Code { get; set; }

    public string? Message { get; set; }
}

/// <summary>The delete check's body: whether a delete would pass, what blocks it and what links to the page.
/// <see cref="Linked"/> was empty on every page read, so its rows stay untyped.</summary>
public sealed class PageDeleteBlockers : Dto
{
    public bool? CanDelete { get; set; }

    public IReadOnlyList<PageDeleteBlocker?>? Blockers { get; set; }

    public IReadOnlyList<object?>? Linked { get; set; }
}
