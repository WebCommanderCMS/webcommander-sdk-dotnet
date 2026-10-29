using System.Globalization;

using WebCommander.Auth;
using WebCommander.Config;
using WebCommander.Errors;
using WebCommander.Http;
using WebCommander.Resources.Aliases;
using WebCommander.Resources.BlogCategories;
using WebCommander.Resources.Blogs;
using WebCommander.Resources.Carts;
using WebCommander.Resources.Categories;
using WebCommander.Resources.Cities;
using WebCommander.Resources.CommonSettings;
using WebCommander.Resources.Countries;
using WebCommander.Resources.Currencies;
using WebCommander.Resources.CustomerGroups;
using WebCommander.Resources.Customers;
using WebCommander.Resources.DiscountProfiles;
using WebCommander.Resources.Email;
using WebCommander.Resources.InventoryLocations;
using WebCommander.Resources.OrderCustomFields;
using WebCommander.Resources.OrderFlags;
using WebCommander.Resources.Orders;
using WebCommander.Resources.Pages;
using WebCommander.Resources.PaymentGateways;
using WebCommander.Resources.Products;
using WebCommander.Resources.ShippingClasses;
using WebCommander.Resources.ShippingMethods;
using WebCommander.Resources.Settings.AnimationEffects;
using WebCommander.Resources.Settings.CustomerPortal;
using WebCommander.Resources.Settings.EcommerceSettings;
using WebCommander.Resources.Settings.PageSettings;
using WebCommander.Resources.Settings.PriceDisplaySettings;
using WebCommander.Resources.Settings.ProductListing;
using WebCommander.Resources.Settings.SearchSettings;
using WebCommander.Resources.Settings.SeoConfiguration;
using WebCommander.Resources.Settings.ShippingSettings;
using WebCommander.Resources.Settings.SiteSettings;
using WebCommander.Resources.TaxCodes;
using WebCommander.Resources.TaxCountryTaxes;
using WebCommander.Resources.TaxCustomProfiles;
using WebCommander.Resources.TaxProfiles;
using WebCommander.Resources.TaxRules;
using WebCommander.Resources.TaxSettings;
using WebCommander.Resources.TaxZones;
using WebCommander.Resources.Transactions;
using WebCommander.Resources.Trash;
using WebCommander.Resources.Variations;

namespace WebCommander.Client;

/// <summary>Authenticated access to one WebCommander store: config in, <see cref="ApiResponse"/> out.</summary>
public sealed class WebCommanderClient : IAsyncDisposable, IDisposable
{
    /// <summary>The SDK's logger name, prefixed to every log line by <see cref="FileLogger"/>.</summary>
    public const string LoggerName = "webcommander";

    private const string ApiVersionParam = "api_version";
    private const string TokenFailureError = "unauthorized";
    private static readonly IReadOnlySet<string> TokenFailureMessages =
        new HashSet<string> { "invalid_access_token", "invalid_request" };

    private readonly SdkConfig _config;
    private readonly Transport _transport;
    private readonly bool _ownsTransport;
    private readonly TokenProvider _tokenProvider;
    private readonly Action<string>? _log;
    private readonly FileLogger? _fileLog;
    private readonly int _logThreshold;

    public WebCommanderClient(SdkConfig config, Action<string>? onLog = null)
        : this(config, null, null, null, null, onLog)
    {
    }

    /// <summary>Build a client with injected collaborators; a null one is built from the config.</summary>
    public WebCommanderClient(
        SdkConfig config,
        ITokenStore? tokenStore,
        Transport? transport,
        RetryPolicy? retryPolicy,
        TokenProvider? tokenProvider,
        Action<string>? onLog = null)
    {
        if (transport is not null && retryPolicy is not null)
            throw new SdkUsageError("pass transport or retryPolicy, not both - an injected "
                + "transport already carries its own policy, so this one would be discarded "
                + "unused; build it into the transport instead, as new Transport(config, "
                + "retryPolicy)");
        if (tokenProvider is not null && tokenStore is not null)
            throw new SdkUsageError("pass tokenProvider or tokenStore, not both - an injected "
                + "token provider already owns the store it reads and writes, so this one would "
                + "be discarded unused; build it into the provider instead, as new "
                + "TokenProvider(config, transport, tokenStore)");
        _config = config;
        _log = onLog;
        _logThreshold = FileLogger.RankOf(config.LogLevel);
        _fileLog = FileLogger.Attach(config);
        try
        {
            if (!config.VerifySsl)
                EmitInsecureTransportWarning(InsecureTransportWarning.ForDisabledVerification().Message);
            WarnAboutPlaintextTransport();
            _ownsTransport = transport is null;
            _transport = transport ?? new Transport(config, retryPolicy, ReportProgress);
            var store = tokenStore ?? BuildTokenStore(config);
            _tokenProvider = tokenProvider ?? new TokenProvider(config, _transport, store);
        }
        catch
        {
            _fileLog?.Dispose();
            throw;
        }
    }

