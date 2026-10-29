namespace WebCommander.Resources.Settings.SiteSettings.SiteSettingsDto;

/// <summary>A country or a state as a read sends it.</summary>
public sealed class Region : Dto
{
    public long? Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }
}

/// <summary>The <c>auto_suggest.addresses</c> block - whether addresses are suggested while typed and then validated.</summary>
public sealed class AutoSuggestAddresses : Dto
{
    public bool? Enabled { get; set; }

    public bool? Validate { get; set; }
}

/// <summary>The <c>auto_suggest</c> block - address and suburb suggestions.</summary>
public sealed class AutoSuggest : Dto
{
    public AutoSuggestAddresses? Addresses { get; set; }

    public bool? SuburbsCities { get; set; }
}

/// <summary>The <c>captcha</c> block - the challenge on store forms and its provider keys.
/// <see cref="MinimumScore"/> is typed as the body sends it, a number, not as the metadata declares it.</summary>
public sealed class Captcha : Dto
{
    public bool? Enabled { get; set; }

    public double? MinimumScore { get; set; }

    public string? PrivateKey { get; set; }

    public string? PrivateKeyTurnstile { get; set; }

    public string? PrivateKeyV3 { get; set; }

    public string? PublicKey { get; set; }

    public string? PublicKeyTurnstile { get; set; }

    public string? PublicKeyV3 { get; set; }

    public string? Type { get; set; }

    public bool? UseOwn { get; set; }

    public string? Version { get; set; }
}

/// <summary>The <c>administration</c> section - units, precision, default region, error pages and the CAPTCHA block.</summary>
public sealed class Administration : Dto
{
    protected override string? EnvelopeKey => "administration";

    public AutoSuggest? AutoSuggest { get; set; }

    public Captcha? Captcha { get; set; }

    public Region? DefaultCountry { get; set; }

    public Region? DefaultState { get; set; }

    public bool? EcommerceEnabled { get; set; }

    public long? MaxPrecision { get; set; }

    public string? Page403 { get; set; }

    public string? Page404 { get; set; }

    public string? UnitLength { get; set; }

    public string? UnitWeight { get; set; }
}

/// <summary>The <c>brand_kit.colors</c> block - six roles, each a hex colour.</summary>
public sealed class BrandColors : Dto
{
    public string? Accent { get; set; }

    public string? Background { get; set; }

    public string? Primary { get; set; }

    public string? Secondary { get; set; }

    public string? Surface { get; set; }

    public string? Text { get; set; }
}

/// <summary>The <c>brand_kit</c> section - the site-wide palette.</summary>
public sealed class BrandKit : Dto
{
    protected override string? EnvelopeKey => "brand_kit";

    public BrandColors? Colors { get; set; }

    public bool? Enabled { get; set; }

    public string? PaletteName { get; set; }
}

/// <summary>The three fields every <c>date_time</c> audience block carries - date and time formats and a zone.</summary>
public class DateTimeFormat : Dto
{
    public string? DateFormat { get; set; }

    public string? TimeFormat { get; set; }

    public string? TimeZone { get; set; }
}

/// <summary>The <c>date_time.admin</c> block - the admin formats, the only block with <see cref="FirstDayOfWeek"/>.</summary>
public sealed class AdminDateTime : DateTimeFormat
{
    public string? FirstDayOfWeek { get; set; }
}

/// <summary>The <c>date_time.email</c> and <c>date_time.user</c> blocks - the three formats and a separate-zone flag.</summary>
public sealed class SeparableDateTime : DateTimeFormat
{
    public bool? Separate { get; set; }
}

/// <summary>The <c>date_time</c> section - formats and zones per audience.
/// The name is the store's, and it shadows <c>System.DateTime</c> in any file importing this namespace.</summary>
public sealed class DateTime : Dto
{
    protected override string? EnvelopeKey => "date_time";

    public AdminDateTime? Admin { get; set; }

    public SeparableDateTime? Email { get; set; }

    public bool? HideTimeZone { get; set; }

    public SeparableDateTime? User { get; set; }
}

/// <summary>The <c>maintenance</c> block - whether the storefront is closed and what it says while it is.</summary>
public sealed class Maintenance : Dto
{
    public bool? Enabled { get; set; }

    public string? Message { get; set; }
}

