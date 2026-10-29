using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using WebCommander.Errors;

namespace WebCommander.Auth;

/// <summary>Token storage in a JSON file, safe across processes: a sidecar lock and atomic writes.</summary>
public sealed class FileTokenStore : ITokenStore
{
    /// <summary>The default cache filename, named for this SDK in a shared per-user directory.</summary>
    public const string DefaultTokenCacheFilename = "wc_dotnet_token.json";

    /// <summary>How long an acquisition waits for another process by default: five minutes.</summary>
    public const double DefaultLockWaitSeconds = 300;

    private const string LockSuffix = ".lock";
    private const int LockPollMillis = 50;
    private const string DefaultNamespace = "default";
    private const int ReplaceRetryAttempts = 5;
    private const int ReplaceRetryInitialMillis = 10;

    // Never removed: a store built later on the same path must find the same record.
    private static readonly Dictionary<string, SharedFileLock> LocksByPath = new(StringComparer.Ordinal);

    private readonly string _path;
    private readonly string _lockPath;
    private readonly string _namespace;
    private readonly int _lockWaitMillis;
    private readonly long _maxBytes;
    private readonly SharedFileLock _shared;

    /// <summary>Open a store over a file path, keyed by a namespace such as a credential fingerprint.</summary>
    public FileTokenStore(
        string filePath,
        string? @namespace = null,
        double lockWaitSeconds = DefaultLockWaitSeconds,
        long maxBytes = Config.SdkConfig.DefaultMaxTokenCacheBytes)
    {
        _path = filePath;
        _lockPath = filePath + LockSuffix;
        _namespace = string.IsNullOrEmpty(@namespace) ? DefaultNamespace : @namespace;
        _lockWaitMillis = (int)Math.Round(Math.Max(0, lockWaitSeconds) * 1000);
        _maxBytes = Math.Max(1, maxBytes);
        _shared = SharedLockFor(_lockPath);
        var parent = Path.GetDirectoryName(Path.GetFullPath(_path));
        if (!string.IsNullOrEmpty(parent) && !Directory.Exists(parent))
        {
            try
            {
                Directory.CreateDirectory(parent);
                // Narrowed only because this SDK created it.
                RestrictToOwner(parent, isDirectory: true);
            }
            catch (Exception error) when (error is IOException or UnauthorizedAccessException)
            {
                throw new SdkConfigError(Redaction.RedactMessageText($"tokenCachePath: the directory for {_path} could not "
                    + $"be created ({error.Message}); name a path whose parent exists, or keep the token "
                    + "in memory"), error);
            }
        }
    }

    private static SharedFileLock SharedLockFor(string lockPath)
    {
        var key = Path.GetFullPath(lockPath);
        if (OperatingSystem.IsWindows())
            key = key.ToLowerInvariant();
        lock (LocksByPath)
        {
            if (!LocksByPath.TryGetValue(key, out var shared))
            {
                shared = new SharedFileLock();
                LocksByPath[key] = shared;
            }
            return shared;
        }
    }

