namespace WebCommander.Resources.Settings.PageSettings.ShoppingCartDto;

/// <summary>The <c>cart_page</c> section - what the cart page itself offers a shopper.</summary>
public sealed class CartPage : Dto
{
    protected override string? EnvelopeKey => "cart_page";

    public bool? ShowProductThumbnail { get; set; }

    public bool? EnableClearCart { get; set; }

    public bool? SaveCart { get; set; }
}

/// <summary>The <c>flying_cart</c> section - the slide-out cart and what it offers.</summary>
public sealed class FlyingCart : Dto
{
    protected override string? EnvelopeKey => "flying_cart";

    public bool? Enabled { get; set; }

    public bool? EnableClearCart { get; set; }

    public bool? SaveCart { get; set; }
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
    public SectionMeta? CartPage { get; set; }

    public SectionMeta? FlyingCart { get; set; }
}

/// <summary>The shopping cart settings, as the facade or a single section sends them.</summary>
public sealed class ShoppingCart : Dto
{
    protected override string? EnvelopeKey => "shopping_cart";

    public CartPage? CartPage { get; set; }

    public FlyingCart? FlyingCart { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