    private static ITokenStore BuildTokenStore(SdkConfig config)
    {
        var auth = config.Auth;
        return auth.TokenCachePath is null
            ? new MemoryTokenStore()
            : new FileTokenStore(
                auth.TokenCachePath,
                FileTokenStore.BuildCredentialsNamespace(
                    auth.BaseUrl, auth.ClientId, auth.ClientSecret, auth.AuthString, auth.GrantType),
                maxBytes: config.MaxTokenCacheBytes);
    }

    private static readonly IReadOnlySet<string> LoopbackHostnames =
        new HashSet<string> { "localhost", "localhost.localdomain" };

    /// <summary>Warn when the base URL is plain <c>http://</c> to another machine; never thrown.</summary>
    private void WarnAboutPlaintextTransport()
    {
        if (!Uri.TryCreate(_config.BaseUrl, UriKind.Absolute, out var parsed)
            || parsed.Scheme != "http" || IsLoopbackHost(parsed.Host))
            return;
        var warning = new InsecureTransportWarning(parsed.Host);
        EmitInsecureTransportWarning(warning.Message);
    }

    /// <summary>Warn on stderr as well as the log sinks, which shipped defaults leave unattached.</summary>
    private void EmitInsecureTransportWarning(string message)
    {
        if (InsecureTransportWarning.ClaimStandardErrorDelivery(message))
            Console.Error.WriteLine(message);
        EmitLog("WARNING", message);
    }

    private static bool IsLoopbackHost(string? host)
    {
        if (string.IsNullOrEmpty(host))
            return false;
        if (LoopbackHostnames.Contains(host.ToLowerInvariant()))
            return true;
        return System.Net.IPAddress.TryParse(host.Trim('[', ']'), out var address)
            && System.Net.IPAddress.IsLoopback(address);
    }

    /// <summary>Build a client for one instance: null for the first, an <c>int</c> position or a <c>clientName</c>.</summary>
    public static WebCommanderClient BuildClient(
        object? instance = null,
        Func<SdkConfig, SdkConfig>? configure = null,
        IEnumerable<object>? credentials = null) =>
        new(SdkConfig.FromCredentials(
            credentials ?? ClientRegistry.GetRegisteredInstances(), instance, configure));

    public SdkConfig Config => _config;

    public string Name => _config.ClientName;

    public IReadOnlyList<string> ListResourceNames() =>
        ResourceBinding.RegisteredResourceNames();

    private AdminCustomers? _adminCustomers;

    /// <summary>The admin customer endpoints, built on first use.</summary>
    public AdminCustomers AdminCustomers =>
        _adminCustomers ??= new AdminCustomers(this);

    private SeoConfiguration? _seoConfiguration;

    /// <summary>The SEO configuration settings endpoints, built on first use.</summary>
    public SeoConfiguration SeoConfiguration =>
        _seoConfiguration ??= new SeoConfiguration(this);

    private SearchResultPage? _searchResultPage;

    /// <summary>The search result page settings endpoints, built on first use.</summary>
    public SearchResultPage SearchResultPage =>
        _searchResultPage ??= new SearchResultPage(this);

    private CategoryPage? _categoryPage;

    /// <summary>The category page settings endpoints, built on first use.</summary>
    public CategoryPage CategoryPage =>
        _categoryPage ??= new CategoryPage(this);

    private ShoppingCart? _shoppingCart;

    /// <summary>The shopping cart settings endpoints, built on first use.</summary>
    public ShoppingCart ShoppingCart =>
        _shoppingCart ??= new ShoppingCart(this);

    private ProductPage? _productPage;

    /// <summary>The product page settings endpoints, built on first use.</summary>
    public ProductPage ProductPage =>
        _productPage ??= new ProductPage(this);

    private CheckoutPage? _checkoutPage;

    /// <summary>The checkout page settings endpoints, built on first use.</summary>
    public CheckoutPage CheckoutPage =>
        _checkoutPage ??= new CheckoutPage(this);

    private PriceDisplaySettings? _priceDisplaySettings;

    /// <summary>The price display settings endpoints, built on first use.</summary>
    public PriceDisplaySettings PriceDisplaySettings =>
        _priceDisplaySettings ??= new PriceDisplaySettings(this);

    private AnimationEffects? _animationEffects;

    /// <summary>The animation effects settings endpoints, built on first use.</summary>
    public AnimationEffects AnimationEffects =>
        _animationEffects ??= new AnimationEffects(this);

    private ProductListing? _productListing;

    /// <summary>The product listing settings endpoints, built on first use.</summary>
    public ProductListing ProductListing =>
        _productListing ??= new ProductListing(this);

