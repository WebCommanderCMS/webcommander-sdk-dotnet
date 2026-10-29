using WebCommander.Errors;

namespace WebCommander.Client;

/// <summary>A resource with a client pre-applied: the base of every typed resource group.</summary>
public abstract class BoundResource
{
    protected WebCommanderClient Client { get; }

    protected BoundResource(WebCommanderClient client) => Client = client;

    private protected static void RequirePayload(object payload)
    {
        if (payload is null)
            throw new SdkUsageError("payload is required: this endpoint takes a body, and null "
                + "would send the request with no body at all");
    }
}
