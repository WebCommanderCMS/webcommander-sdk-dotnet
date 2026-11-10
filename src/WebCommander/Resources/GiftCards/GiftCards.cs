using WebCommander.Client;
using WebCommander.Resources.GiftCards.GiftCardsDto;

namespace WebCommander.Resources.GiftCards;

/// <summary>Gift cards - <c>/admin/gift_cards</c> - as a typed resource bound to one client.</summary>
public sealed class GiftCards : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.GiftCards</c>.</summary>
    public GiftCards(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List gift cards - <c>GET /admin/gift_cards</c>, newest first. <c>code</c> matches a whole code and
    /// <c>email</c> a whole address, in any case; <c>is_active</c> takes <c>true</c>, <c>false</c>, <c>1</c> or
    /// <c>0</c>, and <c>limit</c> caps at 20 with <c>offset</c> or <c>page</c> to move on. Any other name is
    /// <c>400</c> <c>unknown_parameter</c>.</summary>
    public Task<ApiResponse> ListGiftCardsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(GiftCardsUrls.GiftCards,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(GiftCardPage),
            }, cancellationToken);
}
