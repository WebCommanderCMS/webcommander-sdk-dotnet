using WebCommander.Client;
using WebCommander.Resources.Countries.CountriesDto;

namespace WebCommander.Resources.Countries;

/// <summary>Countries and their states - <c>/admin/countries*</c> - as a typed resource bound to one client.</summary>
public sealed class Countries : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Countries</c>.</summary>
    public Countries(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the countries - <c>GET /admin/countries</c>, <c>id</c> ascending. <c>limit</c> is 1 to 100 or
    /// <c>all</c>; <c>code</c>, <c>name</c> and <c>id</c> match exactly, <c>search</c> a substring of <c>name</c> or
    /// <c>code</c>; <c>active</c>; <c>sort</c> <c>id</c>, <c>name</c> or <c>code</c> with <c>order</c>.</summary>
    public Task<ApiResponse> ListCountriesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CountriesUrls.Countries,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CountryPage),
            }, cancellationToken);

    /// <summary>HEAD the country collection - <c>HEAD /admin/countries</c>; no body, and the listing's <c>ETag</c>.</summary>
    public Task<ApiResponse> HeadCountriesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CountriesUrls.Countries,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Retrieve one country - <c>GET /admin/countries/{country_id}</c>. The slot takes the numeric
    /// <c>id</c>, the ISO <c>code</c> or the <c>name</c>, in any case.</summary>
    public Task<ApiResponse> GetCountryAsync(
        object countryId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CountriesUrls.Country,
            new RequestOptions
            {
                PathParams = Id(countryId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Country),
            }, cancellationToken);

    /// <summary>HEAD one country - <c>HEAD /admin/countries/{country_id}</c>; no body, and its read's <c>ETag</c>.</summary>
    public Task<ApiResponse> HeadCountryAsync(
        object countryId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CountriesUrls.Country,
            new RequestOptions
            {
                PathParams = Id(countryId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>List one country's states beside the short parent country, with the query names
    /// <see cref="ListCountriesAsync"/> takes - <c>GET /admin/countries/{country_id}/states</c>.</summary>
    public Task<ApiResponse> ListCountryStatesAsync(
        object countryId,
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CountriesUrls.CountryStates,
            new RequestOptions
            {
                PathParams = Id(countryId),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CountryStatePage),
            }, cancellationToken);

    /// <summary>HEAD one country's states - <c>HEAD /admin/countries/{country_id}/states</c>; no body, and the
    /// state listing's <c>ETag</c>.</summary>
    public Task<ApiResponse> HeadCountryStatesAsync(
        object countryId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CountriesUrls.CountryStates,
            new RequestOptions
            {
                PathParams = Id(countryId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Retrieve one state of one country; <c>state_id</c> takes the state's <c>id</c>, <c>code</c> or
    /// <c>name</c>, in any case - <c>GET /admin/countries/{country_id}/states/{state_id}</c>.</summary>
    public Task<ApiResponse> GetCountryStateAsync(
        object countryId, object stateId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CountriesUrls.CountryState,
            new RequestOptions
            {
                PathParams = StateId(countryId, stateId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CountryState),
            }, cancellationToken);

    /// <summary>HEAD one state of one country - <c>HEAD /admin/countries/{country_id}/states/{state_id}</c>; no
    /// body, and its read's <c>ETag</c>.</summary>
    public Task<ApiResponse> HeadCountryStateAsync(
        object countryId, object stateId,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CountriesUrls.CountryState,
            new RequestOptions
            {
                PathParams = StateId(countryId, stateId),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Id(object countryId) =>
        new() { ["country_id"] = countryId };

    private static Dictionary<string, object?> StateId(object countryId, object stateId) =>
        new() { ["country_id"] = countryId, ["state_id"] = stateId };
}
