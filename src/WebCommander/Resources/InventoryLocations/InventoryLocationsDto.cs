namespace WebCommander.Resources.InventoryLocations.InventoryLocationsDto;

/// <summary>One inventory location, the same seven fields in every form the API sends.</summary>
public sealed class InventoryLocation : Dto
{
    protected override string? EnvelopeKey => "inventory_location";

    /// <summary>What <c>{inventory_location_id}</c> takes; an integer the store assigns.</summary>
    public long? Id { get; set; }

    /// <summary>Required on both writes and not unique: several rows may share one name.</summary>
    public string? Name { get; set; }

    /// <summary>Optional text; <c>null</c> when a create or an update leaves it out.</summary>
    public string? Address { get; set; }

    /// <summary>Defaults to <c>true</c> on a create, and on an update that leaves it out.</summary>
    public bool? Active { get; set; }

    /// <summary>Held by at most one row; the row holding it cannot be deleted.</summary>
    public bool? Default { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The writable form of <see cref="InventoryLocation"/> for the create, the update and the patch, with no envelope; left unset, the update blanks <c>address</c> and turns <c>active</c> on, and the patch keeps both.</summary>
public sealed class InventoryLocationInput : Dto
{
    public string? Name { get; set; }

    public string? Address { get; set; }

    public bool? Active { get; set; }

    public bool? Default { get; set; }
}

/// <summary>The status sub-path's body: <see cref="Active"/>, required and the only field accepted.</summary>
public sealed class InventoryLocationStatusInput : Dto
{
    public bool? Active { get; set; }
}

/// <summary>The five-key <c>pagination</c> block of the listing and the stock sub-path.</summary>
public sealed class Pagination : Dto
{
    /// <summary>How many rows match the filter across every page.</summary>
    public long? Total { get; set; }

    /// <summary>The page size applied: the <c>limit</c> sent, from 1 to 100, or 20.</summary>
    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasPrevious { get; set; }

    public bool? HasNext { get; set; }
}

/// <summary>One page of the listing: the rows and the block beside them, with no envelope.</summary>
public sealed class InventoryLocationPage : Dto
{
    public IReadOnlyList<InventoryLocation?>? InventoryLocations { get; set; }

    public Pagination? Pagination { get; set; }
}

/// <summary>The product one stock item belongs to.</summary>
public sealed class InventoryLocationStockProduct : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Sku { get; set; }
}

/// <summary>One product's holding at a location; <see cref="DefaultForProduct"/> is the product's own flag.</summary>
public sealed class InventoryLocationStockItem : Dto
{
    public InventoryLocationStockProduct? Product { get; set; }

    public object? Variation { get; set; }

    public long? AvailableStock { get; set; }

    public long? LowStockLevel { get; set; }

    public bool? DefaultForProduct { get; set; }
}

/// <summary>The stock sub-path's body: the location, its items and their pagination.</summary>
public sealed class InventoryLocationStock : Dto
{
    public InventoryLocation? InventoryLocation { get; set; }

    public IReadOnlyList<InventoryLocationStockItem?>? Items { get; set; }

    public Pagination? Pagination { get; set; }
}
