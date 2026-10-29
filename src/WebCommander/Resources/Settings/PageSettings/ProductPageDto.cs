namespace WebCommander.Resources.Settings.PageSettings.ProductPageDto;

/// <summary>The <c>actions_buttons</c> block - which buttons a product page carries.</summary>
public sealed class ActionButtons : Dto
{
    public bool? ShowProductActionButtons { get; set; }

    public bool? DisplayAddToCartButton { get; set; }

    public bool? DisplayBuyNowButton { get; set; }

    public bool? DisplayOnHover { get; set; }

    public bool? DisplayAddToCompareButton { get; set; }

    public bool? DisplayAddToWishlistButton { get; set; }
}

/// <summary>The <c>details</c> block - which parts of the product's own description are shown.</summary>
public sealed class ProductDetails : Dto
{
    public bool? DisplayDescription { get; set; }

    public bool? DisplaySalePercentage { get; set; }

    public bool? UseStrikeThroughOnPreviousPrice { get; set; }

    public bool? DisplayTaxWithExpectToPayPrice { get; set; }
}

/// <summary>The <c>social_sharing</c> block - whether share buttons appear, which networks they cover and their title.</summary>
public sealed class SocialSharing : Dto
{
    public bool? EnableShareOnSocialMedia { get; set; }

    public bool? Facebook { get; set; }

    public bool? Twitter { get; set; }

    public bool? Pinterest { get; set; }

    public bool? Messenger { get; set; }

    public bool? Linkedin { get; set; }

    public bool? Email { get; set; }

    public bool? Snapchat { get; set; }

    public bool? Telegram { get; set; }

    public bool? Reddit { get; set; }

    public bool? Instagram { get; set; }

    public bool? Youtube { get; set; }

    public bool? Tiktok { get; set; }

    public bool? Threads { get; set; }

    public bool? Whatsapp { get; set; }

    public bool? Bluesky { get; set; }

    public bool? Mastodon { get; set; }

    public bool? CopyLink { get; set; }

    public bool? DisplayTitle { get; set; }

    public string? Title { get; set; }
}

/// <summary>The <c>set_product_properties</c> block - which physical properties a product page lists.</summary>
public sealed class SetProductProperties : Dto
{
    public bool? DisplayHeight { get; set; }

    public bool? DisplayWidth { get; set; }

    public bool? DisplayLength { get; set; }

    public bool? DisplayWeight { get; set; }

    public bool? DisplayAddToGiftRegistry { get; set; }
}

/// <summary>The <c>configuration</c> section - what the product page itself shows, in four blocks.</summary>
public sealed class Configuration : Dto
{
    protected override string? EnvelopeKey => "configuration";

    public ActionButtons? ActionsButtons { get; set; }

    public ProductDetails? Details { get; set; }

    public SocialSharing? SocialSharing { get; set; }

    public SetProductProperties? SetProductProperties { get; set; }
}

/// <summary>The <c>labels_and_display</c> block - how a variation is labelled and drawn.</summary>
public sealed class LabelsAndDisplay : Dto
{
    public bool? DisableVariationLabelForPrice { get; set; }

    public bool? DisplayProductImageAsVariationIcons { get; set; }

    public bool? DisplaySelectedVariationValue { get; set; }

    public bool? EnableColourTypeVariationImageDisplay { get; set; }

    public bool? UseStrikeThroughForOutOfStockVariations { get; set; }

    public bool? EnableSingleClickVariationLoading { get; set; }
}

/// <summary>One entry of <c>layout.variation_combinations</c> - a variation type the store already defines.</summary>
public sealed class VariationType : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>The <c>layout</c> block - how variations are laid out and which price is shown.</summary>
public sealed class VariationLayout : Dto
{
    public bool? SelectVariationView { get; set; }

    public string? SelectVariationViewType { get; set; }

    public bool? DisplayVariationCombinations { get; set; }

    public IReadOnlyList<VariationType?>? VariationCombinations { get; set; }

    public bool? VariationPriceDisplayMode { get; set; }

    public string? VariationPriceDisplayType { get; set; }
}

/// <summary>The <c>variations</c> section - how a product's variations are labelled and laid out.</summary>
public sealed class Variations : Dto
{
    protected override string? EnvelopeKey => "variations";

    public LabelsAndDisplay? LabelsAndDisplay { get; set; }

    public VariationLayout? Layout { get; set; }
}

/// <summary>The <c>image_display</c> block - how product images are laid out and zoomed.</summary>
public sealed class ImageDisplay : Dto
{
    public bool? EnableImageSlider { get; set; }

    public bool? ProductThumbnailVerticalView { get; set; }

    public bool? ProductImageView { get; set; }

    public string? ProductImageViewType { get; set; }

    public bool? ImageZoomType { get; set; }

