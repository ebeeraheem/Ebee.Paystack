using Ebee.Paystack.Clients.Banks;
using Ebee.Paystack.Clients.Transactions;

namespace Ebee.Paystack;

/// <summary>
/// Main interface for accessing all Paystack API clients
/// </summary>
public interface IPaystackClient
{
    /// <summary>
    /// Banks API client for bank-related operations
    /// </summary>
    IBanksClient Banks { get; }

    /// <summary>
    /// Transactions API client for transaction operations
    /// </summary>
    ITransactionsClient Transactions { get; }
}
