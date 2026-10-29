using System.Globalization;

using WebCommander.Client;
using WebCommander.Config;
using WebCommander.Errors;
using WebCommander.Http;

namespace WebCommander.Auth;

/// <summary>Fetches, caches and renews the access token, refresh grant first, then full credentials.</summary>
public sealed class TokenProvider
{
    /// <summary>The token endpoint path template; prefer <see cref="BuildTokenPath"/>.</summary>
    public const string TokenPathTemplate = "/api/{api_version}/oauth2/token";

    public static string BuildTokenPath(string apiVersion) =>
        TokenPathTemplate.Replace("{api_version}", apiVersion, StringComparison.Ordinal);

    private const string RefreshGrantType = "refresh_token";

    private readonly SdkConfig _config;
    private readonly Transport _transport;
    private readonly ITokenStore _tokenStore;
    private readonly string _tokenPath;

    public TokenProvider(SdkConfig config, Transport transport, ITokenStore tokenStore)
    {
        _config = config;
        _transport = transport;
        _tokenStore = tokenStore;
        _tokenPath = BuildTokenPath(config.ApiVersion!);
    }

    public string TokenUrl => _config.BaseUrl + _tokenPath;

    /// <summary>A usable access token, fetched only if needed.</summary>
    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var record = TokenRecord.FromMap(await _tokenStore.ReadTokenRecordAsync(cancellationToken).ConfigureAwait(false));
        if (record is not null && !record.NeedsRefresh())
            return record.AccessToken;
        return await RenewAccessTokenAsync(null, anyCachedTokenWillDo: true, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Replace a rejected token, unless another worker already renewed it.</summary>
    public Task<string> RefreshAccessTokenAsync(string? staleAccessToken, CancellationToken cancellationToken = default) =>
        RenewAccessTokenAsync(staleAccessToken, anyCachedTokenWillDo: false, cancellationToken);

    private async Task<string> RenewAccessTokenAsync(
        string? staleAccessToken, bool anyCachedTokenWillDo, CancellationToken cancellationToken)
    {
        await using var scope = await _tokenStore.AcquireTokenRefreshLockAsync(cancellationToken).ConfigureAwait(false);

        var record = TokenRecord.FromMap(await scope.ReadTokenRecordAsync(cancellationToken).ConfigureAwait(false));
        if (record is not null && !record.NeedsRefresh())
        {
            if (anyCachedTokenWillDo
                || (staleAccessToken is not null && record.AccessToken != staleAccessToken))
            {
                return record.AccessToken;
            }
        }

        TokenRecord? renewed = null;
        if (record?.RefreshToken is { } refreshToken)
            renewed = await RequestTokenWithRefreshGrantAsync(refreshToken, cancellationToken).ConfigureAwait(false);
        renewed ??= await RequestTokenWithCredentialsAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            await scope.WriteTokenRecordAsync(renewed.ToMap(), cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            // Best effort: a token already paid for is not discarded over a refused write.
        }
        return renewed.AccessToken;
    }

    private async Task<TokenRecord> RequestTokenWithCredentialsAsync(CancellationToken cancellationToken)
    {
        var response = await _transport.SendHttpRequestAsync(
            new TransportRequest("POST", TokenUrl)
            {
                JsonBody = CredentialsPayload(),
                Headers = TokenHeaders(),
            },
            cancellationToken).ConfigureAwait(false);

        if (!response.Succeeded)
        {
            var messages = response.GetErrorMessages();
            var error = new SdkAuthError(Redaction.RedactMessageText(
                $"token request failed with {response.StatusCode}: "
                    + (messages.Count == 0 ? "no detail" : messages[0])),
                response.StatusCode,
                response.ErrorType,
                Redaction.RedactValue(response.Map),
                Redaction.RedactMapping(response.Headers),
                response.RequestInfo);
            foreach (var message in messages)
                error.AddError(Redaction.RedactMessageText(message));
            throw error;
        }
        return BuildTokenRecord(response, null);
    }

    private Dictionary<string, object?> CredentialsPayload() => new()
    {
        ["grant_type"] = _config.Auth.GrantType,
        ["auth_string"] = _config.Auth.AuthString,
        ["redirect_uri"] = _config.Auth.RedirectUri,
        ["client_id"] = _config.Auth.ClientId,
        ["client_secret"] = _config.Auth.ClientSecret,
    };

    private async Task<TokenRecord?> RequestTokenWithRefreshGrantAsync(
        string refreshToken, CancellationToken cancellationToken)
    {
        var payload = new Dictionary<string, object?>
        {
            ["grant_type"] = RefreshGrantType,
            ["refresh_token"] = refreshToken,
            ["redirect_uri"] = _config.Auth.RedirectUri,
            ["client_id"] = _config.Auth.ClientId,
            ["client_secret"] = _config.Auth.ClientSecret,
        };
        try
        {
            var response = await _transport.SendHttpRequestAsync(
                new TransportRequest("POST", TokenUrl) { JsonBody = payload, Headers = TokenHeaders() },
                cancellationToken).ConfigureAwait(false);
            return response.Succeeded ? BuildTokenRecord(response, refreshToken) : null;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private Dictionary<string, string> TokenHeaders() => new()
    {
        ["Accept"] = "application/json",
        ["User-Agent"] = _config.UserAgent,
    };

    private static TokenRecord BuildTokenRecord(ApiResponse response, string? previousRefreshToken)
    {
        if (response.Map is not IReadOnlyDictionary<string, object?> envelope
            || envelope.GetValueOrDefault("access_token") is not string token
            || token.Length == 0)
        {
            throw new SdkAuthError(
                "token response did not contain an access_token",
                response.StatusCode, response.ErrorType,
                Redaction.RedactValue(response.Map), new Dictionary<string, string>(), response.RequestInfo);
        }
        var timeToLiveSeconds = ReadTtlSeconds(envelope.GetValueOrDefault("expires_in"));
        var refresh = envelope.GetValueOrDefault("refresh_token");
        return new TokenRecord(
            token,
            // RFC 6749 6: with no new refresh token, keep the one already held.
            refresh is string refreshToken && refreshToken.Length > 0 ? refreshToken : previousRefreshToken,
            TokenRecord.UnixSecondsNow() + timeToLiveSeconds,
            Math.Min(TokenRecord.ExpirySkewSeconds, timeToLiveSeconds / 2));
    }

    /// <summary>NaN, infinity, non-positive and unparseable lifetimes all take the default.</summary>
    private static double ReadTtlSeconds(object? expiresIn)
    {
        var timeToLiveSeconds = expiresIn switch
        {
            System.Numerics.BigInteger bigValue => (double)bigValue,
            string text when double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) => parsed,
            _ => TokenRecord.FiniteNumber(expiresIn) ?? TokenRecord.DefaultTokenTtlSeconds,
        };
        if (double.IsNaN(timeToLiveSeconds) || double.IsInfinity(timeToLiveSeconds) || timeToLiveSeconds <= 0)
            timeToLiveSeconds = TokenRecord.DefaultTokenTtlSeconds;
        return Math.Min(timeToLiveSeconds, TokenRecord.MaxTokenTtlSeconds);
    }
}
