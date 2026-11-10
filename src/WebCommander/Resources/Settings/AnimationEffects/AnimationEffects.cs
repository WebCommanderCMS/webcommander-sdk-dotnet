using WebCommander.Client;
using WebCommander.Errors;

namespace WebCommander.Resources.Settings.AnimationEffects;

/// <summary>Animation effects - <c>/admin/settings/animation_effects*</c> - as a typed resource bound to one client.</summary>
public sealed class AnimationEffects : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.AnimationEffects</c>.</summary>
    public AnimationEffects(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read the whole area - <c>GET /admin/settings/animation_effects</c>.</summary>
    public Task<ApiResponse> GetAnimationEffectsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(AnimationEffectsUrls.AnimationEffects,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                // Qualified: the simple name resolves to this class first.
                DtoType = typeof(AnimationEffectsDto.AnimationEffects),
            }, cancellationToken);

    /// <summary>Write fields in one or more sections - <c>PATCH /admin/settings/animation_effects</c>.</summary>
    public Task<ApiResponse> PatchAnimationEffectsAsync(
        object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(AnimationEffectsUrls.AnimationEffects,
            new RequestOptions
            {
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AnimationEffectsDto.AnimationEffects),
            }, cancellationToken);
    }

    /// <summary>HEAD the area - <c>HEAD /admin/settings/animation_effects</c>.</summary>
    public Task<ApiResponse> HeadAnimationEffectsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", AnimationEffectsUrls.AnimationEffects,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Read one section - <c>GET /admin/settings/animation_effects/{section}</c>.</summary>
    public Task<ApiResponse> GetAnimationEffectsSectionAsync(
        string section,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(AnimationEffectsUrls.AnimationEffectsSection,
            new RequestOptions
            {
                PathParams = Section(section),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AnimationEffectsDto.AnimationEffects),
            }, cancellationToken);

    /// <summary>Write fields in one section - <c>PATCH /admin/settings/animation_effects/{section}</c>.</summary>
    public Task<ApiResponse> PatchAnimationEffectsSectionAsync(
        string section, object payload,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequirePayload(payload);
        return Client.SendPatchRequestAsync(AnimationEffectsUrls.AnimationEffectsSection,
            new RequestOptions
            {
                PathParams = Section(section),
                JsonBody = payload,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AnimationEffectsDto.AnimationEffects),
            }, cancellationToken);
    }

    /// <summary>Upload the custom preloader image - <c>POST /admin/settings/animation_effects/custom_loader</c>.
    /// The part is named <c>file</c> (svg, png, jpg, jpeg or gif), and a <c>string</c> or <c>byte[]</c> is its
    /// contents, not a path. <c>201</c>, and the loader reads <c>enabled: true</c>.</summary>
    public Task<ApiResponse> UploadAnimationEffectsCustomLoaderAsync(
        IReadOnlyDictionary<string, object?> files,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null)
    {
        RequireFiles(files);
        return Client.SendPostRequestAsync(AnimationEffectsUrls.AnimationEffectsCustomLoader,
            new RequestOptions
            {
                Files = files,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(AnimationEffectsDto.CustomLoaderSection),
            }, cancellationToken);
    }

    /// <summary>Remove the custom preloader image - <c>DELETE /admin/settings/animation_effects/custom_loader</c>;
    /// <c>204</c>, also when there is none.</summary>
    public Task<ApiResponse> DeleteAnimationEffectsCustomLoaderAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendDeleteRequestAsync(AnimationEffectsUrls.AnimationEffectsCustomLoader,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static void RequireFiles(IReadOnlyDictionary<string, object?> files)
    {
        if (files is null || files.Count == 0)
            throw new SdkUsageError("files is required and must carry the upload: this endpoint's "
                + "whole body is the multipart part named 'file', and an empty one sends no "
                + "multipart body at all");
    }

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };
}
