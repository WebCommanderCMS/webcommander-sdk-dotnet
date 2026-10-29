namespace WebCommander.Resources.Settings.CustomerPortal.CustomerPortalDto;

/// <summary>One default or approval group as a read sends it; a write takes bare ids.</summary>
public sealed class CustomerGroup : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>The <c>portal</c> section - the one switch that turns the customer portal on.</summary>
public sealed class Portal : Dto
{
    protected override string? EnvelopeKey => "portal";

    public bool? Enabled { get; set; }
}

/// <summary>The <c>terms</c> block - the terms line shown on the registration form.</summary>
public sealed class RegistrationTerms : Dto
{
    public bool? Enabled { get; set; }

    public string? Text { get; set; }

    public string? AnchorText { get; set; }

    public string? Type { get; set; }

    public string? Reference { get; set; }
}

/// <summary>The <c>facebook</c> block - registering through Facebook.</summary>
public sealed class FacebookLogin : Dto
{
    public bool? Enabled { get; set; }

    public string? AppId { get; set; }
}

/// <summary>The <c>google</c> block - registering through Google.</summary>
public sealed class GoogleLogin : Dto
{
    public bool? Enabled { get; set; }

    public string? ClientId { get; set; }
}

/// <summary>The <c>b2b_configuration</c> block - what a new registrant is created as.</summary>
public sealed class B2bConfiguration : Dto
{
    public bool? Enabled { get; set; }

    public bool? CreateAsOrganisation { get; set; }

    public bool? CreateAsCustomer { get; set; }

    public bool? OrganisationSelected { get; set; }
}

/// <summary>The <c>alternate_login</c> block - third-party login and whether it offers an email field.</summary>
public sealed class AlternateLogin : Dto
{
    public bool? Enabled { get; set; }

    public bool? EnableEmailField { get; set; }
}

/// <summary>The <c>registration</c> section - whether customers may register, and as what.</summary>
public sealed class Registration : Dto
{
    protected override string? EnvelopeKey => "registration";

    public bool? Enabled { get; set; }

    public string? CloseRegistrationMessage { get; set; }

    public bool? AssignDefaultGroup { get; set; }

    public IReadOnlyList<CustomerGroup?>? DefaultGroups { get; set; }

    public bool? RequiresApproval { get; set; }

    public IReadOnlyList<CustomerGroup?>? ApprovalGroups { get; set; }

    public RegistrationTerms? Terms { get; set; }

    public FacebookLogin? Facebook { get; set; }

    public GoogleLogin? Google { get; set; }

    public B2bConfiguration? B2bConfiguration { get; set; }

    public AlternateLogin? AlternateLogin { get; set; }
}

/// <summary>The <c>captcha</c> block - the challenge shown after failed logins.</summary>
public sealed class LoginCaptcha : Dto
{
    public bool? Enabled { get; set; }

    public long? FailCount { get; set; }
}

/// <summary>The <c>login</c> section - whether customers may sign in and what the page offers.</summary>
public sealed class Login : Dto
{
    protected override string? EnvelopeKey => "login";

    public bool? Enabled { get; set; }

    public string? LoginDisabledMessage { get; set; }

    public string? AwaitingApprovalMessage { get; set; }

    public bool? PasswordRecovery { get; set; }

    public bool? RegistrationLink { get; set; }

    public LoginCaptcha? Captcha { get; set; }

    public bool? EnableLandingPageAfterSuccessfulLogin { get; set; }

    public string? LandingPageAfterSuccessfulLogin { get; set; }
}

/// <summary>The <c>azure_ad.data_mapping</c> block - which assertion attribute fills which field.</summary>
public sealed class AzureAdDataMapping : Dto
{
    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? PostCode { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public long? CountryId { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }
}

/// <summary>The <c>azure_ad</c> block - the SAML provider.</summary>
public sealed class AzureAd : Dto
{
    public AzureAdDataMapping? DataMapping { get; set; }

    public string? Identifier { get; set; }

    public string? LoginUrl { get; set; }

    public string? ReplyUrlPostfix { get; set; }
}

/// <summary>The <c>azure_ad_b2c</c> block - the B2C provider; the store does not mask its secret.</summary>
public sealed class AzureAdB2c : Dto
{
    public string? TenantName { get; set; }

    public string? PolicyName { get; set; }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public string? ReplyUrlPrefix { get; set; }

    public string? ReplyUrlPostfix { get; set; }
}

/// <summary>The <c>sso</c> section - single sign-on and which provider serves it.</summary>
public sealed class Sso : Dto
{
    protected override string? EnvelopeKey => "sso";

    public bool? Enabled { get; set; }

