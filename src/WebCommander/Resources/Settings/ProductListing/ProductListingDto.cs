namespace WebCommander.Resources.Settings.ProductListing.ProductListingDto;

/// <summary>The <c>add to cart</c> and <c>view details</c> buttons on a listing tile, and whether they wait for hover.</summary>
public sealed class InteractionButtons : Dto
{
    public bool? Enabled { get; set; }

    public bool? AddToCart { get; set; }

    public bool? ShowViewDetails { get; set; }

    public bool? ShowOnHover { get; set; }
}

/// <summary>Which prices a listing tile shows; not named <c>Price</c>, which <c>sortable_items</c> uses as a boolean.</summary>
public sealed class ListingPrice : Dto
{
    public bool? Enabled { get; set; }

    public bool? ExpectToPay { get; set; }

    public bool? ExpectToPayWithTax { get; set; }
}

/// <summary>The rating stars on a listing tile and the count beside them.</summary>
public sealed class Reviews : Dto
{
    public bool? ShowRating { get; set; }

    public bool? ShowReviewCount { get; set; }
}

/// <summary>Which variation types a listing tile combines, from the store's own variation names.</summary>
public sealed class VariationCombination : Dto
{
    public bool? Enabled { get; set; }

    public IReadOnlyList<string?>? Types { get; set; }
}

/// <summary>The <c>product_information</c> section - what a listing tile shows.
/// No <c>EnvelopeKey</c> on any section here: a bare section body is refused, there being no section endpoint.</summary>
public sealed class ProductInformation : Dto
{
    public InteractionButtons? InteractionButtons { get; set; }

    public bool? ShowSummary { get; set; }

    public bool? ShowSalePercentage { get; set; }

    public bool? ShowStockAvailability { get; set; }

    public bool? ShowParentCategory { get; set; }

    public bool? ShowLoyaltyPoints { get; set; }

    public ListingPrice? Price { get; set; }

    public Reviews? Reviews { get; set; }

    public VariationCombination? VariationCombination { get; set; }

    public bool? ShowVariationsAsProducts { get; set; }
}

/// <summary>Where the pager sits and how many products a page holds.</summary>
public sealed class Pagination : Dto
{
    public string? Position { get; set; }

    public long? PerPage { get; set; }

    public bool? AllowUserPerPage { get; set; }
}

/// <summary>How many products each press of <i>load more</i> adds.</summary>
public sealed class LoadMore : Dto
{
    public long? PerLoad { get; set; }
}

/// <summary>The <c>load</c> section - whether a listing pages or loads more; <see cref="Type"/> picks the block.</summary>
public sealed class Load : Dto
{
    public bool? Enabled { get; set; }

    public string? Type { get; set; }

    public Pagination? Pagination { get; set; }

    public LoadMore? LoadMore { get; set; }
}

/// <summary>The <c>sorting</c> section - the order a listing arrives in.</summary>
public sealed class Sorting : Dto
{
    public bool? Enabled { get; set; }

    public string? SortBy { get; set; }
}

/// <summary>Which orders a shopper may pick from.</summary>
public sealed class SortableOptions : Dto
{
    public bool? Alphabetic { get; set; }

    public bool? Featured { get; set; }

    public bool? New { get; set; }

    public bool? TopSelling { get; set; }

    public bool? Price { get; set; }

    public bool? CreatedDate { get; set; }
}

/// <summary>The <c>sortable_items</c> section - whether a shopper may re-sort a listing, and by what.</summary>
public sealed class SortableItems : Dto
{
    public bool? Enabled { get; set; }

    public SortableOptions? Options { get; set; }
}

/// <summary>The one entry of <c>group_meta</c>: what the area is, whether it is writable and where.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block - a single entry for the whole area, not one per section.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? ProductListing { get; set; }
}

/// <summary>The product listing settings: four sections under <c>product_listing</c>, beside <c>group_meta</c>.</summary>
public sealed class ProductListing : Dto
{
    protected override string? EnvelopeKey => "product_listing";

    public ProductInformation? ProductInformation { get; set; }

    public Load? Load { get; set; }

    public Sorting? Sorting { get; set; }

    public SortableItems? SortableItems { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
