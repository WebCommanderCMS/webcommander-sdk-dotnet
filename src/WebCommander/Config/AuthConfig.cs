using WebCommander.Errors;

namespace WebCommander.Config;

// Not a record: `with` would skip validation, and its ToString would print both secrets.
/// <summary>Who you are and which instance you talk to; validated, with both secrets redacted.</summary>
public sealed class AuthConfig
{
    /// <summary>The grant type the credentials token request sends unless another is configured.</summary>
    public const string DefaultGrantType = "client_credentials";

    /// <summary>Sentinel for the platform cache path; <c>null</c> keeps the token in memory instead.</summary>
    public const string UseDefaultTokenCache = "\0__WC_USE_DEFAULT_TOKEN_CACHE__\0";

    // "Not supplied" for WithOverrides, distinct from null and UseDefaultTokenCache.
    private const string KeepTokenCachePath = "\0__WC_KEEP_TOKEN_CACHE_PATH__\0";

    private static readonly (string Name, Func<AuthConfig, string> Read)[] RequiredFieldReaders =
    {
        ("baseUrl", self => self.BaseUrl),
        ("clientId", self => self.ClientId),
        ("clientSecret", self => self.ClientSecret),
        ("redirectUri", self => self.RedirectUri),
        ("authString", self => self.AuthString),
    };

    /// <summary>Construct and validate, reporting every problem in one exception.</summary>
    public AuthConfig(
        string baseUrl,
        string clientId,
        string clientSecret,
        string redirectUri,
        string authString,
        string clientName,
        string grantType = DefaultGrantType,
        string? tokenCachePath = UseDefaultTokenCache)
    {
        BaseUrl = NormaliseBaseUrl(baseUrl);
        ClientId = clientId;
        ClientSecret = clientSecret;
        RedirectUri = redirectUri;
        AuthString = authString;
        ClientName = clientName;
        GrantType = grantType;

        Validate(tokenCachePath);

        TokenCachePath = tokenCachePath == UseDefaultTokenCache
            ? ResolveDefaultTokenCachePath(clientName)
            : tokenCachePath;
    }

    private static string ResolveDefaultTokenCachePath(string clientName)
    {
        try
        {
            return Auth.FileTokenStore.BuildDefaultTokenCachePath();
        }
        catch (SdkConfigError error)
        {
            throw new SdkConfigError(Redaction.RedactMessageText(
                $"instance {clientName}: tokenCachePath: {error.Message}"), error);
        }
    }

    /// <summary>The instance origin, trailing slashes stripped.</summary>
    public string BaseUrl { get; }

    public string ClientId { get; }

    /// <summary>The OAuth client secret; redacted by <see cref="ToString"/>.</summary>
    public string ClientSecret { get; }

    public string RedirectUri { get; }

    /// <summary>The base64 admin credential; redacted by <see cref="ToString"/>.</summary>
    public string AuthString { get; }

    /// <summary>Required label for this instance: shown in logs, and how it is selected by name.</summary>
    public string ClientName { get; }

    public string GrantType { get; }

    /// <summary>The resolved token cache file, or <c>null</c> for memory only.</summary>
    public string? TokenCachePath { get; }

    /// <summary>Derive new credentials through the validating constructor; this instance is untouched.</summary>
    public AuthConfig WithOverrides(
        string? baseUrl = null,
        string? clientId = null,
        string? clientSecret = null,
        string? redirectUri = null,
        string? authString = null,
        string? clientName = null,
        string? grantType = null,
        string? tokenCachePath = KeepTokenCachePath)
    {
        return new AuthConfig(
            baseUrl ?? BaseUrl,
            clientId ?? ClientId,
            clientSecret ?? ClientSecret,
            redirectUri ?? RedirectUri,
            authString ?? AuthString,
            clientName ?? ClientName,
            grantType ?? GrantType,
            tokenCachePath == KeepTokenCachePath ? TokenCachePath : tokenCachePath);
    }

    /// <summary>Credentials redacted, so safe to log.</summary>
    public override string ToString()
    {
        var shown = Redaction.RedactFields(this, FieldOrder);
        return $"AuthConfig({string.Join(", ", shown)})";
    }

    /// <summary>The fields <see cref="ToString"/> renders, in order; every one is listed.</summary>
    private static readonly string[] FieldOrder =
    {
        nameof(BaseUrl), nameof(ClientId), nameof(ClientSecret), nameof(RedirectUri),
        nameof(AuthString), nameof(ClientName), nameof(GrantType), nameof(TokenCachePath),
    };

    private void Validate(string? tokenCachePath)
    {
        var problems = new List<string>();

        var missing = new List<string>();
        foreach (var (name, read) in RequiredFieldReaders)
        {
            if (IsBlank(read(this)))
                missing.Add(name);
        }
        if (missing.Count > 0)
            problems.Add("missing required values: " + string.Join(", ", missing));

        if (!IsBlank(BaseUrl))
            problems.AddRange(ProblemsWithTheBaseUrl(BaseUrl));

        if (IsBlank(ClientName))
            problems.Add("clientName: must not be blank");
        if (IsBlank(GrantType))
            problems.Add("grantType: must not be blank");

        if (tokenCachePath is not null && tokenCachePath != UseDefaultTokenCache)
        {
            if (IsBlank(tokenCachePath))
                problems.Add("tokenCachePath: must not be blank - pass null to keep the token "
                    + "in memory, or name the file to write");
            else if (Directory.Exists(tokenCachePath))
                problems.Add($"tokenCachePath: '{tokenCachePath}' is an existing directory, so "
                    + "nothing could be written to it - name the file to write, or pass null to "
                    + "keep the token in memory");
        }

        if (problems.Count > 0)
        {
            var label = IsBlank(ClientName) ? "<unnamed>" : ClientName.Trim();
            throw new SdkConfigError(Redaction.RedactMessageText($"instance {label}: " + string.Join("; ", problems)));
        }
    }

    private static string NormaliseBaseUrl(string? raw)
    {
        if (raw is null) return "";
        return raw.Trim().TrimEnd('/');
    }

    private static List<string> ProblemsWithTheBaseUrl(string baseUrl)
    {
        var problems = new List<string>();
        if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var parsed))
        {
            problems.Add($"baseUrl: is not a URL, got '{baseUrl}'");
            return problems;
        }

        var scheme = parsed.Scheme.ToLowerInvariant();
        if ((scheme != "http" && scheme != "https") || string.IsNullOrEmpty(parsed.Host))
        {
            problems.Add("baseUrl: must start with http:// or https:// and include a host, "
                + $"got '{baseUrl}'");
            return problems;
        }
        if (!string.IsNullOrEmpty(parsed.UserInfo))
            problems.Add($"baseUrl: host {parsed.Host} carries embedded credentials, which would "
                + "replace the bearer token; strip the userinfo");
        if (!string.IsNullOrEmpty(parsed.Query) || !string.IsNullOrEmpty(parsed.Fragment))
            problems.Add("baseUrl: must be the instance origin only, with no query string or "
                + $"fragment, got '{baseUrl}'");
        return problems;
    }

    private static bool IsBlank(string? value) => string.IsNullOrWhiteSpace(value);
}
