namespace WebCommander.Client;

/// <summary>Classification of a failed call, derived from the HTTP status.</summary>
public enum ErrorType
{
    /// <summary>2xx whose body did not deny success - no error.</summary>
    None,
    /// <summary>3xx - an unfollowed redirect.</summary>
    Redirect,
    /// <summary>400/422 - the request was malformed or failed validation.</summary>
    Validation,
    /// <summary>401 - credentials were missing, rejected or expired.</summary>
    Authentication,
    /// <summary>403 - authenticated but not permitted.</summary>
    Authorization,
    /// <summary>404 - no such record.</summary>
    NotFound,
    /// <summary>405 - wrong method for the path.</summary>
    MethodNotAllowed,
    /// <summary>409 - a conflict with existing data.</summary>
    Conflict,
    /// <summary>429 - rate limited.</summary>
    RateLimit,
    /// <summary>5xx - the server failed.</summary>
    Server,
    /// <summary>A transport-level failure with no HTTP status.</summary>
    Network,
    Timeout,
    /// <summary>Any other non-2xx status, and a 2xx whose body denied success.</summary>
    Unknown,
}
