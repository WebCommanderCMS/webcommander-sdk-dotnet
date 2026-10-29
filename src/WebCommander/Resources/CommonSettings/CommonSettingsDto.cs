namespace WebCommander.Resources.CommonSettings.CommonSettingsDto;

/// <summary>One entry of a settings group, for a write's lists; a read's entries have the same keys. Reads
/// answer <see cref="Value"/> as text, <c>***</c> for a masked one; a number written is stored as text.</summary>
public sealed class Setting : Dto
{
    public string? ConfigKey { get; set; }

    public string? Value { get; set; }
}

/// <summary>What the read and both writes answer: <see cref="Settings"/>, group name to a list of
/// <c>{"config_key", "value"}</c> maps. The <c>notifications</c> group alone is a name to <c>bool</c> map, so
/// the groups stay a plain map.</summary>
public sealed class CommonSettings : Dto
{
    public IReadOnlyDictionary<string, object?>? Settings { get; set; }
}

/// <summary>The list form both writes take: <see cref="Settings"/> maps a group name to a list of
/// <see cref="Setting"/> entries. A group may instead be <c>{config_key: value}</c> or
/// <c>{name: {"config_key", "value"}}</c>; send those as a map or a JSON string. Keyed by group name, so the
/// groups are not typed properties.</summary>
public sealed class CommonSettingsInput : Dto
{
    public IReadOnlyDictionary<string, object?>? Settings { get; set; }
}

/// <summary>The registration form's <c>fields_configs</c>: six keys for each of the store's 20 fields.
/// <c>_order</c> reads as text; <c>_active</c> and <c>_required</c> are booleans here, though the settings read
/// answers them as text.</summary>
public sealed class CustomerFieldConfigs : Dto
{
    public string? FirstNameOrder { get; set; }

    public string? FirstNameKey { get; set; }

    public string? FirstNameLabel { get; set; }

    public string? FirstNamePlaceholder { get; set; }

    public bool? FirstNameActive { get; set; }

    public bool? FirstNameRequired { get; set; }

    public string? LastNameOrder { get; set; }

    public string? LastNameKey { get; set; }

    public string? LastNameLabel { get; set; }

    public string? LastNamePlaceholder { get; set; }

    public bool? LastNameActive { get; set; }

    public bool? LastNameRequired { get; set; }

    public string? AddressLine1Order { get; set; }

    public string? AddressLine1Key { get; set; }

    public string? AddressLine1Label { get; set; }

    public string? AddressLine1Placeholder { get; set; }

    public bool? AddressLine1Active { get; set; }

    public bool? AddressLine1Required { get; set; }

    public string? AddressLine2Order { get; set; }

    public string? AddressLine2Key { get; set; }

    public string? AddressLine2Label { get; set; }

    public string? AddressLine2Placeholder { get; set; }

    public bool? AddressLine2Active { get; set; }

    public bool? AddressLine2Required { get; set; }

    public string? PostCodeOrder { get; set; }

    public string? PostCodeKey { get; set; }

    public string? PostCodeLabel { get; set; }

    public string? PostCodePlaceholder { get; set; }

    public bool? PostCodeActive { get; set; }

    public bool? PostCodeRequired { get; set; }

    public string? CityOrder { get; set; }

    public string? CityKey { get; set; }

    public string? CityLabel { get; set; }

    public string? CityPlaceholder { get; set; }

    public bool? CityActive { get; set; }

    public bool? CityRequired { get; set; }

    public string? PhoneOrder { get; set; }

    public string? PhoneKey { get; set; }

    public string? PhoneLabel { get; set; }

    public string? PhonePlaceholder { get; set; }

    public bool? PhoneActive { get; set; }

    public bool? PhoneRequired { get; set; }

    public string? MobileOrder { get; set; }

    public string? MobileKey { get; set; }

    public string? MobileLabel { get; set; }

    public string? MobilePlaceholder { get; set; }

    public bool? MobileActive { get; set; }

