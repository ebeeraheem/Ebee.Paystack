using System.Text.Json.Serialization;

namespace Ebee.Paystack.Clients.Transactions.Models;

/// <summary>
/// Customer information in a transaction
/// </summary>
public class TransactionCustomer
{
    /// <summary>
    /// Customer ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Customer code
    /// </summary>
    [JsonPropertyName("customer_code")]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// Customer email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Customer first name
    /// </summary>
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Customer last name
    /// </summary>
    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    /// <summary>
    /// Customer phone number
    /// </summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
}
