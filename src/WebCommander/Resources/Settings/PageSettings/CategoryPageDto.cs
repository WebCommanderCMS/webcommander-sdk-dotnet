namespace WebCommander.Resources.Settings.PageSettings.CategoryPageDto;

/// <summary>The <c>heading</c> block - whether the category name is shown, and at which HTML heading level.</summary>
public sealed class CategoryHeading : Dto
{
    public bool? Enabled { get; set; }

    public string? Style { get; set; }
}

/// <summary>The <c>subcategory</c> block - whether subcategories are listed, and how that list pages.</summary>
public sealed class Subcategory : Dto
{
    public bool? Enabled { get; set; }

    public bool? Display { get; set; }

    public string? LoadType { get; set; }

    public string? PaginationPosition { get; set; }

    public long? PerPage { get; set; }

    public bool? AllowUserPerPage { get; set; }

    public long? PerLoad { get; set; }

    public bool? DisplayProducts { get; set; }

    public bool? ShowSummary { get; set; }
}

/// <summary>The <c>category_information</c> section - what the category shows and how its subcategories are listed.</summary>
public sealed class CategoryInformation : Dto
{
    protected override string? EnvelopeKey => "category_information";

    public CategoryHeading? Heading { get; set; }

    public bool? ShowDescription { get; set; }

    public bool? ShowBannerImage { get; set; }

    public bool? ShowImage { get; set; }

    public Subcategory? Subcategory { get; set; }
}

/// <summary>The <c>interaction_buttons</c> block - which buttons each product card carries.</summary>
public sealed class InteractionButtons : Dto
{
    public bool? Enabled { get; set; }

    public bool? AddToCart { get; set; }

    public bool? ViewDetails { get; set; }

    public bool? ShowOnHover { get; set; }

    public bool? AddToWishList { get; set; }

    public bool? AddToCompare { get; set; }

    public bool? QuickView { get; set; }
}

/// <summary>The <c>price</c> block - whether a card shows a price, and which price.</summary>
public sealed class Price : Dto
{
    public bool? Enabled { get; set; }

    public bool? ExpectToPay { get; set; }

    public bool? ExpectToPayWithTax { get; set; }

    public bool? StrikeThroughPrevious { get; set; }
}

/// <summary>One entry of <c>variation_combination.types</c> - a variation type the store already defines.</summary>
public sealed class VariationType : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>The <c>variation_combination</c> block - whether combinations are shown, and which variation types.</summary>
public sealed class VariationCombination : Dto
{
    public bool? Enabled { get; set; }

    public IReadOnlyList<VariationType?>? Types { get; set; }
}

/// <summary>The <c>rating</c> block - whether the rating is shown, and whether the review count sits beside it.</summary>
public sealed class Rating : Dto
{
    public bool? Enabled { get; set; }

    public bool? ShowReviewCount { get; set; }
}

/// <summary>The <c>view_switcher</c> block - whether a shopper may change the display mode, and to which modes.</summary>
public sealed class ViewSwitcher : Dto
{
    public bool? Enabled { get; set; }

    public bool? Grid { get; set; }

    public bool? List { get; set; }

    public bool? Tabular { get; set; }
}

/// <summary>The <c>columns_per_row</c> block of <c>default_display</c> - product columns per row, per mode.</summary>
public sealed class ColumnsPerRow : Dto
{
    public long? Grid { get; set; }

    public long? List { get; set; }
}

/// <summary>The <c>default_display</c> block - the mode a listing opens in and how wide its rows are.</summary>
public sealed class DefaultDisplay : Dto
{
    public bool? Enabled { get; set; }

    public string? Mode { get; set; }

    public ColumnsPerRow? ColumnsPerRow { get; set; }

    public bool? ShowVariationOptionsLabelInTabular { get; set; }
}

/// <summary>The <c>load</c> block - how the product listing pages and where its control sits.</summary>
public sealed class ProductLoad : Dto
{
    public bool? Enabled { get; set; }

    public string? Type { get; set; }

    public string? PaginationPosition { get; set; }

    public long? PerPage { get; set; }

    public bool? AllowUserPerPage { get; set; }

    public long? PerLoad { get; set; }
}

/// <summary>The <c>default_sort</c> block - whether a default order is applied, and which.</summary>
public sealed class DefaultSort : Dto
{
    public bool? Enabled { get; set; }

    public string? By { get; set; }
}

/// <summary>The <c>sort_options</c> block - which orders a shopper may switch to.</summary>
public sealed class SortOptions : Dto
{
    public bool? Enabled { get; set; }

    public bool? Alphabetic { get; set; }

    public bool? Featured { get; set; }

    public bool? New { get; set; }

    public bool? TopSelling { get; set; }

    public bool? Price { get; set; }

    public bool? Date { get; set; }
}

/// <summary>The <c>product_information</c> section - what each product card shows and how the listing behaves.</summary>
public sealed class ProductInformation : Dto
{
    protected override string? EnvelopeKey => "product_information";

    public bool? UseGlobalDefaults { get; set; }

    public InteractionButtons? InteractionButtons { get; set; }

    public bool? ShowSummary { get; set; }

    public bool? ShowSalePercentage { get; set; }

    public bool? ShowStockAvailability { get; set; }

    public bool? ShowParentCategory { get; set; }

    public Price? Price { get; set; }

    public VariationCombination? VariationCombination { get; set; }

    public Rating? Rating { get; set; }

    public ViewSwitcher? ViewSwitcher { get; set; }

    public DefaultDisplay? DefaultDisplay { get; set; }

    public ProductLoad? Load { get; set; }

    public DefaultSort? DefaultSort { get; set; }

    public SortOptions? SortOptions { get; set; }
}

/// <summary>One entry of <c>group_meta</c>: what a thing is, whether it can be written, and where it is managed.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block, one <see cref="SectionMeta"/> per section; only the facade sends it.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? CategoryInformation { get; set; }

    public SectionMeta? ProductInformation { get; set; }
}

/// <summary>The category page settings, as the facade or a single section sends them.</summary>
public sealed class CategoryPage : Dto
{
    protected override string? EnvelopeKey => "category_page";

    public CategoryInformation? CategoryInformation { get; set; }

    public ProductInformation? ProductInformation { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
