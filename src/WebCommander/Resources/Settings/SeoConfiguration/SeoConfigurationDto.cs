namespace WebCommander.Resources.Settings.SeoConfiguration.SeoConfigurationDto;

/// <summary>One content type's block of <c>global_store_settings</c>: pages, products or categories.</summary>
public sealed class SeoTemplates : Dto
{
    public bool? DisableIndexing { get; set; }

    public IReadOnlyList<string?>? TitleTemplate { get; set; }

    public IReadOnlyList<string?>? DescriptionTemplate { get; set; }
}

/// <summary>The <c>global_store_settings</c> section - store name, title separator and per-type templates.</summary>
public sealed class GlobalStoreSettings : Dto
{
    protected override string? EnvelopeKey => "global_store_settings";

    public string? StoreName { get; set; }

    public string? TitleSeparator { get; set; }

    public SeoTemplates? Pages { get; set; }

    public SeoTemplates? Products { get; set; }

    public SeoTemplates? Categories { get; set; }
}

/// <summary>The <c>header</c> or <c>footer</c> block of <c>custom_code</c>, served on every storefront page.</summary>
public sealed class CustomCodeBlock : Dto
{
    public bool? Enabled { get; set; }

    public string? Code { get; set; }
}

/// <summary>The <c>custom_code</c> section - the two blocks whose <c>code</c> reaches every storefront page.</summary>
public sealed class CustomCode : Dto
{
    protected override string? EnvelopeKey => "custom_code";

    public CustomCodeBlock? Header { get; set; }

    public CustomCodeBlock? Footer { get; set; }
}

/// <summary>One redirect row: the store's <see cref="Id"/> and the pair of URLs.</summary>
public sealed class Redirect : Dto
{
    protected override string? EnvelopeKey => "redirect";

    public long? Id { get; set; }

    public string? SourceUrl { get; set; }

    public string? DestinationUrl { get; set; }
}

/// <summary>The <c>redirects</c> block inside <c>url_settings</c> - the rows inlined, with no <c>pagination</c>.</summary>
public sealed class RedirectsPreview : Dto
{
    public IReadOnlyList<Redirect?>? Items { get; set; }
}

/// <summary>The <c>url_settings</c> section - URL rewriting on rename, the redirect it leaves, and the inlined rows.</summary>
public sealed class UrlSettings : Dto
{
    protected override string? EnvelopeKey => "url_settings";

    public bool? RegenerateUrlOnItemUpdate { get; set; }

    public bool? AutoRedirectOnUrlChange { get; set; }

    public RedirectsPreview? Redirects { get; set; }
}

/// <summary>One file served from the document root; <see cref="ConfigKey"/> is the store's id, not the file name.</summary>
public sealed class DocumentRootFile : Dto
{
    protected override string? EnvelopeKey => "document_root_file";

    public string? ConfigKey { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }
}

/// <summary>The <c>document_root</c> block the facade inlines - the rows with no <c>pagination</c>.</summary>
public sealed class DocumentRootPreview : Dto
{
    public IReadOnlyList<DocumentRootFile?>? Items { get; set; }
}

/// <summary>One entry of <c>group_meta</c>: what a thing is, whether it can be written, and where it lives.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }

    public SectionMeta? Redirects { get; set; }
}

/// <summary>The <c>group_meta</c> block, one <see cref="SectionMeta"/> per declared entry; only the facade sends it.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? GlobalStoreSettings { get; set; }

    public SectionMeta? CustomCode { get; set; }

    public SectionMeta? DocumentRoot { get; set; }

    public SectionMeta? UrlSettings { get; set; }
}

/// <summary>The SEO configuration, as the facade or a single section sends it.</summary>
public sealed class SeoConfiguration : Dto
{
    protected override string? EnvelopeKey => "seo_configuration";

    public GlobalStoreSettings? GlobalStoreSettings { get; set; }

    public CustomCode? CustomCode { get; set; }

    public DocumentRootPreview? DocumentRoot { get; set; }

    public UrlSettings? UrlSettings { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
