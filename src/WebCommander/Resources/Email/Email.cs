using WebCommander.Client;
using WebCommander.Resources.Email.EmailDto;

namespace WebCommander.Resources.Email;

/// <summary>Email - <c>/admin/email/send</c> - as a typed resource bound to one client.</summary>
public sealed class Email : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Email</c>.</summary>
    public Email(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Send an email - <c>POST /admin/email/send</c>, answered <c>200</c> once every message is sent. A
    /// bare body: <c>recipients</c> and <c>subject</c>, with <c>htmlContent</c> or a template <c>identifier</c> and
    /// its <c>data</c>. <paramref name="pluginUuid"/> is sent as the header a template send needs and switches the
    /// call to plugin auth, so an admin token is <c>401</c>. Past 500 addresses a day per sender the store answers
    /// <c>429</c> until 00:00 UTC; a body over 1048576 bytes is <c>413</c>. A send the mail server fails is
    /// <c>502</c>, <c>status</c> <c>failed</c>, each address in <c>rejected</c>.</summary>
    public Task<ApiResponse> SendEmailAsync(
        object payload, string? pluginUuid = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPostRequestAsync(EmailUrls.EmailSend,
            new RequestOptions
            {
                JsonBody = payload,
                Headers = pluginUuid is null
                    ? null
                    : new Dictionary<string, string> { [EmailUrls.PluginUuidHeader] = pluginUuid },
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(EmailSendResult),
            }, cancellationToken);
    }
}
