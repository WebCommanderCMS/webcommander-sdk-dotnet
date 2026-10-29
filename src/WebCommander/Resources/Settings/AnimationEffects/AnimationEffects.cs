using WebCommander.Client;

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

    private static Dictionary<string, object?> Section(string section) =>
        new() { ["section"] = section };
}