/// <summary>The <c>security_code</c> block - the site lock; <see cref="Code"/> reads back masked once set.</summary>
public sealed class SecurityCode : Dto
{
    public string? Code { get; set; }

    public bool? Enabled { get; set; }

    public bool? NumberOnly { get; set; }
}

/// <summary>The <c>ssl.key_file</c> block - what the store holds for the private key, never the key itself.</summary>
public class SslKeyFile : Dto
{
    public string? FileName { get; set; }

    public bool? Uploaded { get; set; }
}

/// <summary>The <c>ssl.certificate_file</c> block - <see cref="SslKeyFile"/>'s two fields, plus the subject and expiry.</summary>
public sealed class SslCertificateFile : SslKeyFile
{
    public string? ExpiresAt { get; set; }

    public string? Subject { get; set; }
}

/// <summary>The <c>ssl</c> block - what is stored for the certificate and its key.</summary>
public sealed class Ssl : Dto
{
    public SslCertificateFile? CertificateFile { get; set; }

    public SslKeyFile? KeyFile { get; set; }
}

/// <summary>The <c>domain_settings</c> section - address, aliases, site lock, maintenance and stored certificate.</summary>
public sealed class DomainSettings : Dto
{
    protected override string? EnvelopeKey => "domain_settings";

    public IReadOnlyList<string?>? Aliases { get; set; }

    public bool? HttpsEnabled { get; set; }

    public Maintenance? Maintenance { get; set; }

    public SecurityCode? SecurityCode { get; set; }

    public Ssl? Ssl { get; set; }

    public string? WebsiteAddress { get; set; }
}

/// <summary>One entry of <c>available</c> - a provider the store can send through.</summary>
public sealed class EmailProvider : Dto
{
    public string? Key { get; set; }

    public string? Label { get; set; }

    public string? Type { get; set; }

    public string? ConnectUrl { get; set; }
}

/// <summary>A <c>reply</c> block - whether replies go somewhere other than the sender address, and where.</summary>
public sealed class ReplyTo : Dto
{
    public bool? Different { get; set; }

    public string? ToEmail { get; set; }

    public string? ToName { get; set; }
}

/// <summary>One entry of <c>default.domains</c> - a platform sending domain and this store's sender identity on it.</summary>
public sealed class DefaultDomain : Dto
{
    public bool? Configure { get; set; }

    public string? Domain { get; set; }

    public string? EmailPrefix { get; set; }

    public ReplyTo? Reply { get; set; }

    public string? SenderName { get; set; }
}

/// <summary>The <c>default</c> block - the platform's own sending domains.</summary>
public sealed class DefaultEmail : Dto
{
    public IReadOnlyList<DefaultDomain?>? Domains { get; set; }
}

/// <summary>The <c>gmail</c> and <c>office365</c> blocks - one read-only <see cref="ConnectUrl"/> each.</summary>
public sealed class OauthProvider : Dto
{
    public string? ConnectUrl { get; set; }
}

/// <summary>The <c>sendgrid</c> block - the SendGrid sender identity and the key behind it.</summary>
public sealed class SendGrid : Dto
{
    public string? ApiKey { get; set; }

    public ReplyTo? Reply { get; set; }

    public string? SenderEmail { get; set; }

    public string? SenderName { get; set; }
}

/// <summary>The <c>smtp.authentication</c> block - whether the server is logged in to and with what.</summary>
public sealed class SmtpAuthentication : Dto
{
    public bool? Enabled { get; set; }

    public string? Password { get; set; }

    public string? Username { get; set; }
}

/// <summary>The <c>smtp</c> block - the server the store sends through and who it sends as.</summary>
public sealed class Smtp : Dto
{
    public SmtpAuthentication? Authentication { get; set; }

    public string? Encryption { get; set; }

    public string? Host { get; set; }

    public long? Port { get; set; }

    public string? SenderEmail { get; set; }

    public string? SenderName { get; set; }
}

/// <summary>The <c>email_settings</c> section - which provider sends the store's mail, and the credentials of each.
/// Two secrets read back as <c>***</c>, so a write names only the fields it moves and never echoes a read.</summary>
public sealed class EmailSettings : Dto
{
    protected override string? EnvelopeKey => "email_settings";

    public IReadOnlyList<EmailProvider?>? Available { get; set; }

