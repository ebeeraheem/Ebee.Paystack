using Ebee.Paystack.Clients.Banks;
using Ebee.Paystack.Clients.Transactions;

namespace Ebee.Paystack;

/// <summary>
/// Main client for accessing all Paystack API functionality
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PaystackClient"/> class
/// </remarks>
/// <param name="banksClient">The banks API client</param>
/// <param name="transactionsClient">The transactions API client</param>
public class PaystackClient(IBanksClient banksClient, ITransactionsClient transactionsClient) : IPaystackClient
{
    /// <inheritdoc />
    public IBanksClient Banks { get; } = banksClient
        ?? throw new ArgumentNullException(nameof(banksClient));

    /// <inheritdoc />
    public ITransactionsClient Transactions { get; } = transactionsClient
        ?? throw new ArgumentNullException(nameof(transactionsClient));
}
