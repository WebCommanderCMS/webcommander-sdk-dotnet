namespace WebCommander.Resources.Settings.ShippingSettings.ShippingSettingsDto;

/// <summary>What a reference field holds: the id the store keeps and the name it resolves to, or <c>null</c>.</summary>
public sealed class ProfileReference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>The <c>group_meta</c> entry: what the area is and whether it can be written.</summary>
public sealed class AreaMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }
}

/// <summary>The <c>group_meta</c> block - a single <see cref="AreaMeta"/>, keyed by the area itself.</summary>
public sealed class GroupMeta : Dto
{
    public AreaMeta? ShippingSettings { get; set; }
}

/// <summary>The shipping settings, seven fields flat under <c>shipping_settings</c> beside <c>group_meta</c>.</summary>
public sealed class ShippingSettings : Dto
{
    protected override string? EnvelopeKey => "shipping_settings";

    public ProfileReference? DefaultShippingProfile { get; set; }

    public bool? EnableShippingDefaultTax { get; set; }

    public ProfileReference? DefaultDeliveryProfile { get; set; }

    public bool? EnableDeliveryDefaultTax { get; set; }

    public ProfileReference? ShippingTaxProfile { get; set; }

    public bool? EnableShippingClass { get; set; }

    public bool? EnableFlatViewClass { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public object? FieldMetadata { get; set; }
}
