using System.Text.Json.Serialization;

namespace Ebee.Paystack.Clients.Transactions.Models;

/// <summary>
/// Plan information in a transaction
/// </summary>
public class TransactionPlan
{
    /// <summary>
    /// Plan ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Plan name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Plan code
    /// </summary>
    [JsonPropertyName("plan_code")]
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// Plan description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Plan amount
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Plan interval
    /// </summary>
    [JsonPropertyName("interval")]
    public string Interval { get; set; } = string.Empty;

    /// <summary>
    /// Plan currency
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;
}
