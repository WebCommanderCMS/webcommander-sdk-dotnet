namespace WebCommander.Resources.Blogs.BlogsDto;

/// <summary>A category a blog is filed under; a write names it by <see cref="Id"/>.</summary>
public sealed class BlogCategoryReference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public bool? IsDisposable { get; set; }

    public bool? IsInTrash { get; set; }
}

/// <summary>The cover image, <c>{}</c> when there is none.</summary>
public sealed class BlogImage : Dto
{
    public string? Title { get; set; }

    public string? AlternativeText { get; set; }

    public string? FileName { get; set; }

    public string? Link { get; set; }
}

/// <summary><c>created_by</c>; <c>{}</c> on a row the store recorded no author for.</summary>
public sealed class BlogAuthor : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>One customer, by <c>internal_id</c>, or one customer group a restricted blog is shown to.</summary>
public sealed class BlogAudienceMember : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary><c>selected_customers</c> once it names anyone; the store sends <c>[]</c> when it names no one, which reads <c>null</c> here.</summary>
public sealed class BlogAudience : Dto
{
    public IReadOnlyList<BlogAudienceMember?>? Customers { get; set; }

    public IReadOnlyList<BlogAudienceMember?>? Groups { get; set; }
}

/// <summary>The audience a write names: customer <c>internal_id</c> values and customer group ids; a <c>customer_id</c> is dropped.</summary>
public sealed class BlogAudienceInput : Dto
{
    public IReadOnlyList<long?>? Customers { get; set; }

    public IReadOnlyList<long?>? Groups { get; set; }
}

/// <summary>One <c>meta_tag</c> entry, the same two fields on a write and a read.</summary>
public sealed class BlogMetaTag : Dto
{
    public string? Name { get; set; }

    public string? Value { get; set; }
}

/// <summary><c>seo_configs</c>, an object holding the <c>meta_tag</c> list.</summary>
public sealed class BlogSeoConfigs : Dto
{
    public IReadOnlyList<BlogMetaTag?>? MetaTag { get; set; }
}

/// <summary>The one <c>group_meta</c> entry: what the collection is, whether it is writable and where.</summary>
public sealed class ResourceMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block the listing, a row read and a <c>PATCH</c> answer carry.</summary>
public sealed class GroupMeta : Dto
{
    public ResourceMeta? Blogs { get; set; }
}

/// <summary>One blog, the same eighteen fields on a listing row, a row read and every write's answer.</summary>
public sealed class Blog : Dto
{
    protected override string? EnvelopeKey => "blog";

    public long? Id { get; set; }

    /// <summary>A UUID string the store assigns; a create naming it is <c>400 unknown_field</c>.</summary>
    public string? Uuid { get; set; }

    /// <summary>Required on the create and the replace, and unique among live and trashed blogs alike.</summary>
    public string? Title { get; set; }

    /// <summary>The title's slug on the create; a later write naming <c>title</c> takes a sent one.</summary>
    public string? Url { get; set; }

    public string? Content { get; set; }

    public string? Date { get; set; }

    public bool? IsPublished { get; set; }

    public string? Visibility { get; set; }

    public string? VisibleTo { get; set; }

    public IReadOnlyList<BlogCategoryReference?>? Categories { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedOn { get; set; }

    public bool? IsDisposable { get; set; }

    public bool? IsInTrash { get; set; }

    public BlogImage? Image { get; set; }

    public BlogAuthor? CreatedBy { get; set; }

    public BlogAudience? SelectedCustomers { get; set; }

    public BlogSeoConfigs? SeoConfigs { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary>The writable form of <see cref="Blog"/>, for the create, the replace and the patch; the replace requires <c>title</c> and <c>visibility</c>.</summary>
public sealed class BlogInput : Dto
{
    protected override string? EnvelopeKey => "blog";

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Date { get; set; }

    public bool? IsPublished { get; set; }

    public string? Visibility { get; set; }

    public string? VisibleTo { get; set; }

    public IReadOnlyList<BlogCategoryReference?>? Categories { get; set; }

    /// <summary>A map, <c>{file_name, base64, title, alternative_text}</c>.</summary>
    public IReadOnlyDictionary<string, object?>? Image { get; set; }

    public BlogSeoConfigs? SeoConfigs { get; set; }

    public BlogAudienceInput? SelectedCustomers { get; set; }
}

/// <summary>The ten-key <c>pagination</c> block; <see cref="Limit"/> is the size the store applied, capped at 20.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public long? Count { get; set; }

    public long? CurrentPage { get; set; }

    public long? TotalPages { get; set; }

    public bool? HasNext { get; set; }

    public bool? HasPrevious { get; set; }

    public string? PreviousPage { get; set; }

    public string? NextPage { get; set; }
}

/// <summary>One page of the listing: the rows, <c>group_meta</c>, the paging block and any schema.</summary>
public sealed class BlogPage : Dto
{
    public IReadOnlyList<Blog?>? Blogs { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public Pagination? Pagination { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary>The count sub-path's body, <c>{"blogs": {"count": N}}</c>.</summary>
public sealed class BlogCount : Dto
{
    protected override string? EnvelopeKey => "blogs";

    public long? Count { get; set; }
}

/// <summary>One comment, the same twelve fields in the listing and on its own path; no write here adds to <see cref="Replies"/>.</summary>
public sealed class BlogComment : Dto
{
    public long? Id { get; set; }

    public string? Status { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    /// <summary>The blog's current title, following a later rename.</summary>
    public string? PostTitle { get; set; }

    public string? Content { get; set; }

    public bool? Spam { get; set; }

    public long? Likes { get; set; }

    public long? TotalReply { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedOn { get; set; }

    public IReadOnlyList<object?>? Replies { get; set; }
}

/// <summary>The comments sub-path's body: the blog's <c>id</c> beside every comment it holds; no envelope, which would merge that id over a comment's.</summary>
public sealed class BlogCommentList : Dto
{
    public long? Id { get; set; }

    public IReadOnlyList<BlogComment?>? Comments { get; set; }
}

/// <summary>One comment's body, from its own path and from each comment write: the blog's <c>id</c> beside the <c>comment</c>.</summary>
public sealed class BlogCommentEnvelope : Dto
{
    public long? Id { get; set; }

    public BlogComment? Comment { get; set; }
}

/// <summary>The comment a create sends: <c>content</c> required, at most 1000 characters; <c>name</c> and <c>email</c> may be left out.</summary>
public sealed class BlogCommentDraft : Dto
{
    public string? Content { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }
}

/// <summary>The comment create's body, <c>{"blog": {"comment": {...}}}</c>.</summary>
public sealed class BlogCommentInput : Dto
{
    protected override string? EnvelopeKey => "blog";

    public BlogCommentDraft? Comment { get; set; }
}
