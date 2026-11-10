namespace WebCommander.Resources.GiftCards.GiftCardsDto;

/// <summary>One gift card. <see cref="Code"/> is what an order preview's <c>gift-card</c> call takes; a card can be
/// applied only while <see cref="IsActive"/> and <see cref="IsPaid"/>, with a <see cref="Balance"/> above zero, before
/// its <see cref="AvailableTo"/> date and with <see cref="Email"/> equal to the previewed customer's email.</summary>
public sealed class GiftCard : Dto
{
    public long? Id { get; set; }

    public string? Code { get; set; }

    public double? Balance { get; set; }

    public double? InitialAmount { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Message { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsPaid { get; set; }

    public string? AvailableTo { get; set; }

    public string? ExternalReference { get; set; }

    public string? ProductName { get; set; }

    public string? CreatedOn { get; set; }
}

/// <summary>The listing's ten-key <c>pagination</c> block; <see cref="Limit"/> is the size the store applied, at most
/// 20.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public long? Count { get; set; }

    public long? CurrentPage { get; set; }

    public long? TotalPages { get; set; }

    public bool? HasNext { get; set; }

    public bool? HasPrevious { get; set; }

    public string? PreviousPage { get; set; }

    public string? NextPage { get; set; }
}

/// <summary>One page of the listing, newest first.</summary>
public sealed class GiftCardPage : Dto
{
    public IReadOnlyList<GiftCard?>? GiftCards { get; set; }

    public Pagination? Pagination { get; set; }
}
