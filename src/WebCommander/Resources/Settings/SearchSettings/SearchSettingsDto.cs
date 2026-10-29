namespace WebCommander.Resources.Settings.SearchSettings.SearchSettingsDto;

/// <summary>One entry of an <c>excluded</c> list as a read sends it; a write may send bare ids instead.</summary>
public sealed class ExcludedRecord : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>One content type the site search covers - whether it is searched, its heading, and the rows held out of it.</summary>
public class SearchableContentGroup : Dto
{
    public bool? Enabled { get; set; }

    public string? DisplayHeading { get; set; }

    public IReadOnlyList<ExcludedRecord?>? Excluded { get; set; }
}

/// <summary>The <c>product_categories</c> block - the one content group with a fourth field.</summary>
public sealed class ProductCategoriesContent : SearchableContentGroup
{
    public bool? HideParentCategory { get; set; }
}

/// <summary>The <c>searchable_content</c> section - what the search box says and which content types it reaches.</summary>
public sealed class SearchableContent : Dto
{
    protected override string? EnvelopeKey => "searchable_content";

    public string? PlaceholderText { get; set; }

    public SearchableContentGroup? Product { get; set; }

    public ProductCategoriesContent? ProductCategories { get; set; }

    public SearchableContentGroup? Blog { get; set; }

    public SearchableContentGroup? OtherPages { get; set; }
}

/// <summary>One row of <c>search_fields.fields</c> - a product field and where it ranks.</summary>
public sealed class SearchFieldPriority : Dto
{
    public string? Field { get; set; }

    public long? Priority { get; set; }
}

/// <summary>The <c>search_fields</c> section - how a query is matched and which fields it is matched against.
/// <see cref="Fields"/> is the store's key and so carries <c>new</c>: it hides <see cref="Dto.Fields(Type)"/>.</summary>
public sealed class SearchFields : Dto
{
    protected override string? EnvelopeKey => "search_fields";

    public bool? MatchKeywordsExactly { get; set; }

    public bool? MatchFullSentences { get; set; }

    public new IReadOnlyList<SearchFieldPriority?>? Fields { get; set; }
}

/// <summary>One group of <c>synonyms.groups</c> - words a query treats as each other.</summary>
public sealed class SynonymGroup : Dto
{
    public IReadOnlyList<string?>? Keywords { get; set; }
}

/// <summary>The <c>synonyms</c> section - read it, do not write it.</summary>
public sealed class Synonyms : Dto
{
    protected override string? EnvelopeKey => "synonyms";

    public IReadOnlyList<SynonymGroup?>? Groups { get; set; }
}

/// <summary>One entry of <c>group_meta</c>: what a thing is, whether it can be written and where it is managed.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block, one <see cref="SectionMeta"/> per section; only the facade sends it.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? SearchableContent { get; set; }

    public SectionMeta? SearchFields { get; set; }

    public SectionMeta? Synonyms { get; set; }
}

/// <summary>The bare <c>reindex_status</c> record; three fields are <c>object</c>, as their type is unknown.</summary>
public sealed class ReindexStatus : Dto
{
    public string? State { get; set; }

    public object? StartedAt { get; set; }

    public object? FinishedAt { get; set; }

    public object? Error { get; set; }
}

/// <summary>The search settings, as the facade or a single section sends them.</summary>
public sealed class SearchSettings : Dto
{
    protected override string? EnvelopeKey => "search_settings";

    public SearchableContent? SearchableContent { get; set; }

    public SearchFields? SearchFields { get; set; }

    public Synonyms? Synonyms { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
