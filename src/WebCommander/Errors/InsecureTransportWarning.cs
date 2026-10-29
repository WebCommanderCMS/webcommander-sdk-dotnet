using System.Collections.Concurrent;

namespace WebCommander.Errors;

/// <summary>A plain <c>http://</c> base URL sends the token in cleartext; written to stderr, never thrown.</summary>
public sealed class InsecureTransportWarning
{
    private static readonly ConcurrentDictionary<string, byte> Delivered = new(StringComparer.Ordinal);

    /// <summary>Stop writing these warnings to stderr, process-wide; the log record still goes out.</summary>
    public static bool Suppress { get; set; }

    internal static bool ClaimStandardErrorDelivery(string message) =>
        !Suppress && Delivered.TryAdd(message, 0);

    public InsecureTransportWarning(string host)
    {
        Host = host;
        Message =
            $"baseUrl uses http://, so every request to {host} crosses the network " +
            "in cleartext - the bearer token, and the credentials in the token " +
            "request itself. Use https:// unless this is a local development instance";
    }

    private InsecureTransportWarning(string host, string message)
    {
        Host = host;
        Message = message;
    }

    /// <summary>The companion warning for <c>verifySsl=false</c>, which names no host.</summary>
    public static InsecureTransportWarning ForDisabledVerification() =>
        new("", "TLS verification is disabled (verifySsl=false); responses cannot be trusted");

    public string Host { get; }

    public string Message { get; }

    public override string ToString() => Message;
}
