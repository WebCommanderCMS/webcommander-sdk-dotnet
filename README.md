# WebCommander .NET SDK

A .NET client for the WebCommander v4 REST API, with OAuth2 token handling, typed resources, retries
and request diagnostics built in.

## Requirements

- .NET SDK 8.0.100 or newer (`dotnet --version`).
- Target framework `net8.0`.
- No runtime dependencies: the SDK uses `HttpClient` and `System.Text.Json` from the framework.

## Installation

Install the package from NuGet:

```bash
dotnet add package WebCommander
```

## Quick start

The SDK does not read credentials from the environment. Load them from your own configuration or secret
store and pass them to `AuthConfig`:

```csharp
using WebCommander.Client;
using WebCommander.Config;

var auth = new AuthConfig(
    baseUrl:      "https://your-instance.mywebcommander.com",
    clientId:     "your-client-id",
    clientSecret: "your-client-secret",
    redirectUri:  "https://example.com/callback",
    authString:   "your-auth-string",
    clientName:   "default");

await using var client = new WebCommanderClient(new SdkConfig(auth, raiseOnError: true));

var response = await client.AdminCustomers.GetCustomersAsync(
    new Dictionary<string, object?> { ["limit"] = 5 });

var body = (IReadOnlyDictionary<string, object?>)response.Map!;
foreach (var row in (IEnumerable<object?>)body["customers"]!)
{
    var customer = (IReadOnlyDictionary<string, object?>)row!;
    Console.WriteLine($"{customer["customer_id"]}  {customer["email"]}");
}
```

`authString` is the base64 encoding of `operator-email:password`. The client fetches and renews the
OAuth2 token for you. `raiseOnError: true` makes a failed call throw; without it, a failed call
returns a response with `Succeeded == false`.

## Configuration

`AuthConfig` holds the credentials for one store. `SdkConfig` holds everything else, and every
setting except the `AuthConfig` is an optional named argument:

```csharp
var config = new SdkConfig(auth,
    timeoutSeconds: 60,
    maxRetries: 3,
    printApiInfo: true);

var debug = config.WithOverrides(logLevel: "DEBUG", printCurl: true);
```

Both types are immutable and validated at construction; a validation error lists every problem at
once. `WithOverrides` returns a new config. To talk to several stores, register several
`AuthConfig` blocks and select one by name or index.

## Usage

### Bound and explicit calls

Each resource is a property on the client, with one method per endpoint. Every method takes an
optional `raiseOnError`, a `CancellationToken` and an optional `responseFormat`:

```csharp
await client.Orders.ListOrdersAsync(new Dictionary<string, object?> { ["limit"] = 20 });
await client.AdminCustomers.GetCustomerAsync(1192, raiseOnError: true, cancellationToken: ct);
```

Register your instances once and build a client anywhere by name, without passing credentials:

```csharp
ClientRegistry.RegisterInstances(new object[] { auth });

await using var client = WebCommanderClient.BuildClient("default");
```

The raw senders reach any path, including endpoints without a typed method:

```csharp
await client.SendGetRequestAsync("/api/{api_version}/admin/products",
    new RequestOptions { QueryParams = new Dictionary<string, object?> { ["limit"] = 20 } });
```

### Payload forms

A request body can be a map, a string of JSON (sent byte for byte) or a payload DTO:

```csharp
using WebCommander.Resources.Customers.AdminCustomersDto;

await client.AdminCustomers.CreateCustomerAsync(new Dictionary<string, object?>
{
    ["customer"] = new Dictionary<string, object?> { ["first_name"] = "Jane", ["email"] = "jane@example.com" },
});

await client.AdminCustomers.CreateCustomerAsync(
    """{"customer":{"first_name":"Jane","email":"jane@example.com"}}""");

await client.AdminCustomers.CreateCustomerAsync(
    new CustomerInput { FirstName = "Jane", Email = "jane@example.com" });
```

### Response forms

Every call returns an `ApiResponse`. `.Map` is the decoded body, `.Json` is the body text as the store
sent it, and `.Dto` is the typed view. `.Data` is whichever form `responseFormat` selects (`"map"` by
default):

```csharp
var response = await client.AdminCustomers.GetCustomerAsync(1192);

var customer = (Customer)response.Dto!;
Console.WriteLine(customer.Email);
```

Keys are never renamed: a key you write is the key that is sent, and a key the store sends is the key
you read.