    private CustomerPortal? _customerPortal;

    /// <summary>The customer portal settings endpoints, built on first use.</summary>
    public CustomerPortal CustomerPortal =>
        _customerPortal ??= new CustomerPortal(this);

    private SiteSettings? _siteSettings;

    /// <summary>The site settings endpoints, built on first use.</summary>
    public SiteSettings SiteSettings =>
        _siteSettings ??= new SiteSettings(this);

    private ShippingSettings? _shippingSettings;

    /// <summary>The shipping settings endpoints, built on first use.</summary>
    public ShippingSettings ShippingSettings =>
        _shippingSettings ??= new ShippingSettings(this);

    private SearchSettings? _searchSettings;

    /// <summary>The search settings endpoints, built on first use.</summary>
    public SearchSettings SearchSettings =>
        _searchSettings ??= new SearchSettings(this);

    private EcommerceSettings? _ecommerceSettings;

    /// <summary>The ecommerce settings endpoints, built on first use.</summary>
    public EcommerceSettings EcommerceSettings =>
        _ecommerceSettings ??= new EcommerceSettings(this);

    private Email? _email;

    /// <summary>The email send endpoint, built on first use.</summary>
    public Email Email =>
        _email ??= new Email(this);

    private OrderFlags? _orderFlags;

    /// <summary>The order flag endpoints, built on first use.</summary>
    public OrderFlags OrderFlags =>
        _orderFlags ??= new OrderFlags(this);

    private InventoryLocations? _inventoryLocations;

    /// <summary>The inventory location endpoints, built on first use.</summary>
    public InventoryLocations InventoryLocations =>
        _inventoryLocations ??= new InventoryLocations(this);

    private ShippingClasses? _shippingClasses;

    /// <summary>The shipping class endpoints, built on first use.</summary>
    public ShippingClasses ShippingClasses =>
        _shippingClasses ??= new ShippingClasses(this);

    private ShippingMethods? _shippingMethods;

    /// <summary>The shipping method endpoints, built on first use.</summary>
    public ShippingMethods ShippingMethods =>
        _shippingMethods ??= new ShippingMethods(this);

    private TaxCodes? _taxCodes;

    /// <summary>The tax code endpoints, built on first use.</summary>
    public TaxCodes TaxCodes =>
        _taxCodes ??= new TaxCodes(this);

    private TaxProfiles? _taxProfiles;

    /// <summary>The tax profile endpoints, built on first use.</summary>
    public TaxProfiles TaxProfiles =>
        _taxProfiles ??= new TaxProfiles(this);

    private TaxRules? _taxRules;

    /// <summary>The tax rule endpoints, built on first use.</summary>
    public TaxRules TaxRules =>
        _taxRules ??= new TaxRules(this);

    private TaxZones? _taxZones;

    /// <summary>The tax zone endpoints, built on first use.</summary>
    public TaxZones TaxZones =>
        _taxZones ??= new TaxZones(this);

    private TaxSettings? _taxSettings;

    /// <summary>The tax settings facade's endpoints, built on first use.</summary>
    public TaxSettings TaxSettings =>
        _taxSettings ??= new TaxSettings(this);

    private TaxCountryTaxes? _taxCountryTaxes;

    /// <summary>The country tax endpoints, built on first use.</summary>
    public TaxCountryTaxes TaxCountryTaxes =>
        _taxCountryTaxes ??= new TaxCountryTaxes(this);

    private TaxCustomProfiles? _taxCustomProfiles;

    /// <summary>The custom tax profile endpoints, built on first use.</summary>
    public TaxCustomProfiles TaxCustomProfiles =>
        _taxCustomProfiles ??= new TaxCustomProfiles(this);

    private Aliases? _aliases;

    /// <summary>The domain alias endpoints, built on first use.</summary>
    public Aliases Aliases =>
        _aliases ??= new Aliases(this);

    private BlogCategories? _blogCategories;

    /// <summary>The blog category endpoints, built on first use.</summary>
    public BlogCategories BlogCategories =>
        _blogCategories ??= new BlogCategories(this);

    private Blogs? _blogs;

    /// <summary>The blog and blog comment endpoints, built on first use.</summary>
    public Blogs Blogs =>
        _blogs ??= new Blogs(this);

    private Carts? _carts;

    /// <summary>The storefront cart endpoints and the order write a cart turns into, built on first use.</summary>
    public Carts Carts =>
        _carts ??= new Carts(this);

    private Categories? _categories;

    /// <summary>The product category endpoints, built on first use. Not <see cref="CategoryPage"/>,
    /// the category page settings area.</summary>
    public Categories Categories =>
        _categories ??= new Categories(this);

    private Cities? _cities;

    /// <summary>The storefront city lookup under <c>/app</c>, built on first use.</summary>
    public Cities Cities =>
        _cities ??= new Cities(this);

