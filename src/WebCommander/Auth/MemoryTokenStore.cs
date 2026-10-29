namespace WebCommander.Auth;

/// <summary>In-process token storage, isolated per instance and detached both ways; nothing on disk.</summary>
public sealed class MemoryTokenStore : ITokenStore
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private IReadOnlyDictionary<string, object?>? _record;

    public async Task<IReadOnlyDictionary<string, object?>?> ReadTokenRecordAsync(
        CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            return Redaction.CopyBuffersInMap(_record);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task WriteTokenRecordAsync(
        IReadOnlyDictionary<string, object?> record, CancellationToken cancellationToken = default)
    {
        await using var scope = await AcquireTokenRefreshLockAsync(cancellationToken).ConfigureAwait(false);
        await scope.WriteTokenRecordAsync(record, cancellationToken).ConfigureAwait(false);
    }

    public async Task ClearTokenRecordAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = await AcquireTokenRefreshLockAsync(cancellationToken).ConfigureAwait(false);
        await scope.ClearTokenRecordAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<ITokenRefreshScope> AcquireTokenRefreshLockAsync(
        CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        return new Scope(this);
    }

    private sealed class Scope : ITokenRefreshScope
    {
        private readonly MemoryTokenStore _store;
        private readonly Depth _depth;
        private bool _released;

        public Scope(MemoryTokenStore store) : this(store, new Depth()) { }

        private Scope(MemoryTokenStore store, Depth depth)
        {
            _store = store;
            _depth = depth;
            depth.Outstanding++;
        }

        public Task<IReadOnlyDictionary<string, object?>?> ReadTokenRecordAsync(
            CancellationToken cancellationToken = default)
        {
            RefuseIfReleased("ReadTokenRecordAsync");
            return Task.FromResult(Redaction.CopyBuffersInMap(_store._record));
        }

        public Task WriteTokenRecordAsync(
            IReadOnlyDictionary<string, object?> record, CancellationToken cancellationToken = default)
        {
            RefuseIfReleased("WriteTokenRecordAsync");
            _store._record = Redaction.DetachMap(record);
            return Task.CompletedTask;
        }

        public Task ClearTokenRecordAsync(CancellationToken cancellationToken = default)
        {
            RefuseIfReleased("ClearTokenRecordAsync");
            _store._record = null;
            return Task.CompletedTask;
        }

        public Task<ITokenRefreshScope> EnterNestedAsync(CancellationToken cancellationToken = default)
        {
            RefuseIfReleased("EnterNestedAsync");
            return Task.FromResult<ITokenRefreshScope>(new Scope(_store, _depth));
        }

        private void RefuseIfReleased(string member)
        {
            if (_released)
                throw Errors.SdkUsageError.ReleasedTokenRefreshScope(member);
        }

        public ValueTask DisposeAsync()
        {
            if (!_released)
            {
                _released = true;
                if (--_depth.Outstanding == 0)
                    _store._gate.Release();
            }
            return default;
        }

        private sealed class Depth
        {
            public int Outstanding { get; set; }
        }
    }
}