### Pagination

`Pagination.IterRecordsAsync` follows `pagination.next_page` for you:

```csharp
await foreach (var customer in Pagination.IterRecordsAsync(client,
    "/api/{api_version}/admin/customers", "customers"))
{
    Console.WriteLine(customer["customer_id"]);
}
```

Most listings serve at most 20 rows per page whatever `limit` asks for, so reading a whole collection
always takes several requests.

## Error handling

By default a failed call returns an `ApiResponse` with `Succeeded == false`. Pass `raiseOnError: true`
on the config or on one call to throw an `SdkError` instead:

```csharp
using WebCommander.Errors;

try
{
    await client.AdminCustomers.GetCustomerAsync(999999, raiseOnError: true);
}
catch (SdkError error)
{
    Console.WriteLine(error.Summary());
    Console.WriteLine($"{error.StatusCode} {error.ErrorType}");
    foreach (var message in error.GetErrorMessages())
        Console.WriteLine(message);
}
```

`SdkConfigError`, `SdkUsageError` and `SdkAuthError` derive from `SdkError`, so one
`catch (SdkError)` covers every failure the SDK throws. A `200` whose body carries a false
`isSuccess` (the store's answer to a mistyped path) counts as a failure.

## Resources

The client exposes 46 resources with 349 operations. The [API reference](https://newlify.mintlify.app)
documents the method, path and behaviour of each one.

| Resource | Client accessor | Operations |
|---|---|---:|
| Customers | `client.AdminCustomers` | 15 |
| Aliases | `client.Aliases` | 4 |
| Animation effects settings | `client.AnimationEffects` | 5 |
| Blog categories | `client.BlogCategories` | 5 |
| Blogs | `client.Blogs` | 12 |
| Carts | `client.Carts` | 7 |
| Categories | `client.Categories` | 10 |
| Category page settings | `client.CategoryPage` | 5 |
| Checkout page settings | `client.CheckoutPage` | 5 |
| Cities | `client.Cities` | 2 |
| Common settings | `client.CommonSettings` | 6 |
| Countries | `client.Countries` | 8 |
| Currencies | `client.Currencies` | 11 |
| Customer groups | `client.CustomerGroups` | 9 |
| Customer portal settings | `client.CustomerPortal` | 5 |
| Discount profiles | `client.DiscountProfiles` | 7 |
| Ecommerce settings | `client.EcommerceSettings` | 5 |
| Email | `client.Email` | 1 |
| Inventory locations | `client.InventoryLocations` | 11 |
| Order custom fields | `client.OrderCustomFields` | 4 |
| Order flags | `client.OrderFlags` | 7 |
| Orders | `client.Orders` | 25 |
| Pages | `client.Pages` | 15 |
| Payment gateways | `client.PaymentGateways` | 8 |
| Price display settings | `client.PriceDisplaySettings` | 5 |
| Product listing settings | `client.ProductListing` | 3 |
| Product page settings | `client.ProductPage` | 5 |
| Products | `client.Products` | 10 |
| Search result page settings | `client.SearchResultPage` | 13 |
| Search settings | `client.SearchSettings` | 9 |
| SEO configuration | `client.SeoConfiguration` | 18 |
| Shipping classes | `client.ShippingClasses` | 9 |
| Shipping methods | `client.ShippingMethods` | 9 |
| Shipping settings | `client.ShippingSettings` | 3 |
| Shopping cart settings | `client.ShoppingCart` | 5 |
| Site settings | `client.SiteSettings` | 5 |
| Tax codes | `client.TaxCodes` | 7 |
| Tax country taxes | `client.TaxCountryTaxes` | 5 |
| Tax custom profiles | `client.TaxCustomProfiles` | 5 |
| Tax profiles | `client.TaxProfiles` | 7 |
| Tax rules | `client.TaxRules` | 7 |
| Tax settings | `client.TaxSettings` | 2 |
| Tax zones | `client.TaxZones` | 7 |
| Transactions | `client.Transactions` | 6 |
| Trash | `client.Trash` | 8 |
| Variations | `client.Variations` | 12 |

Each resource's path templates are constants on its `<Resource>Urls` class, and its typed views are
in the `<Resource>Dto` namespace beside it. The XML documentation on each method names the
endpoint it calls and the behaviour to expect.

## Documentation

- API reference: <https://newlify.mintlify.app>

## License

MIT. See [LICENSE](LICENSE).
