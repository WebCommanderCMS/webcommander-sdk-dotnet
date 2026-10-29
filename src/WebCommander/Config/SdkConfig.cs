using System.Globalization;
using System.Text.RegularExpressions;

using WebCommander.Errors;
using WebCommander.Http;

namespace WebCommander.Config;

/// <summary>Immutable SDK configuration, validated at construction with every problem reported at once.</summary>
public sealed partial class SdkConfig
{
    public static readonly string SdkVersion = ReadInstalledVersion();

    public static readonly string DefaultUserAgent = "webcommander/" + SdkVersion;

    private const string FallbackVersion = "0.0.0+source";

    private static string ReadInstalledVersion()
    {
        var informational = typeof(SdkConfig).Assembly
            .GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), false)
            is System.Reflection.AssemblyInformationalVersionAttribute[] { Length: > 0 } attributes
            ? attributes[0].InformationalVersion
            : null;
        if (string.IsNullOrWhiteSpace(informational))
            return FallbackVersion;
        var plus = informational.IndexOf('+');
        return plus > 0 ? informational[..plus] : informational;
    }

    public const string DefaultApiVersion = "v4";

    public const double DefaultTimeoutSeconds = 30.0;

    public const double DefaultConnectTimeoutSeconds = 10.0;

    /// <summary>Total attempts including the first, so 1 disables retrying.</summary>
    public const int DefaultMaxRetries = 1;

    public const double DefaultRetryBackoffSeconds = 0.5;

    public const double DefaultRetryBackoffMaxSeconds = 20.0;

    /// <summary>How much of one response body the transport buffers: 64 MiB.</summary>
    public const long DefaultMaxResponseBytes = 64L * 1024 * 1024;

    /// <summary>How much of one upload stream the SDK copies into its own storage: 32 MiB.</summary>
    public const long DefaultMaxRequestSpoolBytes = 32L * 1024 * 1024;

    /// <summary>How large the finished request body may be, framing included: 64 MiB.</summary>
    public const long DefaultMaxRequestBytes = 64L * 1024 * 1024;

    /// <summary>How much caller payload one request may carry across every part: 64 MiB.</summary>
    public const long DefaultMaxRequestSourceBytes = 64L * 1024 * 1024;

    /// <summary>How many bytes of multipart framing one request may carry: 1 MiB.</summary>
    public const long DefaultMaxMultipartOverheadBytes = 1L * 1024 * 1024;

    /// <summary>How much of the token cache file is read: 1 MiB.</summary>
    public const long DefaultMaxTokenCacheBytes = 1L * 1024 * 1024;

    /// <summary>How much of a custom CA bundle is imported: 8 MiB.</summary>
    public const long DefaultMaxCaBundleBytes = 8L * 1024 * 1024;

    public const string DefaultLogLevel = "WARNING";

    /// <summary>Pretty (multi-line) console print format.</summary>
    public const string PrintFormatPretty = "pretty";

    /// <summary>Compact (single-line) console print format.</summary>
    public const string PrintFormatCompact = "compact";

    /// <summary>Put the decoded map/list/bytes on <c>ApiResponse.Data</c>; the built-in default.</summary>
    public const string ResponseFormatMap = "map";

    /// <summary>Put the server's own body text on <c>ApiResponse.Data</c>.</summary>
    public const string ResponseFormatJson = "json";

    /// <summary>Put the typed view on <c>ApiResponse.Data</c>.</summary>
    public const string ResponseFormatDto = "dto";

    /// <summary>The environment variable <see cref="ResponseFormat"/> falls back to, the lowest tier.</summary>
    public const string ResponseFormatVariable = "WC_RESPONSE_FORMAT";

    // Severity order, not sorted: the refusal below quotes this ranking.
    internal static readonly string[] LogLevelsBySeverity =
        { "DEBUG", "INFO", "WARNING", "ERROR", "CRITICAL" };

    private static readonly IReadOnlySet<string> ValidLogLevels = new HashSet<string>(LogLevelsBySeverity);
    private static readonly IReadOnlySet<string> ValidPrintFormats =
        new HashSet<string> { PrintFormatPretty, PrintFormatCompact };

    private static readonly System.Collections.Immutable.ImmutableArray<string> ResponseFormats =
        System.Collections.Immutable.ImmutableArray.Create(
            ResponseFormatMap, ResponseFormatJson, ResponseFormatDto);

    private static readonly IReadOnlySet<string> ValidResponseFormats = new HashSet<string>(ResponseFormats);

    [GeneratedRegex("^v?[1-9][0-9]*$", RegexOptions.IgnoreCase)]
    private static partial Regex ApiVersionPattern();

    public SdkConfig(
        AuthConfig auth,
        string apiVersion = DefaultApiVersion,
        double timeoutSeconds = DefaultTimeoutSeconds,
        double connectTimeoutSeconds = DefaultConnectTimeoutSeconds,
        int maxRetries = DefaultMaxRetries,
        long maxResponseBytes = DefaultMaxResponseBytes,
        IReadOnlySet<int>? retryableHttpCodes = null,
        IReadOnlySet<int>? unsafeRetryableHttpCodes = null,
        double retryBackoffSeconds = DefaultRetryBackoffSeconds,
        double retryBackoffMaxSeconds = DefaultRetryBackoffMaxSeconds,
        bool verifySsl = true,
        string? caBundle = null,
        string? proxyUrl = null,
        IReadOnlyDictionary<string, string>? defaultHeaders = null,
        bool raiseOnError = false,
        string? userAgent = null,
        string? logLevel = DefaultLogLevel,
        bool logBodies = false,
        string? logFile = null,
        bool printApiInfo = false,
        bool printRequestPayload = false,
        bool printResponseBody = false,
        bool printHeaders = false,
        bool printCurl = false,
        bool printOnlyOnError = false,
        string? printFormat = PrintFormatPretty,
        bool printColor = false,
        int printMaxChars = 0,
        string? responseFormat = null,
        long maxRequestSpoolBytes = DefaultMaxRequestSpoolBytes,
        long maxRequestBytes = DefaultMaxRequestBytes,
        long maxRequestSourceBytes = DefaultMaxRequestSourceBytes,
        long maxMultipartOverheadBytes = DefaultMaxMultipartOverheadBytes,
        long maxTokenCacheBytes = DefaultMaxTokenCacheBytes,
        long maxCaBundleBytes = DefaultMaxCaBundleBytes)
    {
        Auth = auth;
        ApiVersion = NormaliseApiVersion(apiVersion);
        TimeoutSeconds = timeoutSeconds;
        ConnectTimeoutSeconds = connectTimeoutSeconds;
        MaxRetries = maxRetries;
        MaxResponseBytes = maxResponseBytes;
        MaxRequestSpoolBytes = maxRequestSpoolBytes;
        MaxRequestBytes = maxRequestBytes;
        MaxRequestSourceBytes = maxRequestSourceBytes;
        MaxMultipartOverheadBytes = maxMultipartOverheadBytes;
        MaxTokenCacheBytes = maxTokenCacheBytes;
        MaxCaBundleBytes = maxCaBundleBytes;
        // Frozen: a downcast HashSet could make a POST retryable on a 500 and double-create.
        RetryableHttpCodes = System.Collections.Frozen.FrozenSet.ToFrozenSet(
            retryableHttpCodes ?? RetryPolicy.DefaultRetryableHttpCodes);
        UnsafeRetryableHttpCodes = System.Collections.Frozen.FrozenSet.ToFrozenSet(
            unsafeRetryableHttpCodes ?? RetryPolicy.DefaultUnsafeRetryableHttpCodes);
        RetryBackoffSeconds = retryBackoffSeconds;
        RetryBackoffMaxSeconds = retryBackoffMaxSeconds;
        VerifySsl = verifySsl;
        CaBundle = caBundle;
        ProxyUrl = proxyUrl;
        DefaultHeaders = System.Collections.Frozen.FrozenDictionary.ToFrozenDictionary(
            defaultHeaders ?? new Dictionary<string, string>());
        RaiseOnError = raiseOnError;
        UserAgent = userAgent ?? DefaultUserAgent;
        LogLevel = logLevel?.Trim().ToUpperInvariant();
        LogBodies = logBodies;
        LogFile = logFile;
        PrintApiInfo = printApiInfo;
        PrintRequestPayload = printRequestPayload;
        PrintResponseBody = printResponseBody;
        PrintHeaders = printHeaders;
        PrintCurl = printCurl;
        PrintOnlyOnError = printOnlyOnError;
        PrintFormat = printFormat?.Trim().ToLowerInvariant();
        PrintColor = printColor;
        PrintMaxChars = printMaxChars;
        _environmentResponseFormat = Normalise(Environment.GetEnvironmentVariable(ResponseFormatVariable));
        _suppliedResponseFormat = Normalise(responseFormat);
        ResponseFormat = _suppliedResponseFormat ?? _environmentResponseFormat ?? ResponseFormatMap;
        Validate();
    }

    /// <summary>The <c>WC_RESPONSE_FORMAT</c> value as read, normalised but unjudged.</summary>
    private readonly string? _environmentResponseFormat;

    /// <summary>The format this config was given, so WithOverrides can tell "not supplied" from map.</summary>
    private readonly string? _suppliedResponseFormat;

    /// <summary>Trim and case-fold a response-format value; blank and <c>null</c> read as not supplied.</summary>
    internal static string? Normalise(string? value)
    {
        var trimmed = value?.Trim().ToLowerInvariant();
        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }

    /// <summary>Whether a value is <c>map</c>, <c>json</c> or <c>dto</c>, in any casing.</summary>
    public static bool IsResponseFormat(string? value) =>
        Normalise(value) is { } normalised && ValidResponseFormats.Contains(normalised);

    /// <summary>The three response formats, in tier order; immutable.</summary>
    public static IReadOnlyList<string> ResponseFormatValues => ResponseFormats;

    public AuthConfig Auth { get; }

    public string? ApiVersion { get; }

    /// <summary>A hard ceiling on one attempt, in seconds: connect, send, headers and body read.</summary>
    public double TimeoutSeconds { get; }

    /// <summary>Connection-establishment timeout, in seconds; the shorter of the two ceilings wins.</summary>
    public double ConnectTimeoutSeconds { get; }

    /// <summary>Total attempts including the first.</summary>
    public int MaxRetries { get; }

    /// <summary>How much of one response body to buffer, in bytes; at least 1.</summary>
    public long MaxResponseBytes { get; }

    /// <summary>How much of one upload stream is copied into SDK-owned storage, in bytes; at least 0.</summary>
    public long MaxRequestSpoolBytes { get; }

    /// <summary>How large the finished request body may be, framing included, in bytes; at least 1.</summary>
    public long MaxRequestBytes { get; }

    /// <summary>How much caller payload one request may carry across every part, in bytes; at least 0.</summary>
    public long MaxRequestSourceBytes { get; }

    /// <summary>How many bytes of multipart framing one request may carry; at least 0.</summary>
    public long MaxMultipartOverheadBytes { get; }

    /// <summary>How much of the token cache file is read, in bytes; at least 1.</summary>
    public long MaxTokenCacheBytes { get; }

    /// <summary>How much of a custom CA bundle is imported, in bytes; at least 1.</summary>
    public long MaxCaBundleBytes { get; }

    public IReadOnlySet<int> RetryableHttpCodes { get; }

    /// <summary>Statuses a state-changing method may be replayed on; empty by default.</summary>
    public IReadOnlySet<int> UnsafeRetryableHttpCodes { get; }

    public static IReadOnlySet<int> DefaultRetryableHttpCodes => RetryPolicy.DefaultRetryableHttpCodes;

    public static IReadOnlySet<int> DefaultUnsafeRetryableHttpCodes => RetryPolicy.DefaultUnsafeRetryableHttpCodes;

    public double RetryBackoffSeconds { get; }

    public double RetryBackoffMaxSeconds { get; }

    public bool VerifySsl { get; }

    public string? CaBundle { get; }

    public string? ProxyUrl { get; }

    /// <summary>Default headers sent with every request, snapshotted at construction.</summary>
    public IReadOnlyDictionary<string, string> DefaultHeaders { get; }

    /// <summary>Whether a non-2xx response throws rather than returning the error body.</summary>
    public bool RaiseOnError { get; }

    public string UserAgent { get; }

    /// <summary>The SDK's own log level; ranked once, in the client.</summary>
    public string? LogLevel { get; }

    public bool LogBodies { get; }

    /// <summary>Also write log records to this path, appended in UTF-8, or <c>null</c> for none.</summary>
    public string? LogFile { get; }

    public bool PrintApiInfo { get; }

    /// <summary>Print the body sent, secrets redacted.</summary>
    public bool PrintRequestPayload { get; }

    /// <summary>Print the body received; a byte body is summarised, never dumped.</summary>
    public bool PrintResponseBody { get; }

    public bool PrintHeaders { get; }

    /// <summary>Print a copy-pasteable <c>curl</c> for the call, token redacted.</summary>
    public bool PrintCurl { get; }

    public bool PrintOnlyOnError { get; }

    /// <summary>Either <c>pretty</c> or <c>compact</c>.</summary>
    public string? PrintFormat { get; }

    public bool PrintColor { get; }

    /// <summary>Truncate printed values to this many characters; 0 = unlimited.</summary>
    public int PrintMaxChars { get; }

    /// <summary>The <c>ApiResponse.Data</c> form: the argument, else <c>WC_RESPONSE_FORMAT</c>, else map.</summary>
    public string ResponseFormat { get; }

    public string BaseUrl => Auth.BaseUrl;

    public string ClientName => Auth.ClientName;

    public string ClientId => Auth.ClientId;

    public string ClientSecret => Auth.ClientSecret;

    public string RedirectUri => Auth.RedirectUri;

    public string AuthString => Auth.AuthString;

    public string GrantType => Auth.GrantType;

    public string? TokenCachePath => Auth.TokenCachePath;

    /// <summary>Derive a new config through the validating constructor; null means "not supplied".</summary>
    public SdkConfig WithOverrides(
        AuthConfig? auth = null,
        string? apiVersion = null,
        double? timeoutSeconds = null,
        double? connectTimeoutSeconds = null,
        int? maxRetries = null,
        long? maxResponseBytes = null,
        IReadOnlySet<int>? retryableHttpCodes = null,
        IReadOnlySet<int>? unsafeRetryableHttpCodes = null,
        double? retryBackoffSeconds = null,
        double? retryBackoffMaxSeconds = null,
        bool? verifySsl = null,
        string? caBundle = null,
        string? proxyUrl = null,
        IReadOnlyDictionary<string, string>? defaultHeaders = null,
        bool? raiseOnError = null,
        string? userAgent = null,
        string? logLevel = null,
        bool? logBodies = null,
        string? logFile = null,
        bool? printApiInfo = null,
        bool? printRequestPayload = null,
        bool? printResponseBody = null,
        bool? printHeaders = null,
        bool? printCurl = null,
        bool? printOnlyOnError = null,
        string? printFormat = null,
        bool? printColor = null,
        int? printMaxChars = null,
        string? responseFormat = null,
        long? maxRequestSpoolBytes = null,
        long? maxRequestBytes = null,
        long? maxRequestSourceBytes = null,
        long? maxMultipartOverheadBytes = null,
        long? maxTokenCacheBytes = null,
        long? maxCaBundleBytes = null) =>
        new(
            auth ?? Auth,
            apiVersion ?? ApiVersion!,
            timeoutSeconds ?? TimeoutSeconds,
            connectTimeoutSeconds ?? ConnectTimeoutSeconds,
            maxRetries ?? MaxRetries,
            maxResponseBytes ?? MaxResponseBytes,
            retryableHttpCodes ?? RetryableHttpCodes,
            unsafeRetryableHttpCodes ?? UnsafeRetryableHttpCodes,
            retryBackoffSeconds ?? RetryBackoffSeconds,
            retryBackoffMaxSeconds ?? RetryBackoffMaxSeconds,
            verifySsl ?? VerifySsl,
            caBundle ?? CaBundle,
            proxyUrl ?? ProxyUrl,
            defaultHeaders ?? DefaultHeaders,
            raiseOnError ?? RaiseOnError,
            userAgent ?? UserAgent,
            logLevel ?? LogLevel,
            logBodies ?? LogBodies,
            logFile ?? LogFile,
            printApiInfo ?? PrintApiInfo,
            printRequestPayload ?? PrintRequestPayload,
            printResponseBody ?? PrintResponseBody,
            printHeaders ?? PrintHeaders,
            printCurl ?? PrintCurl,
            printOnlyOnError ?? PrintOnlyOnError,
            printFormat ?? PrintFormat,
            printColor ?? PrintColor,
            printMaxChars ?? PrintMaxChars,
            // The supplied value, not the resolved one, or the environment tier would be stranded.
            responseFormat ?? _suppliedResponseFormat,
            maxRequestSpoolBytes ?? MaxRequestSpoolBytes,
            maxRequestBytes ?? MaxRequestBytes,
            maxRequestSourceBytes ?? MaxRequestSourceBytes,
            maxMultipartOverheadBytes ?? MaxMultipartOverheadBytes,
            maxTokenCacheBytes ?? MaxTokenCacheBytes,
            maxCaBundleBytes ?? MaxCaBundleBytes);

    /// <summary>Pick one block: null for the first, an <c>int</c> position, or a unique <c>clientName</c>.</summary>
    public static object SelectCredentials(IEnumerable<object> credentials, object? instance = null)
    {
        var blocks = credentials?.ToList() ?? new List<object>();
        if (blocks.Count == 0)
            throw new SdkUsageError("no credentials were supplied");

        switch (instance)
        {
            case null:
                return blocks[0];

            case int position:
                {
                    var size = blocks.Count;
                    if (position >= -size && position < size)
                        return blocks[((position % size) + size) % size];
                    throw new SdkUsageError(Redaction.RedactMessageText(
                        $"no credentials at index {position.ToString(CultureInfo.InvariantCulture)}; "
                        + $"{size} supplied (0-{size - 1})"));
                }

            default:
                {
                    var name = instance as string;
                    var matched = new List<int>();
                    if (name is not null)
                        for (var index = 0; index < blocks.Count; index++)
                            if (string.Equals(ReadClientName(blocks[index]), name, StringComparison.Ordinal))
                                matched.Add(index);
                    if (matched.Count > 1)
                        throw new SdkUsageError(Redaction.RedactMessageText($"instance '{name}' is defined more than once, at "
                            + $"index {string.Join(" and ", matched)}; clientName has to identify one entry"));
                    if (matched.Count == 1)
                        return blocks[matched[0]];
                    var available = string.Join(", ",
                        blocks.Select((block, i) => ReadClientName(block) ?? $"<index {i}>"));
                    var rendered = name is not null
                        ? $"'{name}'"
                        : Convert.ToString(instance, CultureInfo.InvariantCulture);
                    throw new SdkUsageError(Redaction.RedactMessageText($"unknown instance {rendered}; available: {available}"));
                }
        }
    }

    /// <summary>An entry's <c>clientName</c>, whatever kind of block it is; <c>null</c> for neither kind.</summary>
    public static string? ReadClientName(object? block) => block switch
    {
        SdkConfig config => config.ClientName,
        AuthConfig auth => auth.ClientName,
        IReadOnlyDictionary<string, object?> mapping =>
            mapping.TryGetValue("client_name", out var name)
                && name?.ToString() is { Length: > 0 } text
                    ? text
                    : null,
        _ => null,
    };

    /// <summary>Pick one entry from a list of credential blocks and build a config from it.</summary>
    public static SdkConfig FromCredentials(
        IEnumerable<object> credentials,
        object? instance = null,
        Func<SdkConfig, SdkConfig>? configure = null)
    {
        var chosen = SelectCredentials(credentials, instance);
        var selected = chosen switch
        {
            SdkConfig config => config,
            AuthConfig auth => new SdkConfig(auth),
            IReadOnlyDictionary<string, object?> mapping => FromMapping(mapping, instance),
            _ => throw new SdkConfigError(Redaction.RedactMessageText(
                CredentialsLabel(instance)
                + ": expected an SdkConfig, an AuthConfig or a mapping of config fields, got "
                + (chosen?.GetType().Name ?? "null"))),
        };
        if (configure is null)
            return selected;
        return configure(selected)
            ?? throw new SdkUsageError("configure returned null; return the config "
                + "(or one derived with WithOverrides) instead");
    }

    private static string CredentialsLabel(object? instance) =>
        "credentials[" + (instance is null ? "0" : instance is string name ? $"'{name}'" : instance) + "]";

    /// <summary>Build a config from a mapping of snake_case field names.</summary>
    private static SdkConfig FromMapping(
        IReadOnlyDictionary<string, object?> block, object? instance)
    {
        var where = CredentialsLabel(instance);
        var remaining = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var (key, value) in block)
            remaining[key] = value;

        var carriesAuth = remaining.TryGetValue("auth", out var suppliedAuth);
        remaining.Remove("auth");

        var authValues = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var parameter in AuthParameters)
            if (remaining.TryGetValue(parameter.Key, out var value))
            {
                authValues[parameter.Key] = value;
                remaining.Remove(parameter.Key);
            }

        if (carriesAuth && authValues.Count > 0)
            throw new SdkConfigError(Redaction.RedactMessageText(
                "auth: an AuthConfig was supplied alongside the credential field(s) "
                + string.Join(", ", authValues.Keys.OrderBy(key => key, StringComparer.Ordinal))
                + "; pass one or the other"));

        AuthConfig auth;
        if (carriesAuth)
        {
            auth = suppliedAuth as AuthConfig
                ?? throw new SdkConfigError(Redaction.RedactMessageText(
                    $"{where}: auth must be an AuthConfig, got "
                    + (suppliedAuth?.GetType().Name ?? "null")));
        }
        else
        {
            var absent = AuthParameters
                .Where(parameter => !parameter.Value.HasDefaultValue
                    && !authValues.ContainsKey(parameter.Key))
                .Select(parameter => parameter.Key)
                .ToList();
            if (absent.Count > 0)
                throw new SdkConfigError(Redaction.RedactMessageText(
                    "credentials: missing required field(s): " + string.Join(", ", absent)));
            auth = (AuthConfig)Build(AuthConstructor, authValues, where, auth: null);
        }

        var unknown = remaining.Keys
            .Where(key => !SettingParameters.ContainsKey(key))
            .OrderBy(key => key, StringComparer.Ordinal)
            .ToList();
        if (unknown.Count > 0)
            throw new SdkConfigError(Redaction.RedactMessageText(
                $"{where}: unknown config field(s): " + string.Join(", ", unknown)
                + "; known: " + string.Join(", ", SettingParameters.Keys
                    .OrderBy(key => key, StringComparer.Ordinal))));

        return (SdkConfig)Build(SdkConstructor, remaining, where, auth);
    }

    private static readonly System.Reflection.ConstructorInfo AuthConstructor =
        typeof(AuthConfig).GetConstructors().Single();

    private static readonly System.Reflection.ConstructorInfo SdkConstructor =
        typeof(SdkConfig).GetConstructors()
            .Single(candidate => candidate.GetParameters().Length > 1);

    /// <summary>AuthConfig's constructor parameters, keyed by their snake_case spelling.</summary>
    private static readonly IReadOnlyDictionary<string, System.Reflection.ParameterInfo>
        AuthParameters = AuthConstructor.GetParameters()
            .ToDictionary(parameter => SnakeCase(parameter.Name!), parameter => parameter,
                StringComparer.Ordinal);

    private static readonly IReadOnlyDictionary<string, System.Reflection.ParameterInfo>
        SettingParameters = SdkConstructor.GetParameters()
            .Where(parameter => parameter.Position > 0)
            .ToDictionary(parameter => SnakeCase(parameter.Name!), parameter => parameter,
                StringComparer.Ordinal);

    /// <summary>Invoke a constructor from snake_case keys, defaulting every parameter not carried.</summary>
    private static object Build(
        System.Reflection.ConstructorInfo constructor,
        IReadOnlyDictionary<string, object?> values,
        string where,
        AuthConfig? auth)
    {
        var parameters = constructor.GetParameters();
        var arguments = new object?[parameters.Length];
        for (var index = 0; index < parameters.Length; index++)
        {
            var parameter = parameters[index];
            if (auth is not null && index == 0)
            {
                arguments[index] = auth;
                continue;
            }
            var key = SnakeCase(parameter.Name!);
            arguments[index] = values.TryGetValue(key, out var value)
                ? Coerce(value, parameter.ParameterType, key, where)
                : parameter.HasDefaultValue ? parameter.DefaultValue : null;
        }
        try
        {
            return constructor.Invoke(arguments);
        }
        catch (System.Reflection.TargetInvocationException wrapped)
            when (wrapped.InnerException is not null)
        {
            throw wrapped.InnerException;
        }
    }

    /// <summary>Convert one loader-shaped value (JSON, YAML or <c>.env</c>) to a parameter's type.</summary>
    private static object? Coerce(object? value, Type wanted, string key, string where)
    {
        var target = Nullable.GetUnderlyingType(wanted) ?? wanted;
        if (value is null)
            return target.IsValueType && wanted == target
                ? throw new SdkConfigError(Redaction.RedactMessageText(
                    $"{where}: {key} must not be null"))
                : null;
        if (target.IsInstanceOfType(value))
            return value;
        if (target == typeof(double) && value is (int or long or float or double or decimal))
            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        if (target == typeof(int) && value is (int or long))
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        if (typeof(IReadOnlySet<int>).IsAssignableFrom(target)
            && value is System.Collections.IEnumerable codes and not string)
        {
            var set = new HashSet<int>();
            foreach (var code in codes)
                set.Add(Convert.ToInt32(code, CultureInfo.InvariantCulture));
            return set;
        }
        if (typeof(IReadOnlyDictionary<string, string>).IsAssignableFrom(target)
            && value is System.Collections.IDictionary pairs)
        {
            var headers = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (System.Collections.DictionaryEntry pair in pairs)
                headers[pair.Key.ToString() ?? string.Empty] = pair.Value?.ToString() ?? string.Empty;
            return headers;
        }
        throw new SdkConfigError(Redaction.RedactMessageText(
            $"{where}: {key} expected {target.Name}, got {value.GetType().Name}"));
    }

    /// <summary>A camelCase name respelled snake_case; a run of capitals is one word.</summary>
    private static string SnakeCase(string name)
    {
        var builder = new System.Text.StringBuilder(name.Length + 4);
        for (var index = 0; index < name.Length; index++)
        {
            var character = name[index];
            if (char.IsUpper(character) && index > 0)
            {
                var previousLower = char.IsLower(name[index - 1]);
                var nextLower = index + 1 < name.Length && char.IsLower(name[index + 1]);
                if (previousLower || nextLower)
                    builder.Append('_');
            }
            builder.Append(char.ToLowerInvariant(character));
        }
        return builder.ToString();
    }

    /// <summary>Every field, with secrets redacted.</summary>
    public override string ToString()
    {
        var fields = new (string Name, object? Value)[]
        {
            ("auth", Auth),
            ("apiVersion", ApiVersion),
            ("timeoutSeconds", TimeoutSeconds),
            ("connectTimeoutSeconds", ConnectTimeoutSeconds),
            ("maxRetries", MaxRetries),
            ("retryableHttpCodes", RetryableHttpCodes),
            ("unsafeRetryableHttpCodes", UnsafeRetryableHttpCodes),
            ("retryBackoffSeconds", RetryBackoffSeconds),
            ("retryBackoffMaxSeconds", RetryBackoffMaxSeconds),
            ("verifySsl", VerifySsl),
            ("caBundle", CaBundle),
            ("proxyUrl", ProxyUrl),
            ("defaultHeaders", Redaction.RedactMapping(DefaultHeaders)),
            ("raiseOnError", RaiseOnError),
            ("userAgent", UserAgent),
            ("logLevel", LogLevel),
            ("logBodies", LogBodies),
            ("logFile", LogFile),
            ("printApiInfo", PrintApiInfo),
            ("printRequestPayload", PrintRequestPayload),
            ("printResponseBody", PrintResponseBody),
            ("printHeaders", PrintHeaders),
            ("printCurl", PrintCurl),
            ("printOnlyOnError", PrintOnlyOnError),
            ("printFormat", PrintFormat),
            ("printColor", PrintColor),
            ("printMaxChars", PrintMaxChars),
            ("responseFormat", ResponseFormat),
            ("maxResponseBytes", MaxResponseBytes),
            ("maxRequestSpoolBytes", MaxRequestSpoolBytes),
            ("maxRequestBytes", MaxRequestBytes),
            ("maxRequestSourceBytes", MaxRequestSourceBytes),
            ("maxMultipartOverheadBytes", MaxMultipartOverheadBytes),
            ("maxTokenCacheBytes", MaxTokenCacheBytes),
            ("maxCaBundleBytes", MaxCaBundleBytes),
        };
        return "SdkConfig("
            + string.Join(", ", fields.Select(field =>
                field.Name + "=" + RenderField(field.Name, field.Value)))
            + ")";
    }

    /// <summary>Render one field for <see cref="ToString"/>, redacting by its name and its content.</summary>
    private static string RenderField(string name, object? value) => value switch
    {
        null => "null",
        _ when Redaction.ShouldRedact(name) => $"'{Redaction.Redacted}'",
        AuthConfig auth => auth.ToString(),
        string text => $"'{Redaction.RedactText(text)}'",
        bool flag => flag ? "true" : "false",
        IReadOnlySet<int> codes => "[" + string.Join(", ", codes.OrderBy(code => code)) + "]",
        IReadOnlyDictionary<string, string> map =>
            "{" + string.Join(", ", map.Select(entry => $"'{entry.Key}': '{entry.Value}'")) + "}",
        IFormattable number => number.ToString(null, CultureInfo.InvariantCulture),
        _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? "null",
    };

    private void Validate()
    {
        var problems = new List<string>();

        if (Auth is null)
            problems.Add("auth: an AuthConfig is required");
        if (ApiVersion is null || !ApiVersionPattern().IsMatch(ApiVersion))
            problems.Add($"apiVersion: expected something like 'v4' or '4', got '{ApiVersion}'");
        if (MaxRetries < 1)
            problems.Add("maxRetries: must be at least 1 (1 disables retry), got "
                + MaxRetries.ToString(CultureInfo.InvariantCulture));
        if (!FinitePositive(TimeoutSeconds))
            problems.Add("timeoutSeconds: must be a finite number > 0");
        if (!FinitePositive(ConnectTimeoutSeconds))
            problems.Add("connectTimeoutSeconds: must be a finite number > 0");
        if (!double.IsFinite(RetryBackoffSeconds) || RetryBackoffSeconds < 0)
            problems.Add("retryBackoffSeconds: must be a finite number >= 0");
        if (!double.IsFinite(RetryBackoffMaxSeconds) || RetryBackoffMaxSeconds < RetryBackoffSeconds)
            problems.Add("retryBackoffMaxSeconds: must be finite and >= retryBackoffSeconds");
        AtLeast(problems, "maxResponseBytes", MaxResponseBytes, 1);
        AtLeast(problems, "maxRequestSpoolBytes", MaxRequestSpoolBytes, 0);
        AtLeast(problems, "maxRequestBytes", MaxRequestBytes, 1);
        AtLeast(problems, "maxRequestSourceBytes", MaxRequestSourceBytes, 0);
        AtLeast(problems, "maxMultipartOverheadBytes", MaxMultipartOverheadBytes, 0);
        AtLeast(problems, "maxTokenCacheBytes", MaxTokenCacheBytes, 1);
        AtLeast(problems, "maxCaBundleBytes", MaxCaBundleBytes, 1);
        problems.AddRange(RetryPolicy.StatusCodeProblems("retryableHttpCodes", RetryableHttpCodes));
        problems.AddRange(RetryPolicy.StatusCodeProblems("unsafeRetryableHttpCodes", UnsafeRetryableHttpCodes));
        if (LogLevel is null || !ValidLogLevels.Contains(LogLevel))
            problems.Add("logLevel: expected one of " + string.Join(", ", LogLevelsBySeverity)
                + $", got '{LogLevel}'");
        if (PrintFormat is null || !ValidPrintFormats.Contains(PrintFormat))
            problems.Add($"printFormat: expected one of {PrintFormatPretty}, {PrintFormatCompact}, got '{PrintFormat}'");
        if (PrintMaxChars < 0)
            problems.Add("printMaxChars: must be >= 0 (0 = unlimited)");
        AddResponseFormatProblem(problems, ResponseFormatVariable, _environmentResponseFormat);
        AddResponseFormatProblem(problems, "responseFormat", _suppliedResponseFormat);
        if (!string.IsNullOrWhiteSpace(CaBundle))
        {
            if (!VerifySsl)
                problems.Add("caBundle and verifySsl=false are contradictory: one supplies a "
                    + "trust store, the other disables verification");
            if (!File.Exists(CaBundle))
            {
                problems.Add("caBundle: no such file: " + CaBundle);
            }
            else
            {
                try
                {
                    Transport.RefuseAnOversizedCaBundle(CaBundle!, MaxCaBundleBytes);
                }
                catch (SdkConfigError oversized)
                {
                    problems.Add(oversized.Message);
                }
            }
        }
        problems.AddRange(HeaderProblems(DefaultHeaders));

        if (problems.Count > 0)
            throw new SdkConfigError(Redaction.RedactMessageText(string.Join("; ", problems)));
    }

    private static void AtLeast(List<string> problems, string name, long value, long floor)
    {
        if (value >= floor)
            return;
        problems.Add($"{name}: must be at least "
            + floor.ToString(CultureInfo.InvariantCulture) + " byte(s), got "
            + value.ToString(CultureInfo.InvariantCulture)
            + " - there is no value that disables the ceiling; raise it instead");
    }

    private static void AddResponseFormatProblem(List<string> problems, string label, string? value)
    {
        if (value is not null && !ValidResponseFormats.Contains(value))
            problems.Add($"{label}: expected one of " + string.Join(", ", ResponseFormats) + $", got '{value}'");
    }

    /// <summary>Refuse a default header the transport could not send; the value is never shown.</summary>
    private static List<string> HeaderProblems(IReadOnlyDictionary<string, string> headers)
    {
        var problems = new List<string>();
        foreach (var (name, value) in headers)
        {
            if (!Transport.IsSendableHeader(name, value))
                problems.Add($"defaultHeaders: '{Client.ExchangeConsole.NeutraliseOneLine(name)}' "
                    + "cannot be sent as a header - " + Transport.HeaderGrammar
                    + ". The value is not shown: it may be a credential");
        }
        return problems;
    }

    private static string? NormaliseApiVersion(string? raw)
    {
        if (raw is null)
            return null;
        var value = raw.Trim().ToLowerInvariant();
        if (value.Length > 0 && !value.StartsWith('v'))
            value = "v" + value;
        return value;
    }

    private static bool FinitePositive(double value) => double.IsFinite(value) && value > 0;
}