    private CommonSettings? _commonSettings;

    /// <summary>The store-wide settings and registration form endpoints, not under <c>/admin</c>, built on first
    /// use. Not one of the settings areas.</summary>
    public CommonSettings CommonSettings =>
        _commonSettings ??= new CommonSettings(this);

    private Countries? _countries;

    /// <summary>The country and state endpoints, built on first use.</summary>
    public Countries Countries =>
        _countries ??= new Countries(this);

    private Currencies? _currencies;

    /// <summary>The currency endpoints, built on first use.</summary>
    public Currencies Currencies =>
        _currencies ??= new Currencies(this);

    private CustomerGroups? _customerGroups;

    /// <summary>The customer group endpoints, built on first use.</summary>
    public CustomerGroups CustomerGroups =>
        _customerGroups ??= new CustomerGroups(this);

    private DiscountProfiles? _discountProfiles;

    /// <summary>The discount profile and coupon endpoints, built on first use.</summary>
    public DiscountProfiles DiscountProfiles =>
        _discountProfiles ??= new DiscountProfiles(this);

    private OrderCustomFields? _orderCustomFields;

    /// <summary>The order custom field endpoints, built on first use.</summary>
    public OrderCustomFields OrderCustomFields =>
        _orderCustomFields ??= new OrderCustomFields(this);

    private Orders? _orders;

    /// <summary>The order endpoints, built on first use.</summary>
    public Orders Orders =>
        _orders ??= new Orders(this);

    private Pages? _pages;

    /// <summary>The page endpoints, built on first use.</summary>
    public Pages Pages =>
        _pages ??= new Pages(this);

    private PaymentGateways? _paymentGateways;

    /// <summary>The payment gateway endpoints, built on first use.</summary>
    public PaymentGateways PaymentGateways =>
        _paymentGateways ??= new PaymentGateways(this);

    private Products? _products;

    /// <summary>The product row and stock endpoints, built on first use.</summary>
    public Products Products =>
        _products ??= new Products(this);

    private Transactions? _transactions;

    /// <summary>The transaction-number endpoints, built on first use.</summary>
    public Transactions Transactions =>
        _transactions ??= new Transactions(this);

    private Variations? _variations;

    /// <summary>The variation type and product variant endpoints, built on first use.</summary>
    public Variations Variations =>
        _variations ??= new Variations(this);

    private Trash? _trash;

    /// <summary>The trash endpoints, built on first use.</summary>
    public Trash Trash =>
        _trash ??= new Trash(this);

