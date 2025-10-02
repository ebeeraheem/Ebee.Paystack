using System.Text.Json.Serialization;

namespace Ebee.Paystack.Clients.Transactions.Models;

/// <summary>
/// Authorization information in a transaction
/// </summary>
public class TransactionAuthorization
{
    /// <summary>
    /// Authorization code
    /// </summary>
    [JsonPropertyName("authorization_code")]
    public string AuthorizationCode { get; set; } = string.Empty;

    /// <summary>
    /// Card bin
    /// </summary>
    [JsonPropertyName("bin")]
    public string Bin { get; set; } = string.Empty;

    /// <summary>
    /// Last 4 digits of card
    /// </summary>
    [JsonPropertyName("last4")]
    public string Last4 { get; set; } = string.Empty;

    /// <summary>
    /// Card expiry month
    /// </summary>
    [JsonPropertyName("exp_month")]
    public string ExpMonth { get; set; } = string.Empty;

    /// <summary>
    /// Card expiry year
    /// </summary>
    [JsonPropertyName("exp_year")]
    public string ExpYear { get; set; } = string.Empty;

    /// <summary>
    /// Card channel
    /// </summary>
    [JsonPropertyName("channel")]
    public string Channel { get; set; } = string.Empty;

    /// <summary>
    /// Card type
    /// </summary>
    [JsonPropertyName("card_type")]
    public string CardType { get; set; } = string.Empty;

    /// <summary>
    /// Bank name
    /// </summary>
    [JsonPropertyName("bank")]
    public string Bank { get; set; } = string.Empty;

    /// <summary>
    /// Country code
    /// </summary>
    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// Card brand
    /// </summary>
    [JsonPropertyName("brand")]
    public string Brand { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if authorization is reusable
    /// </summary>
    [JsonPropertyName("reusable")]
    public bool Reusable { get; set; }

    /// <summary>
    /// Signature
    /// </summary>
    [JsonPropertyName("signature")]
    public string Signature { get; set; } = string.Empty;

    /// <summary>
    /// Account name
    /// </summary>
    [JsonPropertyName("account_name")]
    public string? AccountName { get; set; }
}
