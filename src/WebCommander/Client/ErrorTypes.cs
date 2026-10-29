namespace WebCommander.Client;

/// <summary>The status-to-classification table, public contract.</summary>
public static class ErrorTypes
{
    /// <summary>Map an HTTP status to a classification, told whether a 2xx body denied success.</summary>
    public static ErrorType Classify(int statusCode, bool bodyDeniesSuccess = false)
    {
        if (statusCode is >= 200 and < 300)
            return bodyDeniesSuccess ? ErrorType.Unknown : ErrorType.None;
        if (statusCode is >= 300 and < 400)
            return ErrorType.Redirect;
        return statusCode switch
        {
            400 or 422 => ErrorType.Validation,
            401 => ErrorType.Authentication,
            403 => ErrorType.Authorization,
            404 => ErrorType.NotFound,
            405 => ErrorType.MethodNotAllowed,
            409 => ErrorType.Conflict,
            429 => ErrorType.RateLimit,
            _ => statusCode is >= 500 and < 600 ? ErrorType.Server : ErrorType.Unknown,
        };
    }
}