    /// <summary>Send one request and return the response; every argument is on the options.</summary>
    public async Task<ApiResponse> SendApiRequestAsync(
        string httpMethod, string path, RequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        RefuseIfClosed();
        var resolvedOptions = options ?? RequestOptions.None;
        var method = Transport.NormaliseMethod(httpMethod);
        ValidatePath(path);
        var (jsonBody, jsonBodyBytes) = ValidatePayload(resolvedOptions);
        var responseFormat = ResolveResponseFormat(resolvedOptions.ResponseFormat);

        var resolvedPath = UrlBuilder.SubstitutePathParams(path, SeedApiVersion(path, resolvedOptions.PathParamsAsSent));
        var url = UrlBuilder.BuildUrl(_config.BaseUrl, resolvedPath);
        var queryParams = QueryParams.BuildQueryParams(resolvedOptions.QueryParamsAsSent);
        RejectDuplicatedQueryParams(url, queryParams);

        var reportedBody = Redaction.Detach(
            ReportableBody(jsonBody, jsonBodyBytes) ?? resolvedOptions.FormDataAsSent);
        var canonical = new TransportRequest(method, url)
        {
            QueryParams = queryParams,
            EncodedJsonBody = (jsonBody, jsonBodyBytes),
            FormData = resolvedOptions.FormDataAsSent,
            Files = resolvedOptions.FilesAsSent,
            RequestInfoBody = Redaction.RedactValue(
                ExchangeConsole.SummarisePayload(reportedBody, resolvedOptions.FilesAsSent)),
            ResponseFormat = responseFormat,
            DtoType = resolvedOptions.DtoType,
        };
        Transport.CheckAssembledUrl(canonical);
        ValidateCallerHeaders(resolvedOptions.Headers);
        RefuseContentHeadersWithNoBody(method, resolvedOptions, jsonBody);
        RefuseContentTypeOnMultipart(resolvedOptions);

        await canonical.PrepareAsync(_config, method, cancellationToken).ConfigureAwait(false);

        var accessToken = resolvedOptions.Authenticated
            ? await _tokenProvider.GetAccessTokenAsync(cancellationToken).ConfigureAwait(false)
            : null;

        var response = await SendOnceAsync(canonical, resolvedOptions, accessToken, reportedBody, cancellationToken)
            .ConfigureAwait(false);

        if (resolvedOptions.Authenticated && MeansExpiredToken(response, method))
        {
            if (canonical.ReplaySafe)
            {
                ReportExchange(response, reportedBody, resolvedOptions, warning: true);
                var fresh = await _tokenProvider.RefreshAccessTokenAsync(accessToken, cancellationToken).ConfigureAwait(false);
                response = await SendOnceAsync(canonical, resolvedOptions, fresh, reportedBody, cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                EmitLog("WARNING", Redaction.RedactMessageText(
                    $"{method} {Redaction.UrlForLog(url)} -> 401 with an expired token, and the "
                    + "request was not replayed: it carries an upload stream larger than "
                    + "maxRequestSpoolBytes, so this SDK does not hold its bytes and cannot send "
                    + "the same request twice"));
            }
        }
        ReportExchange(response, reportedBody, resolvedOptions, warning: false);

        var shouldRaise = resolvedOptions.RaiseOnError ?? _config.RaiseOnError;
        return shouldRaise ? response.RaiseForStatus() : response;
    }

    /// <summary>Send a <c>GET</c>, with no options. <paramref name="path"/> may be a template.</summary>
    public Task<ApiResponse> SendGetRequestAsync(string path, CancellationToken cancellationToken = default) =>
        SendApiRequestAsync("GET", path, null, cancellationToken);

    public Task<ApiResponse> SendGetRequestAsync(string path, RequestOptions? options, CancellationToken cancellationToken = default) =>
        SendApiRequestAsync("GET", path, options, cancellationToken);

    public Task<ApiResponse> SendPostRequestAsync(string path, RequestOptions? options = null, CancellationToken cancellationToken = default) =>
        SendApiRequestAsync("POST", path, options, cancellationToken);

    public Task<ApiResponse> SendPutRequestAsync(string path, RequestOptions? options = null, CancellationToken cancellationToken = default) =>
        SendApiRequestAsync("PUT", path, options, cancellationToken);

    public Task<ApiResponse> SendPatchRequestAsync(string path, RequestOptions? options = null, CancellationToken cancellationToken = default) =>
        SendApiRequestAsync("PATCH", path, options, cancellationToken);

    public Task<ApiResponse> SendDeleteRequestAsync(string path, RequestOptions? options = null, CancellationToken cancellationToken = default) =>
        SendApiRequestAsync("DELETE", path, options, cancellationToken);

    private volatile bool _closed;

    /// <summary>Refuse a call on a closed client before a token grant is spent on it.</summary>
    private void RefuseIfClosed()
    {
        if (_closed)
            throw new SdkUsageError(Redaction.RedactMessageText(
                $"client '{_config.ClientName}' is closed: CloseClient() released its log file and "
                + "any connection pool it owned, so this request was not sent - scope it with `await using` "
                + "around the work that needs it, and build a new client rather than reusing a "
                + "closed one"));
    }

    /// <summary>An alias for <see cref="Dispose"/>, not a second teardown path.</summary>
    public void CloseClient() => Dispose();

    /// <summary>Release what this client built; an injected transport is the caller's and stays open.</summary>
    public void Dispose()
    {
        // Set first, so a racing send is refused rather than reaching a half-torn-down transport.
        _closed = true;
        if (_ownsTransport)
            _transport.Dispose();
        _fileLog?.Dispose();
    }

    /// <summary>The same teardown as <see cref="Dispose"/>, which is synchronous.</summary>
    public ValueTask DisposeAsync()
    {
        Dispose();
        return default;
    }

    /// <summary>One attempt at the snapshotted request; only the headers differ on a replay.</summary>
    private async Task<ApiResponse> SendOnceAsync(
        TransportRequest canonical, RequestOptions options, string? accessToken,
        object? reportedBody, CancellationToken cancellationToken)
    {
        var headers = BuildRequestHeaders(options.Headers, accessToken);
        try
        {
            return await _transport.SendHttpRequestAsync(
                    canonical.WithHeaders(headers, Redaction.RedactMapping(headers)), cancellationToken)
                .ConfigureAwait(false);
        }
        catch (SdkError error) when (error.RequestInfo is RequestInfo info)
        {
            ReportFailedExchange(info, reportedBody, options);
            throw;
        }
    }

    /// <summary>Merge headers case-insensitively; the bearer token is applied last and cannot be displaced.</summary>
    internal Dictionary<string, string> BuildRequestHeaders(
        IReadOnlyDictionary<string, string>? callerHeaders, string? accessToken)
    {
        var spellingByLowerName = new Dictionary<string, string>();
        var valueByLowerName = new Dictionary<string, string>();

        PutHeader(spellingByLowerName, valueByLowerName, "Accept", "application/json");
        PutHeader(spellingByLowerName, valueByLowerName, "User-Agent", _config.UserAgent);
        foreach (var (name, value) in _config.DefaultHeaders)
            PutHeader(spellingByLowerName, valueByLowerName, name, value);
        if (callerHeaders is not null)
            foreach (var (name, value) in callerHeaders)
                PutHeader(spellingByLowerName, valueByLowerName, name, value);
        if (accessToken is not null)
            PutHeader(spellingByLowerName, valueByLowerName, "Authorization", "Bearer " + accessToken);
        var merged = new Dictionary<string, string>();
        foreach (var (lower, value) in valueByLowerName)
            merged[spellingByLowerName[lower]] = value;
        return merged;
    }

    /// <summary>Refuse, before the token grant, a caller header that would inject or be mangled.</summary>
    private static void ValidateCallerHeaders(IReadOnlyDictionary<string, string>? callerHeaders)
    {
        if (callerHeaders is null)
            return;
        foreach (var (name, value) in callerHeaders)
            ValidateCallerHeader(name, value);
    }

    /// <summary>The name is shown, neutralised; the value never is, as it is routinely a credential.</summary>
    private static void ValidateCallerHeader(string name, string value)
    {
        if (Transport.IsSendableHeader(name, value))
            return;
        throw new SdkUsageError(Redaction.RedactMessageText(
            $"headers: '{ExchangeConsole.NeutraliseOneLine(name)}' cannot be sent - "
            + Transport.HeaderGrammar
            + ". The value is not shown: it may be a credential"));
    }

    /// <summary>Refuse a caller's <c>Content-*</c> header on a call that carries no body.</summary>
    private static void RefuseContentHeadersWithNoBody(
        string method, RequestOptions options, object? jsonBody)
    {
        if (options.Headers is null || options.Headers.Count == 0)
            return;
        if (Transport.CarriesABody(method, jsonBody, options.FormDataAsSent, options.FilesAsSent))
            return;
        var named = options.Headers.Keys
            .Where(Transport.IsContentHeader)
            .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
            .Select(ExchangeConsole.NeutraliseOneLine)
            .ToList();
        if (named.Count == 0)
            return;
        throw new SdkUsageError(Redaction.RedactMessageText(
            $"headers: {string.Join(", ", named)} describe a body, and this {method} carries none, "
            + "so nothing would be sent under them. Send the body the header describes - jsonBody, "
            + "formData or files - or drop the header; a Content-Type on a request with no content "
            + "has no meaning to the store"));
    }

    /// <summary>Refuse a caller's <c>Content-Type</c> on multipart: it carries the encoder's boundary.</summary>
    private static void RefuseContentTypeOnMultipart(RequestOptions options)
    {
        if (options.FilesAsSent is not { Count: > 0 } || options.Headers is null)
            return;
        var named = options.Headers.Keys
            .Where(name => string.Equals(name, "Content-Type", StringComparison.OrdinalIgnoreCase))
            .Select(ExchangeConsole.NeutraliseOneLine)
            .ToList();
        if (named.Count == 0)
            return;
        throw new SdkUsageError(Redaction.RedactMessageText(
            $"headers: {string.Join(", ", named)} cannot be set on a call that sends files - a "
            + "multipart body's Content-Type carries the boundary its parts are delimited by, "
            + "which this SDK chooses when the request is built, so a replacement would describe "
            + "a framing the body does not have. Drop the header; the encoder sets "
            + "multipart/form-data with the right boundary. To send a body under a content type "
            + "of your own, send it as jsonBody or formData instead of files"));
    }

    private static void PutHeader(
        Dictionary<string, string> spellingByLowerName, Dictionary<string, string> valueByLowerName,
        string name, string value)
    {
        var lower = name.ToLowerInvariant();
        if (!spellingByLowerName.ContainsKey(lower))
            spellingByLowerName[lower] = name;
        if (lower is "authorization" or "accept" or "user-agent")
            spellingByLowerName[lower] = lower switch { "authorization" => "Authorization", "accept" => "Accept", _ => "User-Agent" };
        valueByLowerName[lower] = value;
    }

    /// <summary>A listing page's absolute URL, for cycle state: compared, never printed.</summary>
    internal string ResolvePageUrl(string path) =>
        UrlBuilder.BuildUrl(_config.BaseUrl, UrlBuilder.SubstitutePathParams(path, SeedApiVersion(path, null)));

    private IReadOnlyDictionary<string, object?> SeedApiVersion(
        string path, IReadOnlyDictionary<string, object?>? pathParams)
    {
        var resolved = pathParams is null
            ? new Dictionary<string, object?>()
            : new Dictionary<string, object?>(pathParams);
        if (UrlBuilder.FindPathParamNames(path).Contains(ApiVersionParam) && !resolved.ContainsKey(ApiVersionParam))
            resolved[ApiVersionParam] = _config.ApiVersion;
        return resolved;
    }

    private static void ValidatePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new SdkUsageError("path must name an endpoint - an empty path requests the "
                + "store root, which answers 200 with the storefront");
    }

