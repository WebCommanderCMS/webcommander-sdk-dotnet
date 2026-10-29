namespace WebCommander.Errors;

/// <summary>A name that does not exist on a client or a bound resource.</summary>
public class SdkAttributeError : SdkUsageError
{
    public SdkAttributeError(string message, Exception? inner = null)
        : base(message, inner) { }
}
