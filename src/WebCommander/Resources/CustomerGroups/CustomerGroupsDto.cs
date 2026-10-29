namespace WebCommander.Resources.CustomerGroups.CustomerGroupsDto;

/// <summary>The shipping profile a group names; a write sends the bare id, not this record.</summary>
public sealed class CustomerGroupShippingProfile : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>The stored image; a write sends a base64 or <c>data:</c> URI string, and <c>null</c> removes it.</summary>
public sealed class CustomerGroupBackgroundImage : Dto
{
    public string? Url { get; set; }

    public long? Width { get; set; }

    public long? Height { get; set; }
}

/// <summary>An <c>enabled</c> switch and its <c>amount</c>: <c>minimum_purchase</c>, <c>max_purchase</c> and
/// <c>credit_limit</c> all take this shape.</summary>
public sealed class CustomerGroupAmountSetting : Dto
{
    public bool? Enabled { get; set; }

    public double? Amount { get; set; }
}

/// <summary><see cref="Terms"/> is written <c>NET_30</c>, <c>NET_60</c> or <c>NET_90</c> and reads back in lower case.</summary>
public sealed class CustomerGroupPaymentTerms : Dto
{
    public bool? Enabled { get; set; }

    public string? Terms { get; set; }
}

public sealed class CustomerGroupOnAccountPurchase : Dto
{
    public bool? Enabled { get; set; }

    public CustomerGroupAmountSetting? MaxPurchase { get; set; }

    public CustomerGroupAmountSetting? CreditLimit { get; set; }

    public CustomerGroupPaymentTerms? PaymentTerms { get; set; }
}

public sealed class CustomerGroupPurchasePreference : Dto
{
    public CustomerGroupAmountSetting? MinimumPurchase { get; set; }

    public CustomerGroupOnAccountPurchase? OnAccountPurchase { get; set; }
}

/// <summary><see cref="BasePriceRounding"/> is <c>nearest</c>, <c>up</c> or <c>down</c>.</summary>
public sealed class CustomerGroupTaxConfiguration : Dto
{
    public bool? OverrideStoreTax { get; set; }

    public bool? TaxExempt { get; set; }

    public string? TaxMessage { get; set; }

    public bool? ShowPricesWithTaxIncluded { get; set; }

    public bool? PricesEnteredWithTaxIncluded { get; set; }

    public string? BasePriceRounding { get; set; }
}

/// <summary><see cref="PriceDisplayMethod"/> is <c>lowest</c> or <c>highest</c>.</summary>
public sealed class CustomerGroupProductDisplay : Dto
{
    public bool? OverwriteDefault { get; set; }

    public string? VariationDisplayType { get; set; }

    public bool? DisplayParentImage { get; set; }

    public bool? ActivateColourVariationImage { get; set; }

    public bool? ShowSelectedVariationValue { get; set; }

    public bool? StrikeThroughOutOfStock { get; set; }

    public bool? SingleClickVariationLoading { get; set; }

    public string? PriceDisplayMethod { get; set; }
}

/// <summary>One member, by its <c>customer_id</c>: an entry of <c>customers</c> and the membership read.</summary>
public sealed class CustomerGroupMember : Dto
{
    protected override string? EnvelopeKey => "customer";

    public long? Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }
}

/// <summary>One group. A listing row and a create, patch or assign answer carry the thirteen fields up to
/// <see cref="UpdatedAt"/>; the row read and the membership write add the next two.</summary>
public sealed class CustomerGroup : Dto
{
    protected override string? EnvelopeKey => "customer_group";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? DefaultTaxCode { get; set; }

    public bool? AddAsNewCustomerDefaultGroup { get; set; }

    public CustomerGroupShippingProfile? ShippingProfile { get; set; }

    public CustomerGroupBackgroundImage? BackgroundImage { get; set; }

    public CustomerGroupPurchasePreference? PurchasePreference { get; set; }

    public CustomerGroupTaxConfiguration? TaxConfiguration { get; set; }

    public CustomerGroupProductDisplay? ProductDisplay { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public long? CustomerCount { get; set; }

    public IReadOnlyList<CustomerGroupMember?>? Customers { get; set; }

    /// <summary><c>object</c>, the schema's entries differing in width.</summary>
    public object? FieldMetadata { get; set; }
}

/// <summary>The five-key <c>pagination</c> block; <see cref="Limit"/> is the page size applied.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the listing: the rows, the block beside them and any schema, with no envelope.</summary>
public sealed class CustomerGroupPage : Dto
{
    public IReadOnlyList<CustomerGroup?>? CustomerGroups { get; set; }

    public Pagination? Pagination { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary>The count sub-path's body, a bare <c>{"total": N}</c>.</summary>
public sealed class CustomerGroupCount : Dto
{
    public long? Total { get; set; }
}

/// <summary>The assign answer: <see cref="Added"/> is <c>false</c> when the customer was already a member.</summary>
public sealed class CustomerGroupAssignment : Dto
{
    public string? Status { get; set; }

    public long? CustomerId { get; set; }

    public string? Email { get; set; }

    public CustomerGroup? Group { get; set; }

    public bool? Added { get; set; }
}

/// <summary>The writable form of <see cref="CustomerGroup"/>, for the create and the patch. <see cref="Status"/> takes
/// <c>A</c>, <c>I</c>, <c>active</c> or <c>inactive</c>; <see cref="ShippingProfile"/> is a bare id.</summary>
public sealed class CustomerGroupInput : Dto
{
    protected override string? EnvelopeKey => "customer_group";

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? DefaultTaxCode { get; set; }

    public bool? AddAsNewCustomerDefaultGroup { get; set; }

    public long? ShippingProfile { get; set; }

    public string? BackgroundImage { get; set; }

    public CustomerGroupPurchasePreference? PurchasePreference { get; set; }

    public CustomerGroupTaxConfiguration? TaxConfiguration { get; set; }

    public CustomerGroupProductDisplay? ProductDisplay { get; set; }
}

/// <summary>The membership body: the whole member list, replacing the stored one; <c>[]</c> empties it. An entry is a
/// <c>customer_id</c>, a number or its text.</summary>
public sealed class CustomerGroupCustomersInput : Dto
{
    public IReadOnlyList<object?>? Customers { get; set; }
}

/// <summary>The assign body, both ids bare, each a number or its text: <see cref="Customer"/> is a <c>customer_id</c>.</summary>
public sealed class CustomerGroupAssignInput : Dto
{
    public object? Customer { get; set; }

    public object? Group { get; set; }
}
