using WebCommander.Client;
using WebCommander.Resources.Transactions.TransactionsDto;

namespace WebCommander.Resources.Transactions;

/// <summary>Transaction numbers - <c>/admin/transactions*</c> - as a typed resource bound to one client.</summary>
public sealed class Transactions : BoundResource
{
    /// <summary>Bind to a client; normally reached as <c>client.Transactions</c>.</summary>
    public Transactions(WebCommanderClient client) : base(client)
    {
    }

    /// <summary>List the numbers in ascending order - <c>GET /admin/transactions</c>. <c>limit</c> (or <c>max</c>)
    /// from 1, default 20, held to 200; <c>offset</c>, <c>page</c>; <c>transaction_no</c> matches exactly and
    /// case-sensitively. <c>search</c>, <c>q</c>, <c>sort</c> and <c>order</c> change nothing.</summary>
    public Task<ApiResponse> ListTransactionsAsync(
        IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TransactionsUrls.Transactions,
            new RequestOptions
            {
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TransactionPage),
            }, cancellationToken);

    /// <summary>HEAD the collection - <c>HEAD /admin/transactions</c>; no body.</summary>
    public Task<ApiResponse> HeadTransactionsAsync(
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TransactionsUrls.Transactions,
            new RequestOptions
            {
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>Count the records one number tagged, by type - <c>GET /admin/transactions/{transaction_no}</c>.
    /// Matched exactly and case-sensitively; a number holding <c>/</c> is refused by the server, <c>400</c> HTML,
    /// as an encoded slash.</summary>
    public Task<ApiResponse> GetTransactionAsync(
        string transactionNo,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TransactionsUrls.Transaction,
            new RequestOptions
            {
                PathParams = Number(transactionNo),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(Transaction),
            }, cancellationToken);

    /// <summary>HEAD one number - <c>HEAD /admin/transactions/{transaction_no}</c>; no body.</summary>
    public Task<ApiResponse> HeadTransactionAsync(
        string transactionNo,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TransactionsUrls.Transaction,
            new RequestOptions
            {
                PathParams = Number(transactionNo),
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    /// <summary>List the records a number tagged, newest first -
    /// <c>GET /admin/transactions/{transaction_no}/items</c>. <c>type</c> is optional: left out, every type
    /// is listed; sent, it takes one or more of the 15 <c>by_type</c> names, comma-separated. <c>limit</c>
    /// (or <c>max</c>) 1 to 200, <c>offset</c>, <c>page</c>, <c>detail</c> <c>slim</c> or <c>full</c>. A
    /// <c>slim</c> item is <c>type</c>, <c>id</c>, <c>stamp</c>, <c>trashed</c> and the fields of its type, such
    /// as <c>name</c>, <c>sku</c> and <c>url</c> for a product. A number or a type with no records answers
    /// <c>200</c> with <c>items</c> empty and <c>total</c> 0, not <c>404</c>.</summary>
    public Task<ApiResponse> ListTransactionItemsAsync(
        string transactionNo, IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendGetRequestAsync(TransactionsUrls.TransactionItems,
            new RequestOptions
            {
                PathParams = Number(transactionNo),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
                DtoType = typeof(TransactionItemPage),
            }, cancellationToken);

    /// <summary>HEAD the records under a number; no body, and <c>type</c> is optional here too -
    /// <c>HEAD /admin/transactions/{transaction_no}/items</c>.</summary>
    public Task<ApiResponse> HeadTransactionItemsAsync(
        string transactionNo, IReadOnlyDictionary<string, object?>? queryParams = null,
        bool? raiseOnError = null, CancellationToken cancellationToken = default,
        string? responseFormat = null) =>
        Client.SendApiRequestAsync("HEAD", TransactionsUrls.TransactionItems,
            new RequestOptions
            {
                PathParams = Number(transactionNo),
                QueryParams = queryParams,
                RaiseOnError = raiseOnError,
                ResponseFormat = responseFormat,
            }, cancellationToken);

    private static Dictionary<string, object?> Number(string transactionNo) =>
        new() { ["transaction_no"] = transactionNo };
}