    /// <summary>Refuse an unsendable payload before any I/O; returns the body and its one encoding.</summary>
    private static (object? Body, byte[]? Bytes) ValidatePayload(RequestOptions options)
    {
        if (options.JsonBody is not null && options.FormDataAsSent is not null)
            throw new SdkUsageError("pass jsonBody or formData, not both - they are different "
                + "content types and only one can be sent");
        if (options.JsonBody is not null && options.FilesAsSent is not null)
            throw new SdkUsageError("pass jsonBody or files, not both - a multipart body "
                + "replaces the JSON one, so the JSON would be discarded unsent");
        ValidateMultipartFieldNames(options);
        if (options.JsonBody is null)
            return (null, null);
        // JSON text, not a value: serialising would make any string a JSON string literal.
        if (options.JsonBody is string jsonText)
        {
            ValidateJsonText(jsonText);
            return (jsonText, Transport.EncodeJsonBody(jsonText));
        }
        var body = options.JsonBody;
        if (body is Dto payload)
        {
            body = payload.ToMap()
                ?? throw new SdkUsageError(Redaction.RedactMessageText(
                    $"jsonBody: {payload.GetType().Name}.ToMap() returned null; return the "
                    + "payload map, or pass no body at all"));
        }
        var encoded = Transport.EncodeJsonBody(body)!;
        var head = 0;
        while (head < encoded.Length && encoded[head] is 0x20 or 0x09 or 0x0A or 0x0D)
            head++;
        if (head >= encoded.Length || (encoded[head] != (byte)'{' && encoded[head] != (byte)'['))
            throw new SdkUsageError(Redaction.RedactMessageText("jsonBody must be a JSON object or array - got "
                + body.GetType().Name + ", which would be sent as a bare JSON scalar. "
                + "Wrap it in the payload structure the endpoint expects"));
        return (body, encoded);
    }

