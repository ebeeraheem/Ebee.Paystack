using System.Text.Json.Serialization;

namespace Ebee.Paystack.Clients.Transactions.Models;

/// <summary>
/// Request model for initializing a transaction
/// </summary>
public class InitializeTransactionRequest
{
    /// <summary>
    /// Amount in kobo (NGN) or pesewas (GHS) or cents (ZAR)
    /// </summary>
    [JsonPropertyName("amount")]
    public required decimal Amount { get; set; }

    /// <summary>
    /// Customer's email address
    /// </summary>
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    /// <summary>
    /// Unique transaction reference
    /// </summary>
    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    /// <summary>
    /// Currency (NGN, GHS, ZAR, USD)
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "NGN";

    /// <summary>
    /// URL to redirect to after payment
    /// </summary>
    [JsonPropertyName("callback_url")]
    public string? CallbackUrl { get; set; }

    /// <summary>
    /// Array of payment channels to control what channels you want to make available to the user
    /// </summary>
    [JsonPropertyName("channels")]
    public string[]? Channels { get; set; }

    /// <summary>
    /// Additional information about the transaction
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// Split code for split payments
    /// </summary>
    [JsonPropertyName("split_code")]
    public string? SplitCode { get; set; }

    /// <summary>
    /// Customer ID or code
    /// </summary>
    [JsonPropertyName("customer")]
    public string? Customer { get; set; }

    /// <summary>
    /// Plan code for subscriptions
    /// </summary>
    [JsonPropertyName("plan")]
    public string? Plan { get; set; }

    /// <summary>
    /// Number of times to charge customer during subscription
    /// </summary>
    [JsonPropertyName("invoice_limit")]
    public int? InvoiceLimit { get; set; }
}
