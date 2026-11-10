namespace WebCommander.Resources.ShippingProfiles.ShippingProfilesDto;

/// <summary>An <c>{id, name}</c> pair: a rule's rate, and a zone named in the wizard, where a write sends
/// <c>{"id": ...}</c> for an existing zone or <c>{"name": ...}</c> to create one.</summary>
public sealed class ShippingReference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>A rule on a profile; <see cref="ShippingRate"/> is <c>null</c> on a rule that has no rate.</summary>
public sealed class ShippingProfileRule : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public ShippingReference? ShippingRate { get; set; }
}

/// <summary>One profile, the same ten keys on a listing row, the row read and every write's answer.
/// <see cref="RulePreference"/> reads <c>product_assigned_rule</c>, <c>lowest_shipping_cost_rule</c> or
/// <c>highest_shipping_cost_rule</c>; exactly one profile has <see cref="Default"/> true.</summary>
public sealed class ShippingProfile : Dto
{
    protected override string? EnvelopeKey => "shipping_profile";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? RulePreference { get; set; }

    public bool? Default { get; set; }

    public long? RulesCount { get; set; }

    public IReadOnlyList<ShippingProfileRule?>? ShippingRules { get; set; }

    public string? TransactionNo { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The writable fields, wrapped in <c>shipping_profile</c>, for the create, the <c>PUT</c> replace and the
/// <c>PATCH</c>. <see cref="ShippingRules"/> is the whole list of rule ids, each rule once; <c>PUT</c> needs
/// <see cref="Name"/>, <see cref="Description"/>, <see cref="RulePreference"/> and <see cref="ShippingRules"/>.
/// <see cref="Default"/> takes only <c>true</c> or <c>false</c>. A <c>null</c> field is not sent: to clear one, pass a
/// map.</summary>
public sealed class ShippingProfileInput : Dto
{
    protected override string? EnvelopeKey => "shipping_profile";

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? RulePreference { get; set; }

    public bool? Default { get; set; }

    public IReadOnlyList<long?>? ShippingRules { get; set; }

    public string? TransactionNo { get; set; }

    public bool? SuggestName { get; set; }
}

/// <summary>The rate a wizard rule creates; the fields of a rate write, <see cref="Name"/> and <see cref="Method"/>
/// required. The costs, rows and additional rule stay untyped here, as they are on the rate write.</summary>
public sealed class ShippingProfileWizardRate : Dto
{
    public string? Name { get; set; }

    public string? Method { get; set; }

    public bool? CalculateForEntireOrder { get; set; }

    public bool? PriceEnteredWithTax { get; set; }

    public object? ShippingCost { get; set; }

    public object? HandlingCost { get; set; }

    public IReadOnlyList<object?>? Conditions { get; set; }

    public object? AdditionalRule { get; set; }

    public string? TransactionNo { get; set; }
}

/// <summary>A rule the wizard creates beside its rate: <see cref="Name"/> is required, <see cref="ShippingClass"/> is a
/// class id, and <see cref="Zones"/> name existing zones by <c>{"id": ...}</c> or new ones by
/// <c>{"name": ...}</c>.</summary>
public sealed class ShippingProfileWizardRule : Dto
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public long? ShippingClass { get; set; }

    public ShippingProfileWizardRate? ShippingRate { get; set; }

    public IReadOnlyList<ShippingReference?>? Zones { get; set; }
}

/// <summary>The wizard's body, wrapped in <c>shipping_profile</c>: the profile fields and the rules, each with its rate,
/// to create under it.</summary>
public sealed class ShippingProfileWizardInput : Dto
{
    protected override string? EnvelopeKey => "shipping_profile";

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? RulePreference { get; set; }

    public IReadOnlyList<ShippingProfileWizardRule?>? ShippingRules { get; set; }
}

/// <summary>The five-key <c>pagination</c> block; <see cref="Limit"/> is the size the store applied.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the listing, newest first.</summary>
public sealed class ShippingProfileListing : Dto
{
    public IReadOnlyList<ShippingProfile?>? ShippingProfiles { get; set; }

    public Pagination? Pagination { get; set; }
}

/// <summary>The count sub-path's body, a bare <c>{"total": N}</c>.</summary>
public sealed class ShippingProfileCount : Dto
{
    public long? Total { get; set; }
}
