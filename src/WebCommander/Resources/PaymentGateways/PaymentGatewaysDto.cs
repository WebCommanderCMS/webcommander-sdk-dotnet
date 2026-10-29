namespace WebCommander.Resources.PaymentGateways.PaymentGatewaysDto;

/// <summary>An <c>{id, name}</c> pair: the gateway's <c>zone</c>, and each audience member it names.</summary>
public sealed class PaymentGatewayReference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>Who sees the gateway at checkout; the lists fill only under <c>type</c> <c>selected</c>.</summary>
public sealed class PaymentGatewayVisibility : Dto
{
    public string? Type { get; set; }

    public IReadOnlyList<PaymentGatewayReference?>? Customers { get; set; }

    public IReadOnlyList<PaymentGatewayReference?>? CustomerGroups { get; set; }
}

/// <summary>The delivery types the gateway is offered for; <c>null</c> on gateways without the choice.</summary>
public sealed class PaymentGatewayAllowedFor : Dto
{
    public bool? Shipping { get; set; }

    public bool? Delivery { get; set; }

    public bool? StorePickup { get; set; }
}

/// <summary>The surcharge block, <c>null</c> on gateways without one, and its write; <see cref="PercentAmount"/>
/// reads <c>null</c> when no percentage is set.</summary>
public sealed class PaymentGatewaySurcharge : Dto
{
    public string? Type { get; set; }

    public double? FlatAmount { get; set; }

    public double? PercentAmount { get; set; }

    /// <summary>Each range as its own mapping (<c>id</c>, <c>from</c>, <c>to</c>, <c>amount</c>); every live
    /// gateway answered <c>[]</c>.</summary>
    public IReadOnlyList<object?>? Ranges { get; set; }
}

/// <summary>The pending and completed messages; a message the gateway's form lacks reads <c>null</c>.</summary>
public sealed class PaymentGatewayMessages : Dto
{
    public string? Pending { get; set; }

    public string? Completed { get; set; }
}

/// <summary>PayPal's <c>standard</c> credentials.</summary>
public sealed class PaymentGatewayPayPalStandard : Dto
{
    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }
}

/// <summary>PayPal's <c>classic</c> account.</summary>
public sealed class PaymentGatewayPayPalClassic : Dto
{
    public string? EmailAddress { get; set; }
}

/// <summary>A one-key <c>{"enabled": ...}</c> switch.</summary>
public sealed class PaymentGatewayToggle : Dto
{
    public bool? Enabled { get; set; }
}

/// <summary>The card gateway's processor; the client id and secret read <c>********</c> when set.</summary>
public sealed class PaymentGatewayCardProcessor : Dto
{
    public string? Type { get; set; }

    public string? Mode { get; set; }

    public string? MerchantCode { get; set; }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public PaymentGatewayToggle? WalletPayment { get; set; }
}

/// <summary>The card logos shown at checkout.</summary>
public sealed class PaymentGatewayCardLogo : Dto
{
    public string? Type { get; set; }

    public IReadOnlyList<string?>? Cards { get; set; }

    /// <summary><c>object</c>: every gateway answered <c>null</c>.</summary>
    public object? CustomImage { get; set; }
}

/// <summary>Express Checkout's Google Pay block.</summary>
public sealed class PaymentGatewayGooglePay : Dto
{
    public bool? Enabled { get; set; }

    public string? PaymentMode { get; set; }

    public string? MerchantName { get; set; }

    public string? MerchantId { get; set; }

    public string? Processor { get; set; }

    public string? SecretKey { get; set; }

    public string? PublicKey { get; set; }

    public double? FreeShippingConditionAmount { get; set; }

    public double? ShippingCost { get; set; }
}

/// <summary>Express Checkout's Apple Pay block.</summary>
public sealed class PaymentGatewayApplePay : Dto
{
    public bool? Enabled { get; set; }

    public string? PaymentMode { get; set; }

    public string? MerchantIdentifier { get; set; }

    public string? DomainName { get; set; }

    public string? DisplayName { get; set; }

    public string? Processor { get; set; }

    public string? SecretKey { get; set; }

    public string? PublicKey { get; set; }
}

