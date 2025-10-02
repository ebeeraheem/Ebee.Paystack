using Ebee.Paystack.Common;

namespace Ebee.Paystack.Core;

/// <summary>
/// Interface for making HTTP requests to Paystack API
/// </summary>
public interface IPaystackHttpClient
{
    /// <summary>
    /// Makes a GET request to the specified endpoint
    /// </summary>
    Task<PaystackResponse<T>> GetAsync<T>(
        string endpoint,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Makes a POST request to the specified endpoint with JSON payload
    /// </summary>
    Task<PaystackResponse<T>> PostAsync<T>(
        string endpoint,
        object? payload = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Makes a PUT request to the specified endpoint with JSON payload
    /// </summary>
    Task<PaystackResponse<T>> PutAsync<T>(
        string endpoint,
        object? payload = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Makes a DELETE request to the specified endpoint
    /// </summary>
    Task<PaystackResponse<T>> DeleteAsync<T>(
        string endpoint,
        CancellationToken cancellationToken = default);
}
