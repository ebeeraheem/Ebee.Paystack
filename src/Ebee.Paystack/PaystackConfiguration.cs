namespace Ebee.Paystack;

/// <summary>
/// Configuration options for the Paystack SDK
/// </summary>
public class PaystackConfiguration
{
    /// <summary>
    /// The Paystack secret key for authentication
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// The base URL for Paystack API (defaults to production)
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.paystack.co";

    /// <summary>
    /// Request timeout in seconds (default: 30 seconds)
    /// </summary>
    public int TimeoutInSeconds { get; set; } = 30;

    /// <summary>
    /// Enable request/response logging for debugging
    /// </summary>
    public bool EnableLogging { get; set; } = false;
}
