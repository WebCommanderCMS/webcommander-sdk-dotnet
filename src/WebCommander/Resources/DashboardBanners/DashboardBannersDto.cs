namespace WebCommander.Resources.DashboardBanners.DashboardBannersDto;

/// <summary>One step of an <c>open_modal</c> banner's walkthrough.</summary>
public sealed class BannerStep : Dto
{
    public string? Title { get; set; }

    public string? Description { get; set; }
}

/// <summary>What the banner's learn-more button does. <c>action</c> is <c>open_link</c>, with an <c>https</c>
/// <c>url</c>, or <c>open_modal</c>, with a <c>title</c>, a <c>subtitle</c> and at least one of <c>steps</c>. A patch
/// replaces the whole block, so an <c>open_modal</c> block sent over an <c>open_link</c> one drops the
/// <c>url</c>.</summary>
public sealed class BannerLearnMore : Dto
{
    public string? Action { get; set; }

    public string? Label { get; set; }

    public string? Url { get; set; }

    public string? Title { get; set; }

    public string? Subtitle { get; set; }

    public IReadOnlyList<BannerStep?>? Steps { get; set; }
}

/// <summary>The optional second button; both fields are needed.</summary>
public sealed class BannerTryIt : Dto
{
    public string? Label { get; set; }

    public string? Url { get; set; }
}

/// <summary>One banner. <c>key</c> is the <c>banner_key</c> of the other calls. <c>position</c> counts from 1 over
/// every banner, hidden ones included, and <c>active</c> false hides it from the Dashboard feed only.</summary>
public sealed class DashboardBanner : Dto
{
    public string? Key { get; set; }

    public long? Position { get; set; }

    public bool? Active { get; set; }

    public string? Icon { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public IReadOnlyList<string?>? Tags { get; set; }

    public BannerLearnMore? LearnMore { get; set; }

    public BannerTryIt? TryIt { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The management list: every banner, active or not, in position order.</summary>
public sealed class DashboardBannerListing : Dto
{
    public IReadOnlyList<DashboardBanner?>? Banners { get; set; }
}

/// <summary>The write body, bare. A create needs <c>key</c> (<c>a-z</c>, <c>0-9</c> and <c>_</c>, starting with a
/// letter, up to 64 characters, unique), <c>title</c> (1 to 80 characters), <c>description</c> (1 to 200) and
/// <c>learn_more</c>; <c>tags</c> holds at most 3, <c>active</c> defaults to <c>true</c> and <c>position</c> to the
/// end of the list. A patch sends only what changes and cannot send <c>key</c>. <c>icon</c> is stored as sent.
/// <c>position</c> runs from 1 to one past the banner count, and the banners after it shift down.</summary>
public sealed class DashboardBannerInput : Dto
{
    public string? Key { get; set; }

    public string? Icon { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public IReadOnlyList<string?>? Tags { get; set; }

    public BannerLearnMore? LearnMore { get; set; }

    public BannerTryIt? TryIt { get; set; }

    public long? Position { get; set; }

    public bool? Active { get; set; }
}

/// <summary>The support card of the Dashboard.</summary>
public sealed class BannerContact : Dto
{
    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? LiveChat { get; set; }

    public string? HelpCenterUrl { get; set; }
}

/// <summary>The Dashboard's feed: only the active banners, in position order, the support card, whether the store is
/// an online store and the label of the membership.</summary>
public sealed class DashboardContentInfo : Dto
{
    public IReadOnlyList<DashboardBanner?>? Banners { get; set; }

    public BannerContact? Contact { get; set; }

    public bool? OnlineStore { get; set; }

    public string? MemberLabel { get; set; }
}
