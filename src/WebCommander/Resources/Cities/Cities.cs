using WebCommander.Client;
using WebCommander.Resources.Cities.CitiesDto;

namespace WebCommander.Resources.Cities;

/// <summary>The storefront city lookup - <c>/app/cities</c> - as a typed resource bound to one client.</summary>
public sealed class Cities : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Cities</c>.</summary>
    public Cities(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the cities, by state, post code and name unless <c>sort</c> says otherwise -
    /// <c>GET /app/cities</c>. <c>limit</c> is the page size, 1 to 100 and 100 when absent; <c>offset</c> the
    /// start. <c>country</c> takes a country id or two-letter code, <c>state</c> a state id or, beside
    /// <c>country</c>, a state code; <c>post_code</c> matches exactly and <c>search</c> a substring of
    /// <c>name</c> in any case. <c>sort</c> is <c>state</c>, <c>name</c> or <c>post_code</c>, <c>order</c>
    /// <c>asc</c> or <c>desc</c>.</summary>
    public Task<ApiResponse> ListCitiesAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(CitiesUrls.Cities,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(CityPage),
            }, cancellationToken);

    /// <summary>HEAD the city listing - <c>HEAD /app/cities</c>; the listing's headers, its <c>ETag</c> among
    /// them, and no body.</summary>
    public Task<ApiResponse> HeadCitiesAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", CitiesUrls.Cities,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);
}
