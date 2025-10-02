namespace Ebee.Paystack.Core;

/// <summary>
/// Exception thrown when Paystack API operations fail
/// </summary>
public class PaystackException : Exception
{
    /// <summary>
    /// HTTP status code from the failed request
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Response content from Paystack API
    /// </summary>
    public string? ResponseContent { get; }

    public PaystackException(string message) : base(message)
    {
    }

    public PaystackException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public PaystackException(string message, int statusCode, string? responseContent = null)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseContent = responseContent;
    }
}