    public DefaultEmail? Default { get; set; }

    public OauthProvider? Gmail { get; set; }

    public OauthProvider? Office365 { get; set; }

    public SendGrid? Sendgrid { get; set; }

    public string? ServiceProvider { get; set; }

    public Smtp? Smtp { get; set; }

    /// <summary>The key the store sends the Office 365 block under.</summary>
    public const string StoreOfficeKey = "office365";

    /// <summary>The key <see cref="Office365"/> reads, which the property name derives.</summary>
    public const string DeclaredOfficeKey = "office_365";

    /// <summary>File the store's <c>office365</c> block under <c>office_365</c>, the key a property name derives.</summary>
    protected override IReadOnlyDictionary<string, object?> ReadSource(
        IReadOnlyDictionary<string, object?> body)
    {
        var source = base.ReadSource(body);
        if (!source.TryGetValue(StoreOfficeKey, out var block))
            return source;
        return new Dictionary<string, object?>(source, StringComparer.Ordinal)
        {
            [DeclaredOfficeKey] = block,
        };
    }
}

/// <summary>The <c>image</c> record of either media section - what the store holds and where it serves it.</summary>
public sealed class MediaImage : Dto
{
    public string? ContentType { get; set; }

    public string? FileName { get; set; }

    public long? Height { get; set; }

    public long? SizeBytes { get; set; }

    public string? Url { get; set; }

    public long? Width { get; set; }
}

/// <summary>The <c>favicon</c> section - <c>kind: "media"</c>, read and never written here.</summary>
public sealed class Favicon : Dto
{
    protected override string? EnvelopeKey => "favicon";

    public bool? Enabled { get; set; }

    public MediaImage? Image { get; set; }
}

/// <summary>The <c>store_logo.variants</c> block - the logos for dark backgrounds and the sticky header.</summary>
public sealed class StoreLogoVariants : Dto
{
    // Both read null while the main logo is used; a set variant's shape has not been measured.
    public object? DarkBackground { get; set; }

    public object? StickyHeader { get; set; }
}

/// <summary>The <c>store_logo</c> section - <c>kind: "media"</c>, read and never written here.</summary>
public sealed class StoreLogo : Dto
{
    protected override string? EnvelopeKey => "store_logo";

    public MediaImage? Image { get; set; }

    public StoreLogoVariants? Variants { get; set; }
}

/// <summary>The <c>store_details.address</c> block - where the business is.</summary>
public sealed class StoreAddress : Dto
{
    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public Region? Country { get; set; }

    public string? PostCode { get; set; }

    public Region? State { get; set; }
}

/// <summary>The <c>store_details.contact</c> block - the three numbers the store publishes.</summary>
public sealed class StoreContact : Dto
{
    public string? Fax { get; set; }

    public string? Mobile { get; set; }

    public string? Phone { get; set; }
}

/// <summary>The <c>store_details</c> section - <c>kind: "resource"</c>, every field it declares writable.</summary>
public sealed class StoreDetails : Dto
{
    protected override string? EnvelopeKey => "store_details";

    public string? Abn { get; set; }

    public StoreAddress? Address { get; set; }

    public string? CompanyName { get; set; }

    public StoreContact? Contact { get; set; }

    public string? Email { get; set; }
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
    public SectionMeta? Administration { get; set; }

    public SectionMeta? BrandKit { get; set; }

    public SectionMeta? DateTime { get; set; }

    public SectionMeta? DomainSettings { get; set; }

    public SectionMeta? EmailSettings { get; set; }

    public SectionMeta? Favicon { get; set; }

    public SectionMeta? StoreDetails { get; set; }

    public SectionMeta? StoreLogo { get; set; }
}

/// <summary>The site settings, as the facade or a single section sends them.</summary>
public sealed class SiteSettings : Dto
{
    protected override string? EnvelopeKey => "site_settings";

    public Administration? Administration { get; set; }

    public BrandKit? BrandKit { get; set; }

    public DateTime? DateTime { get; set; }

    public DomainSettings? DomainSettings { get; set; }

    public EmailSettings? EmailSettings { get; set; }

    public Favicon? Favicon { get; set; }

    public StoreDetails? StoreDetails { get; set; }

    public StoreLogo? StoreLogo { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
