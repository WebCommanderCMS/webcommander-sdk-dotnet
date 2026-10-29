namespace WebCommander.Errors;

/// <summary>Configuration is missing or invalid; raised at construction, listing every problem.</summary>
public class SdkConfigError : SdkError
{
    public SdkConfigError(string message, Exception? inner = null)
        : base(message, innerException: inner) { }
}
