using Ebee.Paystack.Clients.Transactions.Models;
using Ebee.Paystack.Common;

namespace Ebee.Paystack.Clients.Transactions;


/// <summary>
/// Interface for Paystack Transactions API operations
/// </summary>
public interface ITransactionsClient
{
    /// <summary>
    /// Initialize a transaction
    /// </summary>
    /// <param name="request">Transaction initialization request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Initialized transaction with authorization URL</returns>
    /// <example>
    /// <code>
    /// var request = new InitializeTransactionRequest
    /// {
    ///     Email = "customer@email.com",
    ///     Amount = 20000, // 200.00 NGN in kobo
    ///     Currency = "NGN",
    ///     CallbackUrl = "https://yourwebsite.com/verify"
    /// };
    /// var response = await transactionsClient.InitializeTransactionAsync(request);
    /// if (response.Status)
    /// {
    ///     // Redirect customer to response.Data!.AuthorizationUrl
    /// }
    /// </code>
    /// </example>
    Task<PaystackResponse<InitializedTransaction>> InitializeTransactionAsync(
        InitializeTransactionRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Verify a transaction using the transaction reference
    /// </summary>
    /// <param name="reference">Transaction reference</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Transaction details</returns>
    /// <example>
    /// <code>
    /// var response = await transactionsClient.VerifyTransactionAsync("T123456789");
    /// if (response.Status && response.Data!.Status == "success")
    /// {
    ///     // Transaction was successful
    ///     Console.WriteLine($"Amount: {response.Data.Amount}");
    /// }
    /// </code>
    /// </example>
    Task<PaystackResponse<Transaction>> VerifyTransactionAsync(
        string reference,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// List transactions with optional filtering
    /// </summary>
    /// <param name="request">List transactions request with filtering options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of transactions</returns>
    /// <example>
    /// <code>
    /// var request = new ListTransactionsRequest
    /// {
    ///     PerPage = 20,
    ///     Status = "success",
    ///     From = DateTime.Now.AddDays(-30)
    /// };
    /// var response = await transactionsClient.ListTransactionsAsync(request);
    /// if (response.Status)
    /// {
    ///     foreach (var transaction in response.Data!)
    ///     {
    ///         Console.WriteLine($"{transaction.Reference} - {transaction.Amount}");
    ///     }
    /// }
    /// </code>
    /// </example>
    Task<PaystackResponse<Transaction[]>> ListTransactionsAsync(
        ListTransactionsRequest? request = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetch a specific transaction by ID
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Transaction details</returns>
    /// <example>
    /// <code>
    /// var response = await transactionsClient.FetchTransactionAsync(123456);
    /// if (response.Status)
    /// {
    ///     var transaction = response.Data!;
    ///     Console.WriteLine($"Status: {transaction.Status}");
    /// }
    /// </code>
    /// </example>
    Task<PaystackResponse<Transaction>> FetchTransactionAsync(
        int transactionId,
        CancellationToken cancellationToken = default);
}
