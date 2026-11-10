using WebCommander.Client;
using WebCommander.Resources.DashboardSections.DashboardSectionsDto;

namespace WebCommander.Resources.DashboardSections;

/// <summary>Dashboard sections - <c>/admin/dashboard/sections/{section}</c> - as a typed resource bound to one client.
/// The Dashboard shows five tabs; a tab is a path value.</summary>
public sealed class DashboardSections : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.DashboardSections</c>.</summary>
    public DashboardSections(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>Read one tab - <c>GET /admin/dashboard/sections/{section}</c>, <c>section</c> being one of
    /// <see cref="DashboardSectionsUrls.DashboardSections"/> (<c>ecommerce</c>, <c>content</c>, <c>bookings</c>,
    /// <c>courses</c>, <c>blog</c>); the answer is bare, <c>section</c> echoing it, with <c>period</c> and that tab's own
    /// fields. Any other value is <c>404</c> <c>not_found</c> on <c>key</c>. <c>duration</c> or <c>start_date</c> with
    /// <c>end_date</c> choose the period, as in the reports.</summary>
    public Task<ApiResponse> GetDashboardSectionAsync(
        string section, IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(DashboardSectionsUrls.DashboardSection,
            new RequestOptions
            {
                PathParams = new Dictionary<string, object?> { ["section"] = section },
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(DashboardSectionReport),
            }, cancellationToken);
}
