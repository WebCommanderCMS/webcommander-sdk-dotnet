namespace WebCommander.Resources.Settings.AnimationEffects.AnimationEffectsDto;

/// <summary>The uploaded loader that goes with <c>enable_custom_preloader</c>; read-only.</summary>
public sealed class CustomLoader : Dto
{
    public bool? Enabled { get; set; }

    public string? FileName { get; set; }

    public string? FileUrl { get; set; }
}

/// <summary>One entry of <c>preloader_custom_page</c> as a read sends it; a write takes page ids.</summary>
public sealed class PreloaderPage : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>The <c>preloader_design</c> section - the loader shown while a page loads.</summary>
public sealed class PreloaderDesign : Dto
{
    protected override string? EnvelopeKey => "preloader_design";

    public bool? EnablePreloader { get; set; }

    public bool? EnableCustomPreloader { get; set; }

    public string? PreloaderPreset { get; set; }

    public long? PreloaderSize { get; set; }

    public double? PreloaderBackgroundOpacity { get; set; }

    public string? BackgroundPreloader { get; set; }

    public string? PreloaderHideCondition { get; set; }

    public string? PreloaderShowTimeCondition { get; set; }

    public double? PreloaderHideAfterTime { get; set; }

    public string? PreloaderPage { get; set; }

    public IReadOnlyList<PreloaderPage?>? PreloaderCustomPage { get; set; }

    public CustomLoader? CustomLoader { get; set; }
}

/// <summary>The <c>widget_transition</c> section - whether widgets fade in, and over how long.</summary>
public sealed class WidgetTransition : Dto
{
    protected override string? EnvelopeKey => "widget_transition";

    public bool? WidgetSmoothRender { get; set; }

    public long? WidgetSmoothRenderDuration { get; set; }
}

/// <summary>The <c>page_transition</c> section - the animation between pages and when it plays.</summary>
public sealed class PageTransition : Dto
{
    protected override string? EnvelopeKey => "page_transition";

    public string? PageTransitionAnimationType { get; set; }

    public double? TransitionAnimationDuration { get; set; }

    public bool? AnimationBeforeEnteringNewPage { get; set; }

    public bool? AnimationAfterEnteringNewPage { get; set; }
}

/// <summary>The <c>image_lazy_effect</c> section - which pages lazy-load their images.</summary>
public sealed class ImageLazyEffect : Dto
{
    protected override string? EnvelopeKey => "image_lazy_effect";

    public string? PreloaderImageLazyEffectPreset { get; set; }

    public bool? ProductDetailsPageImageLazyEffect { get; set; }

    public bool? FlyingCartImageLazyEffect { get; set; }

    public bool? ProductListImageLazyEffect { get; set; }

    public bool? PropertyListImageLazyEffect { get; set; }

    public bool? PropertyDetailsPageImageLazyEffect { get; set; }

    public bool? BlogListImageLazyEffect { get; set; }

    public bool? BlogDetailsPageImageLazyEffect { get; set; }

    public long? LazyLoaderSize { get; set; }

    public string? BackgroundLazyLoader { get; set; }

    public string? ContentImageLazyLoadBackground { get; set; }
}

/// <summary>The <c>page_skeleton</c> section, the one-field section of this area.</summary>
public sealed class PageSkeleton : Dto
{
    protected override string? EnvelopeKey => "page_skeleton";

    public bool? CheckoutPageSkeleton { get; set; }
}

/// <summary>One entry of <c>group_meta</c>: what a thing is, whether it is writable, where it is managed.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block, one <see cref="SectionMeta"/> per section; only the facade sends it.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? PreloaderDesign { get; set; }

    public SectionMeta? WidgetTransition { get; set; }

    public SectionMeta? PageTransition { get; set; }

    public SectionMeta? ImageLazyEffect { get; set; }

    public SectionMeta? PageSkeleton { get; set; }
}

/// <summary>The animation effects settings, as the facade or a single section sends them.</summary>
public sealed class AnimationEffects : Dto
{
    protected override string? EnvelopeKey => "animation_effects";

    public PreloaderDesign? PreloaderDesign { get; set; }

    public WidgetTransition? WidgetTransition { get; set; }

    public PageTransition? PageTransition { get; set; }

    public ImageLazyEffect? ImageLazyEffect { get; set; }

    public PageSkeleton? PageSkeleton { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
