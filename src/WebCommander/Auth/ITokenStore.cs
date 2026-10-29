namespace WebCommander.Auth;

/// <summary>Where an access token lives between calls: persistence plus refresh coordination.</summary>
public interface ITokenStore
{
    /// <summary>The stored token, or <c>null</c> when absent or unreadable.</summary>
    Task<IReadOnlyDictionary<string, object?>?> ReadTokenRecordAsync(CancellationToken cancellationToken = default);

    /// <summary>Persist a token, replacing whatever was there; takes and releases the refresh lock.</summary>
    Task WriteTokenRecordAsync(IReadOnlyDictionary<string, object?> record, CancellationToken cancellationToken = default);

    /// <summary>Discard the stored token; absence is success, and a store still holding it throws.</summary>
    Task ClearTokenRecordAsync(CancellationToken cancellationToken = default);

    /// <summary>Hold the refresh lock for the scope's lifetime; not reentrant, so nest through the scope.</summary>
    Task<ITokenRefreshScope> AcquireTokenRefreshLockAsync(CancellationToken cancellationToken = default);
}
