
namespace WebCommander.Auth;

/// <summary>An access token and when it stops being usable; its map is the on-disk cache format.</summary>
public sealed class TokenRecord
{
    /// <summary>Assumed lifetime when the token endpoint gives none.</summary>
    public const double DefaultTokenTtlSeconds = 3600;

    public const double ExpirySkewSeconds = 60;

    /// <summary>The furthest ahead a cached expiry is believed: one day, as the stamp is another clock's.</summary>
    public const double MaxTokenTtlSeconds = 24 * 60 * 60;

    /// <summary>Construct a record; a stored map goes through <see cref="FromMap"/> instead.</summary>
    public TokenRecord(string accessToken, string? refreshToken, double expiresAt, double skewSeconds)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresAt = expiresAt;
        SkewSeconds = skewSeconds;
    }

    public string AccessToken { get; }

    public string? RefreshToken { get; }

    /// <summary>Epoch seconds when the token expires.</summary>
    public double ExpiresAt { get; }

    /// <summary>How early to treat the token as expired, carried per record.</summary>
    public double SkewSeconds { get; }

    /// <summary>Whether to treat the token as expired, early by the skew so it cannot lapse in flight.</summary>
    public bool NeedsRefresh()
    {
        var now = UnixSecondsNow();
        if (ExpiresAt - now > MaxTokenTtlSeconds)
            return true;
        return now >= ExpiresAt - SkewSeconds;
    }

    public IReadOnlyDictionary<string, object?> ToMap() => new Dictionary<string, object?>
    {
        ["access_token"] = AccessToken,
        ["refresh_token"] = RefreshToken,
        ["expires_at"] = ExpiresAt,
        ["skew_seconds"] = SkewSeconds,
    };

    /// <summary>Rebuild from a stored map; <c>null</c> when absent, malformed or non-finite.</summary>
    public static TokenRecord? FromMap(IReadOnlyDictionary<string, object?>? record)
    {
        if (record is null)
            return null;
        var accessToken = record.GetValueOrDefault("access_token");
        var expiresAt = FiniteNumber(record.GetValueOrDefault("expires_at"));
        if (accessToken is not string token || token.Length == 0 || expiresAt is null)
            return null;
        var skew = FiniteNumber(record.GetValueOrDefault("skew_seconds"));
        if (skew is null || skew < 0)
            skew = ExpirySkewSeconds;
        skew = Math.Min(skew.Value, MaxTokenTtlSeconds);
        var refresh = record.GetValueOrDefault("refresh_token") as string;
        return new TokenRecord(
            token, refresh is { Length: > 0 } ? refresh : null, expiresAt.Value, skew.Value);
    }

    /// <summary>Both tokens redacted; an absent refresh token renders as an unquoted <c>null</c>.</summary>
    public override string ToString() =>
        $"TokenRecord(accessToken='{Redaction.Redacted}'"
        + $", refreshToken={(RefreshToken is null ? "null" : $"'{Redaction.Redacted}'")}"
        + $", expiresAt={ExpiresAt}, skewSeconds={SkewSeconds})";

    internal static double? FiniteNumber(object? value)
    {
        double number;
        switch (value)
        {
            case double doubleValue: number = doubleValue; break;
            case float floatValue: number = floatValue; break;
            case long longValue: number = longValue; break;
            case int intValue: number = intValue; break;
            case short shortValue: number = shortValue; break;
            case decimal decimalValue: number = (double)decimalValue; break;
            default: return null;
        }
        return double.IsNaN(number) || double.IsInfinity(number) ? null : number;
    }

    internal static double UnixSecondsNow() =>
        DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000.0;
}
