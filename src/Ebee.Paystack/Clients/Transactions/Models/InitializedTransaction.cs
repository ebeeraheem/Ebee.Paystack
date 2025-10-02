using System.Text.Json.Serialization;

namespace Ebee.Paystack.Clients.Transactions.Models;

/// <summary>
/// Response model for initialized transaction
/// </summary>
public class InitializedTransaction
{
    /// <summary>
    /// Authorization URL for the transaction
    /// </summary>
    [JsonPropertyName("authorization_url")]
    public string AuthorizationUrl { get; set; } = string.Empty;

    /// <summary>
    /// Access code for the transaction
    /// </summary>
    [JsonPropertyName("access_code")]
    public string AccessCode { get; set; } = string.Empty;

    /// <summary>
    /// Transaction reference
    /// </summary>
    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;
}