    /// <summary>A 16-hex-character fingerprint that identifies credentials without carrying them.</summary>
    public static string BuildCredentialsNamespace(
        string? baseUrl, string? clientId, string? clientSecret = "",
        string? authString = "", string? grantType = "")
    {
        var material = new StringBuilder();
        foreach (var value in new[] { "v3", baseUrl, clientId, clientSecret, authString, grantType })
        {
            var text = value ?? "";
            // Code points, not text.Length: UTF-16 units would tie the fingerprint to the encoding.
            var codePoints = text.EnumerateRunes().Count();
            material.Append(codePoints).Append(':').Append(text);
        }
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material.ToString()));
        var hex = new StringBuilder(hash.Length * 2);
        foreach (var byteValue in hash)
            hex.Append(byteValue.ToString("x2", CultureInfo.InvariantCulture));
        return hex.ToString(0, 16);
    }

    /// <summary>The conventional per-user cache path for this platform, keeping tokens out of the cwd.</summary>
    public static string BuildDefaultTokenCachePath(string filename = DefaultTokenCacheFilename)
    {
        string root;
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (OperatingSystem.IsWindows())
        {
            root = Environment.GetEnvironmentVariable("LOCALAPPDATA") ?? home;
        }
        else if (OperatingSystem.IsMacOS())
        {
            root = Path.Combine(home, "Library", "Caches");
        }
        else
        {
            root = Environment.GetEnvironmentVariable("XDG_CACHE_HOME") ?? Path.Combine(home, ".cache");
        }
        var directory = Path.Combine(root, "webcommander");
        // Fully qualified, not merely rooted: "C:webcommander" resolves against the drive's cwd.
        if (!Path.IsPathFullyQualified(directory))
            directory = Path.Combine(Path.GetTempPath(), "webcommander");
        if (!Path.IsPathFullyQualified(directory))
            throw new SdkConfigError(Redaction.RedactMessageText("neither the platform cache directory nor the system "
                + "temp directory resolved to an absolute path (got '" + directory + "'); pass a "
                + "path, or keep the token in memory"));
        try
        {
            Directory.CreateDirectory(directory);
            RestrictToOwner(directory, isDirectory: true);
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            throw new SdkConfigError(Redaction.RedactMessageText("the default token cache directory could not be created ("
                + error.Message + "); pass a path, or keep the token in memory"), error);
        }
        return Path.Combine(directory, filename);
    }

    public async Task<IReadOnlyDictionary<string, object?>?> ReadTokenRecordAsync(
        CancellationToken cancellationToken = default)
    {
        // The in-process gate only: writes are atomic, so a reader sees a whole old or new file.
        await _shared.Gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            return ReadNamespaceRecord();
        }
        finally
        {
            _shared.Gate.Release();
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

    /// <inheritdoc/>
    public async Task<ITokenRefreshScope> AcquireTokenRefreshLockAsync(
        CancellationToken cancellationToken = default)
    {
        await _shared.Gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            _shared.Handle = await TakeTheFileLockAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            _shared.Gate.Release();
            throw;
        }
        _shared.Depth = 1;
        return new Scope(this, _shared);
    }

    // FileShare, not FileStream.Lock: the byte-range API throws on macOS and FreeBSD.
    private async Task<FileStream> TakeTheFileLockAsync(CancellationToken cancellationToken)
    {
        var waited = Stopwatch.StartNew();
        var remadeTheParent = false;
        while (true)
        {
            try
            {
                return new FileStream(_lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            }
            catch (DirectoryNotFoundException error)
            {
                if (remadeTheParent)
                    throw UnopenableSidecar(error);
                remadeTheParent = true;
                if (RemakeTheParentDirectory() is { } reason)
                    throw new SdkError(Redaction.RedactMessageText(
                        $"the directory holding the token lock file {_lockPath} is gone and could "
                        + $"not be remade ({reason}), so the token cache cannot be locked; every "
                        + "process sharing it would refresh at once"), innerException: error);
            }
            catch (IOException error) when (!MeansContention(error))
            {
                throw UnopenableSidecar(error);
            }
            catch (IOException error)
            {
                if (waited.ElapsedMilliseconds >= _lockWaitMillis)
                    throw new SdkError(Redaction.RedactMessageText(
                        $"another process has held the token cache lock on {_lockPath} for longer "
                        + $"than {(_lockWaitMillis / 1000).ToString(CultureInfo.InvariantCulture)}s; "
                        + "refusing to read or write the cache without it, since two processes "
                        + "refreshing at once overwrite one another"), innerException: error);
                await Task.Delay(LockPollMillis, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception error) when (error is UnauthorizedAccessException
                                              or ArgumentException
                                              or NotSupportedException)
            {
                throw UnopenableSidecar(error);
            }
        }
    }

    private const int SharingViolation = 32;
    private const int LockViolation = 33;

    private static bool MeansContention(IOException error)
    {
        if (error is PathTooLongException or DriveNotFoundException or FileNotFoundException)
            return false;
        if (!OperatingSystem.IsWindows())
            return true;
        return (error.HResult & 0xFFFF) is SharingViolation or LockViolation;
    }

    private SdkError UnopenableSidecar(Exception error) => new(Redaction.RedactMessageText(
        $"the token cache lock file {_lockPath} could not be opened ({error.Message}), so the "
        + "cache cannot be locked; name a tokenCachePath this process can write, or keep the "
        + "token in memory with tokenCachePath: null"), innerException: error);

    private string? RemakeTheParentDirectory()
    {
        var parent = Path.GetDirectoryName(Path.GetFullPath(_lockPath));
        if (string.IsNullOrEmpty(parent))
            return "the lock file path names no directory";
        try
        {
            Directory.CreateDirectory(parent);
            return null;
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            return error.Message;
        }
    }

    private static void Leave(SharedFileLock shared)
    {
        if (shared.Depth > 1)
        {
            shared.Depth--;
            return;
        }
        shared.Depth = 0;
        var handle = shared.Handle;
        shared.Handle = null;
        handle?.Dispose();
        shared.Gate.Release();
    }

    private IReadOnlyDictionary<string, object?>? ReadNamespaceRecord()
    {
        if (ReadTheCache().Entries.TryGetValue(_namespace, out var entry)
            && entry is IReadOnlyDictionary<string, object?> record)
        {
            return new Dictionary<string, object?>(record);
        }
        return null;
    }

    // An unreadable cache is not an empty one: writing over it would drop every other namespace.
    private void WriteNamespaceRecord(IReadOnlyDictionary<string, object?> record)
    {
        var reading = ReadTheCache();
        if (reading.State is CacheState.Corrupt or CacheState.Unreadable)
            throw new SdkError(Redaction.RedactMessageText(
                $"the token cache at {_path} is {Describe(reading.State)}, so this entry was not "
                + "written: replacing the file would discard every other credential namespace in "
                + "it. Clearing the token removes the file and lets the next call rebuild it"),
                innerException: reading.Cause);
        var stored = reading.Entries;
        stored[_namespace] = new Dictionary<string, object?>(record);
        WriteAllNamespaces(stored);
    }

    private void ClearNamespaceRecord()
    {
        var reading = ReadTheCache();
        if (reading.State is CacheState.Corrupt or CacheState.Unreadable)
        {
            DeleteTheTokenFile();
            return;
        }
        var stored = reading.Entries;
        if (!stored.Remove(_namespace))
            return;
        if (stored.Count > 0)
            WriteAllNamespaces(stored);
        else
            DeleteTheTokenFile();
    }

    // Keep the sidecar: deleting it splits the lock.
    private void DeleteTheTokenFile()
    {
        try
        {
            File.Delete(_path);
        }
        catch (DirectoryNotFoundException)
        {
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            throw new SdkError(Redaction.RedactMessageText(
                "the token cache entry was removed from memory but the file holding it could not "
                + $"be deleted: {failure.Message}. A reusable token may still be on disk at "
                + $"{_path}, so treat this as a failed logout rather than a completed one"),
                innerException: failure);
        }
    }

    private CacheReading ReadTheCache()
    {
        string content;
        try
        {
            if (!File.Exists(_path))
                return new CacheReading(CacheState.Absent);
            using var handle = new FileStream(
                _path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var room = (int)Math.Min(_maxBytes + 1, Array.MaxLength);
            var buffer = new byte[room];
            var filled = 0;
            while (filled < room)
            {
                var read = handle.Read(buffer, filled, room - filled);
                if (read <= 0)
                    break;
                filled += read;
            }
            if (filled > _maxBytes)
            {
                return new CacheReading(CacheState.Unreadable, cause: new IOException(
                    $"the token cache at {_path} is larger than the "
                    + _maxBytes.ToString(CultureInfo.InvariantCulture)
                    + "-byte maxTokenCacheBytes ceiling, so it was not read. Clear the token to "
                    + "rebuild the file, or raise maxTokenCacheBytes"));
            }
            content = Encoding.UTF8.GetString(buffer, 0, filled);
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException
                                          or NotSupportedException)
        {
            return new CacheReading(CacheState.Unreadable, cause: error);
        }
        if (content.Trim().Length == 0)
            return new CacheReading(CacheState.Empty);
        try
        {
            using var document = JsonDocument.Parse(content);
            return Redaction.Materialise(document.RootElement) is Dictionary<string, object?> map
                ? new CacheReading(CacheState.Entries, entries: map)
                : new CacheReading(CacheState.Corrupt);
        }
        catch (JsonException error)
        {
            return new CacheReading(CacheState.Corrupt, cause: error);
        }
    }

    private static string Describe(CacheState state) => state switch
    {
        CacheState.Absent => "not there",
        CacheState.Empty => "empty",
        CacheState.Entries => "readable",
        CacheState.Corrupt => "present but not a JSON object",
        _ => "present but could not be read",
    };

    private void WriteAllNamespaces(Dictionary<string, object?> stored)
    {
        var directory = Path.GetDirectoryName(Path.GetFullPath(_path));
        if (string.IsNullOrEmpty(directory))
            directory = ".";
        Directory.CreateDirectory(directory);
        var temporary = Path.Combine(directory, Path.GetFileName(_path) + "." + Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            // BOM-less: Encoding.UTF8 writes a BOM, which a strict JSON reader rejects.
            File.WriteAllBytes(temporary, JsonSerializer.SerializeToUtf8Bytes(stored));
            // Narrowed before the move too: a same-volume move keeps the temporary's permissions.
            RestrictToOwner(temporary, isDirectory: false);
            ReplaceTheTokenFile(temporary);
            RestrictToOwner(_path, isDirectory: false);
        }
        catch (Exception)
        {
            TryDelete(temporary);
            throw;
        }
    }

    private void ReplaceTheTokenFile(string temporary)
    {
        var wait = ReplaceRetryInitialMillis;
        for (var attempt = 0; ; attempt++)
        {
            try
            {
                File.Move(temporary, _path, overwrite: true);
                return;
            }
            catch (Exception error) when (error is IOException or UnauthorizedAccessException)
            {
                if (!OperatingSystem.IsWindows() || attempt == ReplaceRetryAttempts - 1)
                    throw;
                Thread.Sleep(wait);
                wait *= 2;
            }
        }
    }

    private static void RestrictToOwner(string target, bool isDirectory)
    {
        if (OperatingSystem.IsWindows())
        {
            RestrictToOwnerOnWindows(target, isDirectory);
            return;
        }
        try
        {
            var mode = isDirectory
                ? UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute
                : UnixFileMode.UserRead | UnixFileMode.UserWrite;
            File.SetUnixFileMode(target, mode);
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException or PlatformNotSupportedException)
        {
        }
    }

    // Owner-only ACL with inheritance off, as chmod 700/600.
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    private static void RestrictToOwnerOnWindows(string target, bool isDirectory)
    {
        try
        {
            var owner = System.Security.Principal.WindowsIdentity.GetCurrent().User;
            if (owner is null)
                return;
            var inheritance = isDirectory
                ? System.Security.AccessControl.InheritanceFlags.ObjectInherit
                  | System.Security.AccessControl.InheritanceFlags.ContainerInherit
                : System.Security.AccessControl.InheritanceFlags.None;
            if (inheritance == System.Security.AccessControl.InheritanceFlags.None)
            {
                var file = new FileInfo(target);
                var security = file.GetAccessControl();
                Narrow(security, owner, inheritance);
                file.SetAccessControl(security);
            }
            else
            {
                var directory = new DirectoryInfo(target);
                var security = directory.GetAccessControl();
                Narrow(security, owner, inheritance);
                directory.SetAccessControl(security);
            }
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException
                                          or PlatformNotSupportedException
                                          or System.Security.SecurityException
                                          or InvalidOperationException)
        {
        }

        static void Narrow(
            System.Security.AccessControl.FileSystemSecurity security,
            System.Security.Principal.SecurityIdentifier owner,
            System.Security.AccessControl.InheritanceFlags inheritance)
        {
            security.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
            var existing = security.GetAccessRules(
                includeExplicit: true, includeInherited: true,
                targetType: typeof(System.Security.Principal.SecurityIdentifier));
            foreach (System.Security.AccessControl.FileSystemAccessRule rule in existing)
                security.RemoveAccessRuleSpecific(rule);
            security.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(
                owner,
                System.Security.AccessControl.FileSystemRights.FullControl,
                inheritance,
                System.Security.AccessControl.PropagationFlags.None,
                System.Security.AccessControl.AccessControlType.Allow));
        }
    }

    private static void TryDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
        }
    }

    private sealed class SharedFileLock
    {
        public SemaphoreSlim Gate { get; } = new(1, 1);

        public FileStream? Handle { get; set; }

        public int Depth { get; set; }
    }

    private enum CacheState
    {
        Absent,
        Empty,
        Entries,
        Corrupt,
        Unreadable,
    }

    private sealed class CacheReading
    {
        private readonly Dictionary<string, object?>? _entries;

        public CacheReading(
            CacheState state, Dictionary<string, object?>? entries = null, Exception? cause = null)
        {
            State = state;
            _entries = entries;
            Cause = cause;
        }

        public CacheState State { get; }

        public Exception? Cause { get; }

        public Dictionary<string, object?> Entries =>
            _entries is null ? new Dictionary<string, object?>() : new Dictionary<string, object?>(_entries);
    }

    private sealed class Scope : ITokenRefreshScope
    {
        private readonly FileTokenStore _store;
        private readonly SharedFileLock _shared;
        private bool _released;

        public Scope(FileTokenStore store, SharedFileLock shared)
        {
            _store = store;
            _shared = shared;
        }

        public Task<IReadOnlyDictionary<string, object?>?> ReadTokenRecordAsync(
            CancellationToken cancellationToken = default)
        {
            RefuseIfReleased("ReadTokenRecordAsync");
            return Task.FromResult(_store.ReadNamespaceRecord());
        }

        public Task WriteTokenRecordAsync(
            IReadOnlyDictionary<string, object?> record, CancellationToken cancellationToken = default)
        {
            RefuseIfReleased("WriteTokenRecordAsync");
            _store.WriteNamespaceRecord(record);
            return Task.CompletedTask;
        }

        public Task ClearTokenRecordAsync(CancellationToken cancellationToken = default)
        {
            RefuseIfReleased("ClearTokenRecordAsync");
            _store.ClearNamespaceRecord();
            return Task.CompletedTask;
        }

        public Task<ITokenRefreshScope> EnterNestedAsync(CancellationToken cancellationToken = default)
        {
            RefuseIfReleased("EnterNestedAsync");
            _shared.Depth++;
            return Task.FromResult<ITokenRefreshScope>(new Scope(_store, _shared));
        }

        private void RefuseIfReleased(string member)
        {
            if (_released)
                throw SdkUsageError.ReleasedTokenRefreshScope(member);
        }

        public ValueTask DisposeAsync()
        {
            if (!_released)
            {
                _released = true;
                Leave(_shared);
            }
            return default;
        }
    }
}
