using System.Text.Json.Serialization;

namespace Ebee.Paystack.Common;

/// <summary>
/// Base response structure for all Paystack API responses
/// </summary>
/// <typeparam name="T">The type of data returned</typeparam>
public class PaystackResponse<T>
{
    /// <summary>
    /// Indicates if the request was successful
    /// </summary>
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    /// <summary>
    /// Response message from Paystack
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The actual response data
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    /// <summary>
    /// Meta information (pagination, etc.)
    /// </summary>
    [JsonPropertyName("meta")]
    public PaystackMeta? Meta { get; set; }
}
