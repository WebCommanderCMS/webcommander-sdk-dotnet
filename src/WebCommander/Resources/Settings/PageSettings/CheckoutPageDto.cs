namespace WebCommander.Resources.Settings.PageSettings.CheckoutPageDto;

/// <summary>One entry of an address section's <c>fields</c> - a form field and how it is drawn.</summary>
public sealed class AddressField : Dto
{
    public string? Key { get; set; }

    public string? Label { get; set; }

    public bool? Active { get; set; }

    public bool? Required { get; set; }

    public long? Order { get; set; }

    public string? Group { get; set; }
}

/// <summary>An address section's record: <c>active_fields</c> from the facade, every catalog field from the section.</summary>
public class AddressForm : Dto
{
    public IReadOnlyList<string?>? ActiveFields { get; set; }

    public new IReadOnlyList<AddressField?>? Fields { get; set; }
}

/// <summary>The read-only <c>billing_address</c> section.</summary>
public sealed class BillingAddress : AddressForm
{
    protected override string? EnvelopeKey => "billing_address";
}

/// <summary>The read-only <c>shipping_address</c> section.</summary>
public sealed class ShippingAddress : AddressForm
{
    protected override string? EnvelopeKey => "shipping_address";
}

/// <summary>The <c>allow_sign_up</c> block - whether checkout may create an account, and whether it demands one.</summary>
public sealed class SignUp : Dto
{
    public bool? Enabled { get; set; }

    public bool? Required { get; set; }
}

/// <summary>The <c>checkout_as_guest</c> block - guest orders, and whether a guest is offered a one-click sign-up.</summary>
public sealed class GuestCheckout : Dto
{
    public bool? Enabled { get; set; }

    public bool? QuickSignup { get; set; }
}

/// <summary>The <c>customer_checkout</c> section - who may check out and how, in three flat booleans and two blocks.</summary>
public sealed class CustomerCheckout : Dto
{
    protected override string? EnvelopeKey => "customer_checkout";

    public bool? AllowSignIn { get; set; }

    public SignUp? AllowSignUp { get; set; }

    public GuestCheckout? CheckoutAsGuest { get; set; }

    public bool? NewsletterSubscription { get; set; }

    public bool? MultipleShippingMethods { get; set; }
}

/// <summary>The <c>delivery_options</c> section - which delivery choices checkout offers.</summary>
public sealed class DeliveryOptions : Dto
{
    protected override string? EnvelopeKey => "delivery_options";

    public bool? EnableShipping { get; set; }

    public bool? EnableStorePickup { get; set; }

    public bool? EnableShippingQuote { get; set; }

    public bool? EnableDeliveryService { get; set; }
}

/// <summary>The <c>order_summary</c> section - what the checkout's summary panel shows.</summary>
public sealed class OrderSummary : Dto
{
    protected override string? EnvelopeKey => "order_summary";

    public bool? ShowProductThumbnail { get; set; }

    public bool? ShowQuantity { get; set; }

    public bool? EnablePaymentExpandView { get; set; }

    public bool? OrderConfirmCaptcha { get; set; }
}

/// <summary>The <c>comment</c> block - whether an order may carry a customer comment and whether staff are notified.</summary>
public sealed class OrderComment : Dto
{
    public bool? Enabled { get; set; }

    public bool? SendNotification { get; set; }
}

/// <summary>The <c>terms_and_condition.ref</c> block - where the terms text comes from.
/// <see cref="Page"/> is <c>object</c>: <c>field_metadata</c> types it a string but describes a record.</summary>
public sealed class TermsReference : Dto
{
    public object? Page { get; set; }

    public string? Link { get; set; }

    public string? Text { get; set; }
}

/// <summary>The <c>terms_and_condition</c> block - whether terms must be accepted, and the wording around the link.</summary>
public sealed class TermsAndCondition : Dto
{
    public bool? Enabled { get; set; }

    public string? Text { get; set; }

    public string? LinkLabel { get; set; }

    public string? Type { get; set; }

    public TermsReference? Ref { get; set; }
}

/// <summary>The <c>others</c> section - the checkout page's chrome: logo, login link, its own HTML and two blocks.</summary>
public sealed class Others : Dto
{
    protected override string? EnvelopeKey => "others";

    public bool? ShowLogo { get; set; }

    public string? LoginUrl { get; set; }

    public string? CustomHtml { get; set; }

    public OrderComment? Comment { get; set; }

    public TermsAndCondition? TermsAndCondition { get; set; }
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
    public SectionMeta? BillingAddress { get; set; }

    public SectionMeta? ShippingAddress { get; set; }

    public SectionMeta? CustomerCheckout { get; set; }

    public SectionMeta? DeliveryOptions { get; set; }

    public SectionMeta? OrderSummary { get; set; }

    public SectionMeta? Others { get; set; }
}

/// <summary>The checkout page settings, as the facade or a single section sends them.</summary>
public sealed class CheckoutPage : Dto
{
    protected override string? EnvelopeKey => "checkout_page";

    public BillingAddress? BillingAddress { get; set; }

    public ShippingAddress? ShippingAddress { get; set; }

    public CustomerCheckout? CustomerCheckout { get; set; }

    public DeliveryOptions? DeliveryOptions { get; set; }

    public OrderSummary? OrderSummary { get; set; }

    public Others? Others { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
