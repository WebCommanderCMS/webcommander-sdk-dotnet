using System.Globalization;
using System.Text.Json;

namespace WebCommander.Client;

internal static class Display
{
    internal static string Seconds(double elapsed) =>
        elapsed.ToString("0.0###", CultureInfo.InvariantCulture);

    internal static string RenderBody(object? value)
    {
        switch (value)
        {
            case null:
                return "";
            case string text:
                return text;
            case byte[] bytes:
                return $"<{bytes.Length.ToString(CultureInfo.InvariantCulture)} bytes>";
        }
        try
        {
            return JsonSerializer.Serialize(value, Redaction.DisplayJson);
        }
        catch (Exception error) when (error is JsonException or NotSupportedException)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
        }
    }
}
