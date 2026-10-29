namespace WebCommander.Resources.BlogCategories.BlogCategoriesDto;

/// <summary>A category's <c>image</c>, <c>{}</c> until one is uploaded.</summary>
public sealed class BlogCategoryImage : Dto
{
    public string? Title { get; set; }

    public string? AlternativeText { get; set; }

    public string? FileName { get; set; }

    public string? Link { get; set; }
}

/// <summary>One <c>seo_configs</c> entry; no write here sets the list, so a created category's is empty.</summary>
public sealed class BlogCategorySeoConfig : Dto
{
    public long? Id { get; set; }

    public string? Value { get; set; }

    public string? Type { get; set; }

    public string? ConfigKey { get; set; }
}

/// <summary>One blog category: a listing row, a row path's body and every write's answer, the same eleven fields each time.</summary>
public sealed class BlogCategory : Dto
{
    protected override string? EnvelopeKey => "blog_category";

    public long? Id { get; set; }

    /// <summary>A UUID string the store assigns; a create or update naming another one keeps the stored one.</summary>
    public string? Uuid { get; set; }

    /// <summary>Required on both writes, 2 to 255 characters and unique.</summary>
    public string? Name { get; set; }

    public string? Url { get; set; }

    public string? Description { get; set; }

    public bool? IsDisposable { get; set; }

    public bool? IsInTrash { get; set; }

    public BlogCategoryImage? Image { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedOn { get; set; }

    public IReadOnlyList<BlogCategorySeoConfig?>? SeoConfigs { get; set; }
}

/// <summary>A blog the category is linked to, by <see cref="Id"/>; the category row never lists them.</summary>
public sealed class BlogCategoryBlog : Dto
{
    public long? Id { get; set; }
}

/// <summary>The writable form of <see cref="BlogCategory"/>, inside the wrapper both writes require. An update blanks an
/// unset <c>description</c> and keeps the <c>url</c>, the <c>image</c> and the blog links.</summary>
public sealed class BlogCategoryInput : Dto
{
    protected override string? EnvelopeKey => "blog_category";

    public string? Name { get; set; }

    public string? Url { get; set; }

    public string? Description { get; set; }

    /// <summary>A map, <c>{file_name, base64, title, alternative_text}</c>; <c>title</c> and <c>alternative_text</c>
    /// are stored only beside a <c>base64</c>.</summary>
    public IReadOnlyDictionary<string, object?>? Image { get; set; }

    public IReadOnlyList<BlogCategoryBlog?>? Blogs { get; set; }
}

/// <summary>The listing's ten-key <c>pagination</c> block; <see cref="Limit"/> is the size the store applied.</summary>
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

/// <summary>One page of the listing; <c>field_metadata</c> is accepted and adds no key.</summary>
public sealed class BlogCategoryPage : Dto
{
    public IReadOnlyList<BlogCategory?>? BlogCategories { get; set; }

    public Pagination? Pagination { get; set; }
}
