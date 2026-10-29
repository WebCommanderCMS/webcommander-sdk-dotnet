namespace WebCommander.Resources.Transactions;

/// <summary>Path templates for the transaction-number endpoints.</summary>
public static class TransactionsUrls
{
    public const string Transactions = "/api/{api_version}/admin/transactions";

    public const string Transaction = "/api/{api_version}/admin/transactions/{transaction_no}";

    public const string TransactionItems = "/api/{api_version}/admin/transactions/{transaction_no}/items";
}