    public bool? RequireSiteAccess { get; set; }

    public string? ServiceProvider { get; set; }

    public AzureAd? AzureAd { get; set; }

    public AzureAdB2c? AzureAdB2c { get; set; }
}

/// <summary>One row of a profile-page form: which field, whether it is drawn and where.</summary>
public class ProfileField : Dto
{
    public string? Key { get; set; }

    public bool? Active { get; set; }

    public long? Order { get; set; }
}

/// <summary>One row of a named profile-page list, with a <see cref="Label"/>.</summary>
public class ProfileEntry : ProfileField
{
    public string? Label { get; set; }
}

/// <summary>One row of <c>overview</c>; the store sends <see cref="ShowProductInfo"/> on some rows only.</summary>
public sealed class ProfileOverviewEntry : ProfileEntry
{
    public bool? ShowProductInfo { get; set; }
}

/// <summary>The <c>manage_my_account</c> block - the account screen's sections and form fields.</summary>
public sealed class ManageMyAccount : Dto
{
    public IReadOnlyList<ProfileEntry?>? Sections { get; set; }

    public IReadOnlyList<ProfileField?>? AccountInformation { get; set; }

    public IReadOnlyList<ProfileField?>? BillingAddress { get; set; }

    public IReadOnlyList<ProfileField?>? ShippingAddress { get; set; }
}

/// <summary>The <c>my_orders.flags</c> block - six booleans.</summary>
public sealed class MyOrderFlags : Dto
{
    public bool? EnableDuePayment { get; set; }

    public bool? ShowTrackMyOrders { get; set; }

    public bool? DisplayTrackInColumn { get; set; }

    public bool? ShowOrderChannel { get; set; }

    public bool? ShowPayNow { get; set; }

    public bool? AllowOrderShare { get; set; }
}

/// <summary>The <c>my_orders</c> block - what the orders tab shows and which lists it offers.</summary>
public sealed class MyOrders : Dto
{
    public MyOrderFlags? Flags { get; set; }

    public IReadOnlyList<ProfileEntry?>? Sections { get; set; }
}

/// <summary>The <c>assign_to_customer</c> block - the assigned-pages tab and how it paginates.</summary>
public sealed class AssignToCustomer : Dto
{
    public IReadOnlyList<ProfileEntry?>? Items { get; set; }

    public string? ShowPagination { get; set; }

    public long? ItemPerPage { get; set; }
}

/// <summary>The read-only <c>profile_page</c> section - every tab of the customer's own page.</summary>
public sealed class ProfilePage : Dto
{
    protected override string? EnvelopeKey => "profile_page";

    public string? PageHtml { get; set; }

    public IReadOnlyList<ProfileEntry?>? Tabs { get; set; }

    public IReadOnlyList<ProfileOverviewEntry?>? Overview { get; set; }

    public ManageMyAccount? ManageMyAccount { get; set; }

    public MyOrders? MyOrders { get; set; }

    public IReadOnlyList<ProfileEntry?>? MyEntitlements { get; set; }

    public AssignToCustomer? AssignToCustomer { get; set; }
}

/// <summary>One row of the registration form's field catalog.</summary>
public sealed class RegistrationField : Dto
{
    public string? Key { get; set; }

    public string? Label { get; set; }

    public string? DefaultLabel { get; set; }

    public bool? Active { get; set; }

    public bool? Required { get; set; }

    public bool? System { get; set; }

    public long? Order { get; set; }

    public string? Placeholder { get; set; }
}

/// <summary>The read-only <c>registration_fields</c> section - the registration form's field catalog.</summary>
public sealed class RegistrationFields : Dto
{
    protected override string? EnvelopeKey => "registration_fields";

    public new IReadOnlyList<RegistrationField?>? Fields { get; set; }
}

/// <summary>One entry of <c>group_meta</c>: what a thing is, whether it is writable, where it is managed.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block, one <see cref="SectionMeta"/> per section; only the facade sends it.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? Portal { get; set; }

    public SectionMeta? Registration { get; set; }

    public SectionMeta? Login { get; set; }

    public SectionMeta? Sso { get; set; }

    public SectionMeta? ProfilePage { get; set; }

    public SectionMeta? RegistrationFields { get; set; }
}

/// <summary>The customer portal settings, as the facade or a single section sends them.</summary>
public sealed class CustomerPortal : Dto
{
    protected override string? EnvelopeKey => "customer_portal";

    public Portal? Portal { get; set; }

    public Registration? Registration { get; set; }

    public Login? Login { get; set; }

    public Sso? Sso { get; set; }

    public ProfilePage? ProfilePage { get; set; }

    public RegistrationFields? RegistrationFields { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
