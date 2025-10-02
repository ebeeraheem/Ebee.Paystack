using System.Text.Json.Serialization;

namespace Ebee.Paystack.Clients.Transactions.Models;

/// <summary>
/// Represents a transaction from Paystack
/// </summary>
public class Transaction
{
    /// <summary>
    /// Transaction ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Transaction domain
    /// </summary>
    [JsonPropertyName("domain")]
    public string Domain { get; set; } = string.Empty;

    /// <summary>
    /// Transaction status
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Transaction reference
    /// </summary>
    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    /// <summary>
    /// Receipt number
    /// </summary>
    [JsonPropertyName("receipt_number")]
    public string? ReceiptNumber { get; set; }

    /// <summary>
    /// Transaction amount in kobo
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Transaction message
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Gateway response
    /// </summary>
    [JsonPropertyName("gateway_response")]
    public string GatewayResponse { get; set; } = string.Empty;

    /// <summary>
    /// Date transaction was paid
    /// </summary>
    [JsonPropertyName("paid_at")]
    public DateTime? PaidAt { get; set; }

    /// <summary>
    /// Date transaction was created
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Transaction channel
    /// </summary>
    [JsonPropertyName("channel")]
    public string Channel { get; set; } = string.Empty;

    /// <summary>
    /// Transaction currency
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// IP address of customer
    /// </summary>
    [JsonPropertyName("ip_address")]
    public string? IpAddress { get; set; }

    /// <summary>
    /// Transaction metadata
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// Transaction fees
    /// </summary>
    [JsonPropertyName("fees")]
    public decimal? Fees { get; set; }

    /// <summary>
    /// Customer information
    /// </summary>
    [JsonPropertyName("customer")]
    public TransactionCustomer? Customer { get; set; }

    /// <summary>
    /// Authorization information
    /// </summary>
    [JsonPropertyName("authorization")]
    public TransactionAuthorization? Authorization { get; set; }

    /// <summary>
    /// Plan information (if transaction is for a subscription)
    /// </summary>
    [JsonPropertyName("plan")]
    public TransactionPlan? Plan { get; set; }
}