    /// <summary>The body as the record and printout describe it, read back from the wire bytes.</summary>
    private static object? ReportableBody(object? jsonBody, byte[]? jsonBodyBytes)
    {
        if (jsonBody is null || jsonBodyBytes is null)
            return null;
        if (jsonBody is string)
            return jsonBody;
        using var document = System.Text.Json.JsonDocument.Parse(jsonBodyBytes, Redaction.DeepParse);
        return Redaction.Materialise(document.RootElement);
    }

    /// <summary>Settle one call's <c>Data</c> form, refusing an unknown value before any I/O.</summary>
    private string ResolveResponseFormat(string? responseFormat)
    {
        if (responseFormat is null)
            return _config.ResponseFormat;
        var resolved = SdkConfig.Normalise(responseFormat);
        if (resolved is null || !SdkConfig.IsResponseFormat(resolved))
            throw new SdkUsageError(Redaction.RedactMessageText(
                "responseFormat: expected one of " + string.Join(", ", SdkConfig.ResponseFormatValues)
                + $", got '{ExchangeConsole.NeutraliseOneLine(responseFormat)}'"));
        return resolved;
    }

    /// <summary>Verify raw JSON text holds an object or array; parsed only to verify, never rewritten.</summary>
    private static void ValidateJsonText(string jsonText)
    {
        System.Text.Json.JsonValueKind kind;
        try
        {
            using var parsed = System.Text.Json.JsonDocument.Parse(jsonText, Redaction.DeepParse);
            kind = parsed.RootElement.ValueKind;
        }
        catch (System.Text.Json.JsonException error)
        {
            throw new SdkUsageError(Redaction.RedactMessageText(
                "jsonBody: not valid JSON: " + error.Message), error);
        }
        if (kind is System.Text.Json.JsonValueKind.Object or System.Text.Json.JsonValueKind.Array)
            return;
        throw new SdkUsageError(Redaction.RedactMessageText(
            "jsonBody: a JSON string must hold an object or an array, got " + DescribeJsonKind(kind)
            + " - wrap it in the payload structure the endpoint expects"));
    }

    /// <summary>A refused root value named in JSON's vocabulary, since the caller wrote JSON text.</summary>
    private static string DescribeJsonKind(System.Text.Json.JsonValueKind kind) => kind switch
    {
        System.Text.Json.JsonValueKind.String => "a JSON string",
        System.Text.Json.JsonValueKind.Number => "a JSON number",
        System.Text.Json.JsonValueKind.True or System.Text.Json.JsonValueKind.False => "a JSON boolean",
        System.Text.Json.JsonValueKind.Null => "JSON null",
        _ => "a bare JSON value",
    };

