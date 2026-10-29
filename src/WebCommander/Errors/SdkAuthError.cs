namespace WebCommander.Errors;

/// <summary>Authentication failed: a rejected token grant, or a 401 or 403 under <c>raiseOnError</c>.</summary>
public class SdkAuthError : SdkError
{
    public SdkAuthError(string message) : base(message) { }

    public SdkAuthError(
        string message,
        int? statusCode = null,
        Client.ErrorType? errorType = null,
        object? rawResponse = null,
        IReadOnlyDictionary<string, string>? responseHeaders = null,
        Client.RequestInfo? requestInfo = null)
        : base(message, statusCode, errorType, rawResponse, responseHeaders, requestInfo) { }
}
