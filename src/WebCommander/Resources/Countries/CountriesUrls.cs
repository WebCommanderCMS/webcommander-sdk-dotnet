namespace WebCommander.Resources.Countries;

/// <summary>Path templates for the country endpoints.</summary>
public static class CountriesUrls
{
    public const string Countries = "/api/{api_version}/admin/countries";

    public const string Country = "/api/{api_version}/admin/countries/{country_id}";

    public const string CountryStates = "/api/{api_version}/admin/countries/{country_id}/states";

    public const string CountryState =
        "/api/{api_version}/admin/countries/{country_id}/states/{state_id}";
}
