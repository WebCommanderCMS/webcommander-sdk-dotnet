namespace WebCommander.Resources.InventoryLocations;

/// <summary>Path templates for the inventory location endpoints.</summary>
public static class InventoryLocationsUrls
{
    public const string InventoryLocations = "/api/{api_version}/admin/inventory_locations";

    public const string InventoryLocation =
        "/api/{api_version}/admin/inventory_locations/{inventory_location_id}";

    public const string InventoryLocationStock =
        "/api/{api_version}/admin/inventory_locations/{inventory_location_id}/stock";

    public const string InventoryLocationDefault =
        "/api/{api_version}/admin/inventory_locations/{inventory_location_id}/default";

    public const string InventoryLocationStatus =
        "/api/{api_version}/admin/inventory_locations/{inventory_location_id}/status";
}
