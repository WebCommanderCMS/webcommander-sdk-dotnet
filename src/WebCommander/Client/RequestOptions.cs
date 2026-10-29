namespace WebCommander.Client;

/// <summary>The optional arguments of one call; immutable, with collections copied in and out.</summary>
public sealed class RequestOptions
{
    public static RequestOptions None { get; } = new();

    /// <summary>Values for the <c>{name}</c> placeholders in the path template.</summary>
    public IReadOnlyDictionary<string, object?>? PathParams
    {
        get => Redaction.CopyBuffersInMap(_pathParams);
        init => _pathParams = Redaction.DetachMap(value);
    }

    /// <summary>Query parameters, normalised by the value table before they are sent.</summary>
    public IReadOnlyDictionary<string, object?>? QueryParams
    {
        get => Redaction.CopyBuffersInMap(_queryParams);
        init => _queryParams = Redaction.DetachMap(value);
    }

    /// <summary>A JSON body: a map, list or POCO, a raw JSON string sent as is, or a <see cref="Dto"/>.</summary>
    public object? JsonBody { get; init; }

    /// <summary>Form-encoded data to send; string-keyed, so a field name cannot repeat.</summary>
    public IReadOnlyDictionary<string, object?>? FormData
    {
        get => Redaction.CopyBuffersInMap(_formData);
        init => _formData = Redaction.DetachMap(value);
    }

    /// <summary>Multipart files to send; an oversized stream part is sent once, with no retry.</summary>
    public IReadOnlyDictionary<string, object?>? Files
    {
        get => Redaction.CopyBuffersInMap(_files);
        init => _files = Redaction.DetachMap(value);
    }

    /// <summary>Extra request headers, merged case-insensitively; <c>Authorization</c> cannot be overridden.</summary>
    public IReadOnlyDictionary<string, string>? Headers
    {
        get => _headers;
        init => _headers = Redaction.DetachText(value);
    }

    public bool Authenticated { get; init; } = true;

    /// <summary>Per-call override of <c>SdkConfig.RaiseOnError</c>; <c>null</c> defers to the config.</summary>
    public bool? RaiseOnError { get; init; }

    /// <summary>Per-call override of <c>SdkConfig.ResponseFormat</c>; <c>null</c> defers to the config.</summary>
    public string? ResponseFormat { get; init; }

    /// <summary>The <c>Dto</c> subclass <c>ApiResponse.Dto</c> builds for this call; <c>null</c> for none.</summary>
    public Type? DtoType { get; init; }

    internal IReadOnlyDictionary<string, object?>? PathParamsAsSent => _pathParams;

    internal IReadOnlyDictionary<string, object?>? QueryParamsAsSent => _queryParams;

    internal IReadOnlyDictionary<string, object?>? FormDataAsSent => _formData;

    internal IReadOnlyDictionary<string, object?>? FilesAsSent => _files;

    private readonly IReadOnlyDictionary<string, object?>? _pathParams;
    private readonly IReadOnlyDictionary<string, object?>? _queryParams;
    private readonly IReadOnlyDictionary<string, object?>? _formData;
    private readonly IReadOnlyDictionary<string, object?>? _files;
    private readonly IReadOnlyDictionary<string, string>? _headers;
}