    /// <summary>Refuse a <c>formData</c> name a multipart part could not carry.</summary>
    private static void ValidateMultipartFieldNames(RequestOptions options)
    {
        if (options.FilesAsSent is not { Count: > 0 } || options.FormDataAsSent is null)
            return;
        foreach (var name in options.FormDataAsSent.Keys)
            if (Transport.ProblemWithMultipartFieldName(name) is { } problem)
                throw problem;
    }

    /// <summary>Refuse a query name the URL already carries, as an absolute <c>next_page</c> does.</summary>
    private static void RejectDuplicatedQueryParams(string url, IReadOnlyDictionary<string, object?> queryParams)
    {
        var mark = url.IndexOf('?');
        if (queryParams.Count == 0 || mark < 0)
            return;
        var existing = UrlBuilder.QueryPairs(url[(mark + 1)..]).Select(pair => pair.Name).ToHashSet();
        var clash = queryParams.Keys.Where(existing.Contains).OrderBy(key => key, StringComparer.Ordinal).ToList();
        if (clash.Count > 0)
            throw new SdkUsageError(Redaction.RedactMessageText("query params " + string.Join(", ", clash)
                + " already appear in the path's own query string, so each would be sent twice with "
                + "two values and the store chooses - pass them once, in one place"));
    }

    /// <summary>Whether a 401 means the token; this API also answers 401 to a business rejection.</summary>
    internal static bool MeansExpiredToken(ApiResponse response, string method)
    {
        if (response.StatusCode != 401)
            return false;
        if (response.Map is not IReadOnlyDictionary<string, object?> body)
            return RetryPolicy.IsSafeMethod(method);
        if (TokenFailureError.Equals(NormalisedReason(body.GetValueOrDefault("error"))))
            return true;
        return TokenFailureMessages.Contains(NormalisedReason(body.GetValueOrDefault("message")));
    }

    private static string NormalisedReason(object? value) =>
        value is null ? "" : (Convert.ToString(value, CultureInfo.InvariantCulture) ?? "").Trim().ToLowerInvariant();

    /// <summary>One record to every sink, gated by <c>logLevel</c> first.</summary>
    private void EmitLog(string level, string message)
    {
        if (FileLogger.RankOf(level) < _logThreshold)
            return;
        _log?.Invoke(level + " " + message);
        _fileLog?.Log(level, message);
    }

    private static string RenderForLog(object? body) => Display.RenderBody(Redaction.RedactValue(body));

    private static string ExchangeLogLine(RequestInfo info, string status) =>
        $"{info.HttpMethod} {Redaction.UrlForLog(info.Url)} -> {status} in "
        + $"{Display.Seconds(info.ElapsedSeconds)}s "
        + $"(attempt {info.AttemptCount}/{info.MaxAttempts})";

    private void ReportProgress(string message)
    {
        if (_config.PrintApiInfo && !_config.PrintOnlyOnError)
            Console.WriteLine(message);
        EmitLog("INFO", message);
    }

    /// <summary>Print and log a send that got no response, whatever <c>printOnlyOnError</c> says.</summary>
    private void ReportFailedExchange(RequestInfo info, object? reportedBody, RequestOptions options)
    {
        if (WantsPrint())
            Console.WriteLine(ExchangeConsole.RenderFailedExchange(
                info, reportedBody, _config, options.FilesAsSent, IsFormEncoded(options)));
        EmitLog("ERROR", ExchangeLogLine(info, ExchangeConsole.NoResponseStatus));
        if (_config.LogBodies)
            EmitLog("DEBUG", $"payload={RenderForLog(reportedBody)} response=");
    }

    private bool WantsPrint() =>
        _config.PrintApiInfo || _config.PrintRequestPayload
        || _config.PrintResponseBody || _config.PrintHeaders || _config.PrintCurl;

    /// <summary>Whether the body went as form data; the curl line cannot tell from the headers.</summary>
    private static bool IsFormEncoded(RequestOptions options) =>
        options.JsonBody is null && options.FormDataAsSent is not null;

    private void ReportExchange(ApiResponse response, object? reportedBody, RequestOptions options, bool warning)
    {
        var info = response.RequestInfo;
        if (info is not null && WantsPrint() && !(_config.PrintOnlyOnError && response.Succeeded))
            Console.WriteLine(ExchangeConsole.RenderExchange(
                response, reportedBody, _config, options.FilesAsSent, IsFormEncoded(options)));

        if (info is not null)
            EmitLog(warning ? "WARNING" : "INFO",
                ExchangeLogLine(info, response.StatusCode.ToString(CultureInfo.InvariantCulture)));
        if (_config.LogBodies)
            EmitLog("DEBUG",
                $"payload={RenderForLog(reportedBody)} response={RenderForLog(response.Map)}");
    }
}
