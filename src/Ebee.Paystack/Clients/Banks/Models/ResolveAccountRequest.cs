using System.Text.Json.Serialization;

namespace Ebee.Paystack.Clients.Banks.Models;

/// <summary>
/// Request model for resolving account number
/// </summary>
public class ResolveAccountRequest
{
    /// <summary>
    /// Account number to resolve
    /// </summary>
    [JsonPropertyName("account_number")]
    public required string AccountNumber { get; set; }

    /// <summary>
    /// Bank code
    /// </summary>
    [JsonPropertyName("bank_code")]
    public required string BankCode { get; set; }
}
