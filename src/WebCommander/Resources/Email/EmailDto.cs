using System.Text.Json.Serialization;

namespace WebCommander.Resources.Email.EmailDto;

// Not a Dto: the Dto name rule cannot spell htmlContent.
/// <summary>The send body, bare and camelCase as the store takes it; <c>html_content</c> and other unknown keys
/// are ignored. <see cref="Recipients"/>, <see cref="Cc"/> and <see cref="Bcc"/> hold at most 100 addresses
/// together. Send <see cref="HtmlContent"/> or a plugin template's <see cref="Identifier"/>, not both.</summary>
public sealed class EmailInput
{
    [JsonPropertyName("recipients"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string?>? Recipients { get; set; }

    [JsonPropertyName("cc"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string?>? Cc { get; set; }

    [JsonPropertyName("bcc"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string?>? Bcc { get; set; }

    [JsonPropertyName("subject"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Subject { get; set; }

    [JsonPropertyName("htmlContent"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? HtmlContent { get; set; }

    /// <summary><c>html</c>, <c>text</c> or <c>text/html</c>.</summary>
    [JsonPropertyName("contentType"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContentType { get; set; }

    [JsonPropertyName("identifier"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Identifier { get; set; }

    /// <summary>The template's variables, an object of any shape.</summary>
    [JsonPropertyName("data"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; set; }
}

/// <summary>One address the store could not send to, and why, such as <c>send_failed</c>.</summary>
public sealed class EmailRejection : Dto
{
    public string? Email { get; set; }

    public string? Reason { get; set; }
}

/// <summary>The send's verdict: each list holds the addresses trimmed, a repeat in any case once.</summary>
public sealed class EmailSendResult : Dto
{
    public string? Status { get; set; }

    public IReadOnlyList<string?>? Accepted { get; set; }

    public IReadOnlyList<EmailRejection?>? Rejected { get; set; }

    public IReadOnlyList<string?>? Cc { get; set; }

    public IReadOnlyList<string?>? Bcc { get; set; }
}
