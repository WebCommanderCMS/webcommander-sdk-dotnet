namespace WebCommander.Resources.Email;

/// <summary>Path template and the plugin header for the email endpoint.</summary>
public static class EmailUrls
{
    public const string EmailSend = "/api/{api_version}/admin/email/send";

    /// <summary>Names the plugin that owns the template <c>identifier</c>; a <c>plugin_uuid</c> body field is ignored.</summary>
    public const string PluginUuidHeader = "plugin_uuid";
}
