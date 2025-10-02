using System.Text.Json.Serialization;

namespace Ebee.Paystack.Clients.Banks.Models;

/// <summary>
/// Represents a resolved account from Paystack
/// </summary>
public class ResolvedAccount
{
    /// <summary>
    /// Account number
    /// </summary>
    [JsonPropertyName("account_number")]
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Account name
    /// </summary>
    [JsonPropertyName("account_name")]
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Bank ID
    /// </summary>
    [JsonPropertyName("bank_id")]
    public int BankId { get; set; }
}