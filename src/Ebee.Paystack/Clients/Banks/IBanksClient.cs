using Ebee.Paystack.Clients.Banks.Models;
using Ebee.Paystack.Common;

namespace Ebee.Paystack.Clients.Banks;

/// <summary>
/// Interface for Paystack Banks API operations
/// </summary>
public interface IBanksClient
{
    /// <summary>
    /// Get a list of all banks supported by Paystack
    /// </summary>
    /// <param name="country">The country to filter banks by (e.g., "nigeria", "ghana", "south africa")</param>
    /// <param name="useCursor">Use cursor for pagination instead of page numbers</param>
    /// <param name="perPage">Number of records to fetch per page (default: 50, max: 100)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of banks</returns>
    /// <example>
    /// <code>
    /// var banksResponse = await banksClient.ListBanksAsync(country: "nigeria", perPage: 20);
    /// if (banksResponse.Status)
    /// {
    ///     foreach (var bank in banksResponse.Data!)
    ///     {
    ///         Console.WriteLine($"{bank.Name} - {bank.Code}");
    ///     }
    /// }
    /// </code>
    /// </example>
    Task<PaystackResponse<Bank[]>> ListBanksAsync(
        string? country = null,
        bool useCursor = false,
        int perPage = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolve account number to get account details
    /// </summary>
    /// <param name="request">Account resolution request containing account number and bank code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Resolved account details</returns>
    /// <example>
    /// <code>
    /// var request = new ResolveAccountRequest
    /// {
    ///     AccountNumber = "0022728151",
    ///     BankCode = "063"
    /// };
    /// var response = await banksClient.ResolveAccountAsync(request);
    /// if (response.Status)
    /// {
    ///     Console.WriteLine($"Account Name: {response.Data!.AccountName}");
    /// }
    /// </code>
    /// </example>
    Task<PaystackResponse<ResolvedAccount>> ResolveAccountAsync(
        ResolveAccountRequest request,
        CancellationToken cancellationToken = default);
}