    public string? ZoomType { get; set; }
}

/// <summary>The <c>video</c> block - whether a product video is previewed and where its thumbnail sits.</summary>
public sealed class ProductVideo : Dto
{
    public bool? EnableVideoPreview { get; set; }

    public bool? DisplayVideoThumbnailInFirstOrder { get; set; }
}

/// <summary>The <c>image_and_video</c> section - one field of its own beside two blocks.</summary>
public sealed class ImageAndVideo : Dto
{
    protected override string? EnvelopeKey => "image_and_video";

    public bool? AllowSingleFileUploadPerProduct { get; set; }

    public ImageDisplay? ImageDisplay { get; set; }

    public ProductVideo? Video { get; set; }
}

/// <summary>The fields a product listing on this page carries, shared by the two sections that hold one.</summary>
public class ProductListing : Dto
{
    public long? NumberOfProducts { get; set; }

    public bool? AllowViewersToChangeDisplayMode { get; set; }

    public bool? GridView { get; set; }

    public bool? ListView { get; set; }

    public bool? TabularView { get; set; }

    public string? SelectDisplayType { get; set; }

    public long? MaximumNumberOfColumnsPerRow { get; set; }

    public long? MaximumNumberOfColumnsPerRowList { get; set; }

    public bool? ShortDescription { get; set; }

    public bool? DisplayOnHover { get; set; }

    public bool? DisplayAddToCartButton { get; set; }

    public bool? DisplayAddToCompareButton { get; set; }

    public bool? DisplayAddToWishlistButton { get; set; }

    public bool? Price { get; set; }

    public bool? ShowExpectToPayPrice { get; set; }

    public bool? DisplayRating { get; set; }

    public bool? DisplayReviewCount { get; set; }
}

/// <summary>The <c>up_sell</c> section - the up-sell listing, a flat <see cref="ProductListing"/>.</summary>
public sealed class UpSell : ProductListing
{
    protected override string? EnvelopeKey => "up_sell";
}

/// <summary>The <c>related_products</c> section - the related-products listing, a flat <see cref="ProductListing"/>.</summary>
public sealed class RelatedProducts : ProductListing
{
    protected override string? EnvelopeKey => "related_products";
}

/// <summary>The read-only <c>capabilities</c> block - what the review plugin makes available.</summary>
public sealed class ReviewCapabilities : Dto
{
    public bool? Available { get; set; }
}

/// <summary>The <c>review_and_ratings</c> section - where reviews appear and which version renders them.</summary>
public sealed class ReviewAndRatings : Dto
{
    protected override string? EnvelopeKey => "review_and_ratings";

    public ReviewCapabilities? Capabilities { get; set; }

    public bool? EnableReviewInTab { get; set; }

    public bool? EnableNewVersion { get; set; }
}

/// <summary>The <c>quick_view</c> section - the quick-view popup and what it shows.</summary>
public sealed class QuickView : Dto
{
    protected override string? EnvelopeKey => "quick_view";

    public bool? ProductQuickView { get; set; }

    public bool? DisplayProductName { get; set; }

    public bool? DisplaySku { get; set; }

    public bool? DisplayProductRating { get; set; }

    public bool? DisplayProductStock { get; set; }

    public bool? DisplayProductVariation { get; set; }

    public bool? DisplayProductImage { get; set; }

    public bool? DisplayVariationImage { get; set; }

    public bool? DisplayProductSummary { get; set; }

    public bool? DisplayAddToCartButton { get; set; }

    public bool? DisplayAddToCompareButton { get; set; }

    public bool? DisplayAddToWishlistButton { get; set; }
}

/// <summary>One entry of <c>group_meta</c>: what a thing is, whether it is writable, its label and where it lives.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Plugin { get; set; }

    public bool? Available { get; set; }

    public string? UiLabel { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block, one <see cref="SectionMeta"/> per section; only the facade sends it.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? Configuration { get; set; }

    public SectionMeta? Variations { get; set; }

    public SectionMeta? ImageAndVideo { get; set; }

    public SectionMeta? UpSell { get; set; }

    public SectionMeta? RelatedProducts { get; set; }

    public SectionMeta? ReviewAndRatings { get; set; }

    public SectionMeta? QuickView { get; set; }
}

/// <summary>The product page settings; a single section arrives under <c>product_page</c> too, one level in.</summary>
public sealed class ProductPage : Dto
{
    protected override string? EnvelopeKey => "product_page";

    public Configuration? Configuration { get; set; }

    public Variations? Variations { get; set; }

    public ImageAndVideo? ImageAndVideo { get; set; }

    public UpSell? UpSell { get; set; }

    public RelatedProducts? RelatedProducts { get; set; }

    public ReviewAndRatings? ReviewAndRatings { get; set; }

    public QuickView? QuickView { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
