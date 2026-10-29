namespace WebCommander.Auth;

/// <summary>Token-store operations under a held refresh lock; every member is refused once disposed.</summary>
public interface ITokenRefreshScope : IAsyncDisposable
{
    /// <exception cref="Errors.SdkUsageError">this scope has already been disposed.</exception>
    Task<IReadOnlyDictionary<string, object?>?> ReadTokenRecordAsync(CancellationToken cancellationToken = default);

    /// <exception cref="Errors.SdkUsageError">this scope has already been disposed.</exception>
    Task WriteTokenRecordAsync(IReadOnlyDictionary<string, object?> record, CancellationToken cancellationToken = default);

    /// <exception cref="Errors.SdkUsageError">this scope has already been disposed.</exception>
    Task ClearTokenRecordAsync(CancellationToken cancellationToken = default);

    /// <summary>Enter the held lock again; it is released when the last outstanding scope is disposed.</summary>
    Task<ITokenRefreshScope> EnterNestedAsync(CancellationToken cancellationToken = default);
}
