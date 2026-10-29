namespace WebCommander.Errors;

/// <summary>A caller mistake the SDK detects before any network I/O.</summary>
public class SdkUsageError : SdkError
{
    public SdkUsageError(string message, Exception? inner = null)
        : base(message, innerException: inner) { }

    internal static SdkUsageError ReleasedTokenRefreshScope(string member) => new(
        member + " was called on a token refresh scope that has already been "
        + "released, so the lock it operated under is no longer held - scope it with "
        + "`await using` around the whole read-decide-write, and acquire a new scope "
        + "for later work");
}
