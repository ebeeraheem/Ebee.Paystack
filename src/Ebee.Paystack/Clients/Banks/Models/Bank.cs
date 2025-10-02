using System.Text.Json.Serialization;

namespace Ebee.Paystack.Clients.Banks.Models;

/// <summary>
/// Represents a bank supported by Paystack
/// </summary>
public class Bank
{
    /// <summary>
    /// Bank name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Bank slug identifier
    /// </summary>
    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Bank code
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Long bank code
    /// </summary>
    [JsonPropertyName("longcode")]
    public string? LongCode { get; set; }

    /// <summary>
    /// Bank gateway
    /// </summary>
    [JsonPropertyName("gateway")]
    public string? Gateway { get; set; }

    /// <summary>
    /// Indicates if the bank supports pay with bank
    /// </summary>
    [JsonPropertyName("pay_with_bank")]
    public bool PayWithBank { get; set; }

    /// <summary>
    /// Indicates if the bank supports pay with bank transfer
    /// </summary>
    [JsonPropertyName("pay_with_bank_transfer")]
    public bool PayWithBankTransfer { get; set; }

    /// <summary>
    /// Indicates if the bank is active
    /// </summary>
    [JsonPropertyName("active")]
    public bool Active { get; set; }

    /// <summary>
    /// Country where the bank operates
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Currency used by the bank
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Bank type
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Bank ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}
