namespace WebCommander.Resources.TaxCustomProfiles.TaxCustomProfilesDto;

/// <summary>The <c>pagination</c> block of the custom profile listing.</summary>
public sealed class Pagination : Dto
{
    /// <summary>How many rows match across every page.</summary>
    public long? Total { get; set; }

    /// <summary>The page size in effect, clamped to 100.</summary>
    public long? Limit { get; set; }

    public long? Offset { get; set; }

    /// <summary>The rows on this page; <see cref="Total"/> is the rows matching.</summary>
    public long? Count { get; set; }

    public long? CurrentPage { get; set; }

    public long? TotalPages { get; set; }

    public bool? HasNext { get; set; }

    public bool? HasPrevious { get; set; }

    public string? PreviousPage { get; set; }

    public string? NextPage { get; set; }
}

/// <summary>One profile a rule is filed under, on the rule's <c>profiles</c>.</summary>
public sealed class CustomProfileReference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>One entry of a custom profile's <c>rules</c>, sent only by the row path.</summary>
public sealed class CustomProfileRule : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public long? ZonesCount { get; set; }

    public IReadOnlyList<CustomProfileReference?>? Profiles { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedOn { get; set; }
}

/// <summary>One custom tax profile; the row path adds <see cref="Rules"/> to the eight keys a listing row, the
/// create and the update answer carry.</summary>
public sealed class CustomProfile : Dto
{
    protected override string? EnvelopeKey => "custom_profile";

    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? TaxCode { get; set; }

    public string? Description { get; set; }

    public bool? IsDefault { get; set; }

    public long? RulesCount { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedOn { get; set; }

    public IReadOnlyList<CustomProfileRule?>? Rules { get; set; }
}

/// <summary>The writable form of <see cref="CustomProfile"/>; <see cref="Name"/> is the create's one requirement.
/// The update is a replace that keeps an omitted name and empties an omitted description and tax code.</summary>
public sealed class CustomProfileInput : Dto
{
    protected override string? EnvelopeKey => "custom_profile";

    protected override IReadOnlyList<string> TopLevel { get; } = new[] { "transaction_no" };

    public string? Name { get; set; }

    public string? TaxCode { get; set; }

    public string? Description { get; set; }

    public bool? IsDefault { get; set; }

    /// <summary>Beside the envelope, where the create files the profile; the update files it only on a profile no number holds yet.</summary>
    public string? TransactionNo { get; set; }
}

/// <summary>One <c>group_meta</c> entry: what the path is, whether it is writable and where.</summary>
public sealed class ResourceMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block only the listing sends.</summary>
public sealed class GroupMeta : Dto
{
    public ResourceMeta? CustomProfile { get; set; }

    public ResourceMeta? Rules { get; set; }
}

/// <summary>One page of the custom profile listing: the rows, <c>group_meta</c> and the paging block.</summary>
public sealed class CustomProfilePage : Dto
{
    public IReadOnlyList<CustomProfile?>? CustomProfiles { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public Pagination? Pagination { get; set; }
}