/// <summary>Express Checkout's PayPal block.</summary>
public sealed class PaymentGatewayExpressPayPal : Dto
{
    public bool? ShowInstallmentAmount { get; set; }
}

/// <summary>The row read's gateway-specific <c>settings</c>, every key any gateway answers; each gateway
/// fills its own few and offline gateways answer <c>{}</c>.</summary>
public sealed class PaymentGatewaySettings : Dto
{
    public string? Version { get; set; }

    public string? Mode { get; set; }

    public PaymentGatewayPayPalStandard? Standard { get; set; }

    public PaymentGatewayPayPalClassic? Classic { get; set; }

    public PaymentGatewayCardProcessor? Processor { get; set; }

    public PaymentGatewayCardLogo? CardLogo { get; set; }

    public PaymentGatewayGooglePay? GooglePay { get; set; }

    public PaymentGatewayApplePay? ApplePay { get; set; }

    /// <summary>Spelled <c>Paypal</c> so the wire name is <c>paypal</c>.</summary>
    public PaymentGatewayExpressPayPal? Paypal { get; set; }

    public bool? ShowInstallmentAmount { get; set; }

    public bool? ShowInProductDetails { get; set; }

    public string? ApiKey { get; set; }

    public long? DefaultTerm { get; set; }

    public double? AmountPer100Points { get; set; }

    public string? MerchantId { get; set; }

    public string? MerchantSecretKey { get; set; }

    /// <summary><c>object</c>: every gateway answered <c>null</c>.</summary>
    public object? Image { get; set; }
}

/// <summary>One gateway: a listing row, and the row path's and every write's body, which add
/// <see cref="Settings"/>.</summary>
public sealed class PaymentGateway : Dto
{
    protected override string? EnvelopeKey => "payment_gateway";

    public long? Id { get; set; }

    /// <summary>Fixed per gateway type, and read-only.</summary>
    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? Added { get; set; }

    public bool? Enabled { get; set; }

    public bool? Default { get; set; }

    public bool? Promotional { get; set; }

    public PaymentGatewayReference? Zone { get; set; }

    public string? Information { get; set; }

    public PaymentGatewayVisibility? Visibility { get; set; }

    public PaymentGatewayAllowedFor? AllowedFor { get; set; }

    public PaymentGatewaySurcharge? Surcharge { get; set; }

    public PaymentGatewayMessages? Messages { get; set; }

    public PaymentGatewaySettings? Settings { get; set; }
}

/// <summary>The visibility a write takes: customers and customer groups by id.</summary>
public sealed class PaymentGatewayVisibilityInput : Dto
{
    public string? Type { get; set; }

    public IReadOnlyList<long?>? Customers { get; set; }

    public IReadOnlyList<long?>? CustomerGroups { get; set; }
}

/// <summary>The writable fields, for the <c>PUT</c> and the <c>PATCH</c>; no envelope. <see cref="Zone"/>
/// takes a zone's id or a <see cref="PaymentGatewayReference"/>; <c>code</c> and <c>promotional</c> are
/// read-only.</summary>
public sealed class PaymentGatewayInput : Dto
{
    public string? Name { get; set; }

    public bool? Added { get; set; }

    public bool? Enabled { get; set; }

    public bool? Default { get; set; }

    public object? Zone { get; set; }

    public string? Information { get; set; }

    public PaymentGatewayVisibilityInput? Visibility { get; set; }

    public PaymentGatewayAllowedFor? AllowedFor { get; set; }

    public PaymentGatewaySurcharge? Surcharge { get; set; }

    public PaymentGatewayMessages? Messages { get; set; }

    public PaymentGatewaySettings? Settings { get; set; }
}

/// <summary>The five-key <c>pagination</c> block; <see cref="Limit"/> is the page size applied.
/// <see cref="HasPrevious"/> is <c>true</c> past the first page and <c>false</c> on a page past the end.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the listing: the rows, without <c>settings</c>, and where they sit.</summary>
public sealed class PaymentGatewayPage : Dto
{
    public IReadOnlyList<PaymentGateway?>? PaymentGateways { get; set; }

    public Pagination? Pagination { get; set; }
}

/// <summary>The count sub-path's body, a bare <c>{"total": N}</c>.</summary>
public sealed class PaymentGatewayCount : Dto
{
    public long? Total { get; set; }
}
