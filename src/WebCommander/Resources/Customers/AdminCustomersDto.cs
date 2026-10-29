using System.Collections;

namespace WebCommander.Resources.Customers.AdminCustomersDto;

/// <summary>A lookup sent as an object, <c>{id, name, code}</c>; not every source fills all three.</summary>
public sealed class Reference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }
}

/// <summary>One entry of a customer's <c>addresses</c>; the spellings are exact.</summary>
public sealed class Address : Dto
{
    public long? Id { get; set; }

    /// <summary><c>shipping</c>, <c>billing</c> or <c>primary</c>.</summary>
    public string? Type { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? CompanyName { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    /// <summary><c>{id, name, code}</c> inbound; on the way out an id or a name.</summary>
    public Reference? Country { get; set; }

    /// <summary>May arrive as <c>{}</c>, leaving all three fields null.</summary>
    public Reference? State { get; set; }

    public string? City { get; set; }

    public string? PostCode { get; set; }

    public string? Mobile { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }
}

/// <summary>One entry of <c>custom_properties</c>; a write replaces the whole list.</summary>
public sealed class CustomProperty : Dto
{
    /// <summary>Tracks the label/description pair: the same text answers the same id.</summary>
    public long? Id { get; set; }

    public string? Label { get; set; }

    public string? Description { get; set; }
}

/// <summary>One customer; a listing entry carries a subset of the fields a detail fetch does.</summary>
public sealed class Customer : Dto
{
    protected override string? EnvelopeKey => "customer";

    /// <summary>What every path segment resolves against, and what export and import rows key on.</summary>
    public long? CustomerId { get; set; }

    /// <summary>A second id space; not what a path takes - the wrong one answers 200 with another record.</summary>
    public long? InternalId { get; set; }

    /// <summary>A UUID string the store assigns; a create or update naming another one keeps the stored one.</summary>
    public string? Uuid { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Name { get; set; }

    /// <summary>Unique across the store, and held against re-registration even after a delete.</summary>
    public string? Email { get; set; }

    /// <summary><c>active</c>, <c>inactive</c>, <c>pending</c>, <c>approval_awaiting</c> or <c>awaiting_verification</c>; the last is refused change-password for life.</summary>
    public string? Status { get; set; }

    public string? Gender { get; set; }

    public string? CompanyName { get; set; }

    public double? StoreCredit { get; set; }

    public IReadOnlyList<Reference?>? CustomerGroups { get; set; }

    public object? ShippingProfile { get; set; }

    /// <summary>A create answers three of the store's own; primary and billing share one id.</summary>
    public IReadOnlyList<Address?>? Addresses { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public string? Abn { get; set; }

    public string? AbnBranch { get; set; }

    public bool? AccountHold { get; set; }

    public long? ActiveBillingAddress { get; set; }

    public long? ActiveShippingAddress { get; set; }

    public bool? AllowCreditLimit { get; set; }

    public string? BackgroundImage { get; set; }

    public string? BaseUrl { get; set; }

    public long? CountReferralCodeUsed { get; set; }

    public double? CreditLimit { get; set; }

    public IReadOnlyList<CustomProperty?>? CustomProperties { get; set; }

    public bool? CustomerPasswordExpired { get; set; }

    public string? DefaultTaxCode { get; set; }

    public bool? EmailVerified { get; set; }

    public bool? HidePrice { get; set; }

    public string? HowDoYouKnow { get; set; }

    /// <summary>Whether the record is in the trash - what a soft delete leaves behind.</summary>
    public bool? IsInTrash { get; set; }

    public IReadOnlyList<object?>? Newsletters { get; set; }

    public string? Note { get; set; }

    public object? PaymentTerm { get; set; }

    public object? ProfilePicture { get; set; }

    public object? ReferralCode { get; set; }

    public bool? SmsSubscribed { get; set; }

    public string? Source { get; set; }

    public string? Timezone { get; set; }

    public string? TransactionNumber { get; set; }

    public object? UsedReferralCode { get; set; }

    public long? Version { get; set; }
}

/// <summary>The writable form of <see cref="Customer"/>; FirstName and Email are required.</summary>
public sealed class CustomerInput : Dto
{
    protected override string? EnvelopeKey => "customer";

    protected override IReadOnlyList<string> TopLevel { get; } = new[] { "transaction_no" };

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public bool? EmailVerified { get; set; }

    public string? Password { get; set; }

    /// <summary>The password being replaced; only change-password takes it.</summary>
    public string? OldPassword { get; set; }

    /// <summary>Anything but <c>"active"</c> files the record <c>awaiting_verification</c>, unusable for life.</summary>
    public string? Status { get; set; }

    /// <summary><c>false</c> is a value that is sent; leave it unset to omit the field.</summary>
    public bool? Company { get; set; }

    public string? CompanyName { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    /// <summary>An id or a name.</summary>
    public object? State { get; set; }

    /// <summary>An id or a name.</summary>
    public object? Country { get; set; }

    public string? PostCode { get; set; }

    public IReadOnlyList<Address?>? Addresses { get; set; }

    public IReadOnlyList<CustomProperty?>? CustomProperties { get; set; }

    /// <summary><c>transaction_no</c>, sent beside the <c>customer</c> object rather than inside it.</summary>
    public string? TransactionNo { get; set; }
}

/// <summary>One movement of a customer's store credit; a history entry leaves the balances null.</summary>
public sealed class StoreCreditAdjustment : Dto
{
    protected override string? EnvelopeKey => "store_credit_adjustment";

    public long? Id { get; set; }

    public long? CustomerId { get; set; }

    /// <summary>The customer's second id - not the movement's own, and not what the path takes.</summary>
    public long? InternalId { get; set; }

    /// <summary><c>add</c> or <c>deduct</c>; the sign lives here, <see cref="Amount"/> being unsigned.</summary>
    public string? Action { get; set; }

    public double? Amount { get; set; }

    public string? Note { get; set; }

    public string? CreatedAt { get; set; }

    public object? CreatedBy { get; set; }

    public double? BalanceBefore { get; set; }

    public double? BalanceAfter { get; set; }

    /// <summary>Read off <c>store_credit</c> beside the envelope; equal to <see cref="BalanceAfter"/>.</summary>
    public double? StoreCredit { get; set; }
}

/// <summary>The writable form of <see cref="StoreCreditAdjustment"/>, written with no envelope.</summary>
public sealed class StoreCreditAdjustmentInput : Dto
{
    /// <summary><c>add</c> or <c>deduct</c>; <see cref="Amount"/> is unsigned.</summary>
    public string? Action { get; set; }

    public double? Amount { get; set; }

    public string? Note { get; set; }
}

/// <summary>The <c>pagination</c> block of a listing; not the <c>Client.Pagination</c> walker.</summary>
public sealed class Pagination : Dto
{
    /// <summary>How many records match across every page; <see cref="Count"/> is this page's.</summary>
    public long? Total { get; set; }

    /// <summary>The page size the store applied: capped at 20 on the customer listings and at 100 on the SEO ones.</summary>
    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public long? Count { get; set; }

    /// <summary>Counting from 1.</summary>
    public long? CurrentPage { get; set; }

    public long? TotalPages { get; set; }

    public bool? HasNext { get; set; }

    public bool? HasPrevious { get; set; }

    public string? PreviousPage { get; set; }

    public string? NextPage { get; set; }
}

/// <summary>One page of a listing; the records are found by shape - the envelope's single list.</summary>
public sealed class Page<TItem> : Dto where TItem : Dto
{
    /// <summary>The page's records; <c>null</c> when the body carried no single list.</summary>
    public IReadOnlyList<TItem?>? Items { get; set; }

    public Pagination? Pagination { get; set; }

    private const string ItemsKey = "items";

    /// <summary>File the body's one list under the name <see cref="Items"/> reads.</summary>
    protected override IReadOnlyDictionary<string, object?> ReadSource(
        IReadOnlyDictionary<string, object?> body) =>
        new Dictionary<string, object?>(body, StringComparer.Ordinal)
        {
            [ItemsKey] = OneList(body),
        };

    private static object? OneList(IReadOnlyDictionary<string, object?> body)
    {
        object? records = null;
        var found = 0;
        foreach (var value in body.Values)
        {
            if (value is not IEnumerable || value is string || value is IDictionary)
                continue;
            records = value;
            found++;
        }
        return found == 1 ? records : null;
    }
}
