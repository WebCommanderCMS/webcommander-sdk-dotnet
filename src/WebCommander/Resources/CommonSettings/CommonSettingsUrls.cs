namespace WebCommander.Resources.CommonSettings;

/// <summary>Path templates for the common settings endpoints.</summary>
public static class CommonSettingsUrls
{
    /// <summary>Not under <c>/admin</c>: the store-wide settings reader and writer.</summary>
    public const string Settings = "/api/{api_version}/settings";

    /// <summary>Not under <c>/admin</c>: the customer registration form.</summary>
    public const string CreateCustomerFields = "/api/{api_version}/create-customer-fields";
}
