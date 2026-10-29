namespace WebCommander.Resources.Settings.PageSettings.SearchResultPageDto;

/// <summary>The <c>product_interaction_buttons</c> block - which buttons each result carries.</summary>
public sealed class ProductInteractionButtons : Dto
{
    public bool? Enabled { get; set; }

    public bool? DisplayAddToCartButton { get; set; }

    public bool? DisplayAddToCompareButton { get; set; }

    public bool? DisplayAddToWishlistButton { get; set; }

    public bool? DisplayViewDetailsButton { get; set; }

    public bool? DisplayOnHover { get; set; }

    public bool? DisplayQuickViewButton { get; set; }
}

/// <summary>The <c>number_of_searches_per_result</c> block - whether results are paginated, and how many sit on a page.</summary>
public sealed class NumberOfSearchesPerResult : Dto
{
    public bool? Enabled { get; set; }

    public long? ItemsPerPage { get; set; }
}

/// <summary>The <c>set_product_display_preferences</c> block - how results load and where the pagination controls sit.</summary>
public sealed class SetProductDisplayPreferences : Dto
{
    public bool? Enabled { get; set; }

    public string? DisplayMode { get; set; }

    public string? SelectPaginationPosition { get; set; }

    public long? NumberOfProductsPerPage { get; set; }

    public bool? AllowUsersToSelectNumberOfProducts { get; set; }

    public long? NumberOfProductsPerLoad { get; set; }
}

/// <summary>The <c>set_product_sorting</c> block - whether a default sort is applied, and which.</summary>
public sealed class SetProductSorting : Dto
{
    public bool? Enabled { get; set; }

    public string? DefaultSort { get; set; }
}

/// <summary>The <c>configure_sorting_options</c> block - which sort orders a shopper may switch to.</summary>
public sealed class ConfigureSortingOptions : Dto
{
    public bool? Enabled { get; set; }

    public bool? Alphabetical { get; set; }

    public bool? Featured { get; set; }

    public bool? New { get; set; }

    public bool? TopSelling { get; set; }

    public bool? Price { get; set; }

    public bool? CreatedDate { get; set; }
}

/// <summary>The <c>display_price</c> block - whether a result shows a price, and which price.</summary>
public sealed class DisplayPrice : Dto
{
    public bool? Enabled { get; set; }

    public bool? DisplayExpectToPayPrice { get; set; }

    public bool? ShowTaxWithTheExpectToPayPrice { get; set; }
}

/// <summary>The <c>product_information</c> section - what each search result shows and how the list behaves.</summary>
public sealed class ProductInformation : Dto
{
    protected override string? EnvelopeKey => "product_information";

    public bool? UseDefaultProductListingConfigurations { get; set; }

    public ProductInteractionButtons? ProductInteractionButtons { get; set; }

    public bool? DisplayContentPage { get; set; }

    public NumberOfSearchesPerResult? NumberOfSearchesPerResult { get; set; }

    public bool? DisplayShortDescription { get; set; }

    public bool? DisplayVariationsCombination { get; set; }

    public SetProductDisplayPreferences? SetProductDisplayPreferences { get; set; }

    public SetProductSorting? SetProductSorting { get; set; }

    public ConfigureSortingOptions? ConfigureSortingOptions { get; set; }

    public bool? DisplayLoyaltyPoints { get; set; }

    public DisplayPrice? DisplayPrice { get; set; }

    public string? LabelForPrice { get; set; }
}

/// <summary>The read-only <c>seo</c> section - two markers; this page's SEO is configured elsewhere.</summary>
public sealed class Seo : Dto
{
    protected override string? EnvelopeKey => "seo";

    public bool? ReadOnly { get; set; }

    public string? PageKind { get; set; }
}

/// <summary>The read-only <c>reinitialise</c> section - one field naming the verb its <c>href</c> takes.</summary>
public sealed class Reinitialise : Dto
{
    protected override string? EnvelopeKey => "reinitialise";

    public string? Action { get; set; }
}

/// <summary>The <c>product_promotion</c> block the facade inlines - a marker only; the rows have their own path.</summary>
public sealed class ProductPromotionPreview : Dto
{
    public bool? ReadOnly { get; set; }
}

/// <summary>One promotion row: its <see cref="Id"/> and stamps, three fields a create must carry and six it may.</summary>
public sealed class ProductPromotion : Dto
{
    protected override string? EnvelopeKey => "product_promotion";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Rule { get; set; }

    public string? Position { get; set; }

    public string? SpecificKeywords { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsAlwaysAvailable { get; set; }

    public string? AvailableFromDate { get; set; }

    public string? AvailableToDate { get; set; }

    public IReadOnlyList<long?>? ProductIds { get; set; }

    public string? Created { get; set; }

    public string? Updated { get; set; }
}

/// <summary>One entry of <c>group_meta</c>: what a thing is, whether it is writable, its label and where it lives.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? UiLabel { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block, one <see cref="SectionMeta"/> per declared entry; only the facade sends it.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? ProductInformation { get; set; }

    public SectionMeta? Seo { get; set; }

    public SectionMeta? ProductPromotion { get; set; }

    public SectionMeta? Reinitialise { get; set; }
}

/// <summary>The search result page settings; a section arrives under <c>search_result_page</c> too, one level in.</summary>
public sealed class SearchResultPage : Dto
{
    protected override string? EnvelopeKey => "search_result_page";

    public ProductInformation? ProductInformation { get; set; }

    public Seo? Seo { get; set; }

    public ProductPromotionPreview? ProductPromotion { get; set; }

    public Reinitialise? Reinitialise { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