    public bool? MobileRequired { get; set; }

    public string? FaxOrder { get; set; }

    public string? FaxKey { get; set; }

    public string? FaxLabel { get; set; }

    public string? FaxPlaceholder { get; set; }

    public bool? FaxActive { get; set; }

    public bool? FaxRequired { get; set; }

    public string? EmailOrder { get; set; }

    public string? EmailKey { get; set; }

    public string? EmailLabel { get; set; }

    public string? EmailPlaceholder { get; set; }

    public bool? EmailActive { get; set; }

    public bool? EmailRequired { get; set; }

    public string? ConfirmEmailOrder { get; set; }

    public string? ConfirmEmailKey { get; set; }

    public string? ConfirmEmailLabel { get; set; }

    public string? ConfirmEmailPlaceholder { get; set; }

    public bool? ConfirmEmailActive { get; set; }

    public bool? ConfirmEmailRequired { get; set; }

    public string? PasswordOrder { get; set; }

    public string? PasswordKey { get; set; }

    public string? PasswordLabel { get; set; }

    public string? PasswordPlaceholder { get; set; }

    public bool? PasswordActive { get; set; }

    public bool? PasswordRequired { get; set; }

    public string? RetypePasswordOrder { get; set; }

    public string? RetypePasswordKey { get; set; }

    public string? RetypePasswordLabel { get; set; }

    public string? RetypePasswordPlaceholder { get; set; }

    public bool? RetypePasswordActive { get; set; }

    public bool? RetypePasswordRequired { get; set; }

    public string? CountryOrder { get; set; }

    public string? CountryKey { get; set; }

    public string? CountryLabel { get; set; }

    public string? CountryPlaceholder { get; set; }

    public bool? CountryActive { get; set; }

    public bool? CountryRequired { get; set; }

    public string? CustomerTypeOrder { get; set; }

    public string? CustomerTypeKey { get; set; }

    public string? CustomerTypeLabel { get; set; }

    public string? CustomerTypePlaceholder { get; set; }

    public bool? CustomerTypeActive { get; set; }

    public bool? CustomerTypeRequired { get; set; }

    public string? SexOrder { get; set; }

    public string? SexKey { get; set; }

    public string? SexLabel { get; set; }

    public string? SexPlaceholder { get; set; }

    public bool? SexActive { get; set; }

    public bool? SexRequired { get; set; }

    public string? AbnOrder { get; set; }

    public string? AbnKey { get; set; }

    public string? AbnLabel { get; set; }

    public string? AbnPlaceholder { get; set; }

    public bool? AbnActive { get; set; }

    public bool? AbnRequired { get; set; }

    public string? AbnBranchOrder { get; set; }

    public string? AbnBranchKey { get; set; }

    public string? AbnBranchLabel { get; set; }

    public string? AbnBranchPlaceholder { get; set; }

    public bool? AbnBranchActive { get; set; }

    public bool? AbnBranchRequired { get; set; }

    public string? CompanyNameOrder { get; set; }

    public string? CompanyNameKey { get; set; }

    public string? CompanyNameLabel { get; set; }

    public string? CompanyNamePlaceholder { get; set; }

    public bool? CompanyNameActive { get; set; }

    public bool? CompanyNameRequired { get; set; }

    public string? DateOfBirthOrder { get; set; }

    public string? DateOfBirthKey { get; set; }

    public string? DateOfBirthLabel { get; set; }

    public string? DateOfBirthPlaceholder { get; set; }

    public bool? DateOfBirthActive { get; set; }

    public bool? DateOfBirthRequired { get; set; }
}

/// <summary>The registration form: <see cref="Fields"/> lists the active fields' keys in their order, and
/// <see cref="FieldsConfigs"/> configures every field, inactive ones included.</summary>
public sealed class CustomerFields : Dto
{
    public new IReadOnlyList<string?>? Fields { get; set; }

    public CustomerFieldConfigs? FieldsConfigs { get; set; }
}
