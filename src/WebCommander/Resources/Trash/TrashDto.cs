namespace WebCommander.Resources.Trash.TrashDto;

/// <summary>One trashed row, and the <c>item</c> a restore answers. <see cref="Type"/> is the snake_case name
/// <c>types</c> answers; <see cref="RemovedAt"/> is when the row was trashed, in UTC; <see cref="Url"/> is the
/// record's own path, <c>null</c> for a <c>blog_post</c>. A restore and a bulk delete send no <c>removed_at</c>.</summary>
public sealed class TrashItem : Dto
{
    protected override string? EnvelopeKey => "item";

    public string? Type { get; set; }

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? RemovedAt { get; set; }

    public string? Url { get; set; }
}

/// <summary>The five-key <c>pagination</c> block; there is no next-page link, so step <c>offset</c>.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    /// <summary>The page size applied.</summary>
    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the listing: the rows and where they sit.</summary>
public sealed class TrashPage : Dto
{
    public IReadOnlyList<TrashItem?>? Items { get; set; }

    public Pagination? Pagination { get; set; }
}

/// <summary>One type the trash holds, as a path's <c>{trash_type}</c> takes it.</summary>
public sealed class TrashType : Dto
{
    public string? Type { get; set; }
}

/// <summary>The <c>types</c> body: every type, alphabetical.</summary>
public sealed class TrashTypeList : Dto
{
    public IReadOnlyList<TrashType?>? Types { get; set; }
}

/// <summary>One item a bulk call could not act on: an error <c>code</c> such as <c>not_found</c> or
/// <c>not_in_trash</c>, and why in words.</summary>
public sealed class TrashBulkError : Dto
{
    public string? Type { get; set; }

    public long? Id { get; set; }

    public string? Code { get; set; }

    public string? Message { get; set; }
}

/// <summary>The bulk restore's body: the rows restored, and an entry per failed item.</summary>
public sealed class TrashBulkRestoreResult : Dto
{
    public IReadOnlyList<TrashItem?>? Restored { get; set; }

    public IReadOnlyList<TrashBulkError?>? Failed { get; set; }
}

/// <summary>The bulk delete's body: the rows deleted, each <c>type</c>, <c>id</c> and <c>name</c>, and an entry
/// per failed item.</summary>
public sealed class TrashBulkDeleteResult : Dto
{
    public IReadOnlyList<TrashItem?>? Deleted { get; set; }

    public IReadOnlyList<TrashBulkError?>? Failed { get; set; }
}

/// <summary>One item of a bulk body; <see cref="Type"/> in any letter case, <see cref="Id"/> a number.</summary>
public sealed class TrashBulkItemInput : Dto
{
    public string? Type { get; set; }

    public long? Id { get; set; }
}

/// <summary>The bulk body, <c>{"items": [...]}</c>, 1 to 100 items; no envelope.</summary>
public sealed class TrashBulkInput : Dto
{
    public IReadOnlyList<TrashBulkItemInput?>? Items { get; set; }
}
