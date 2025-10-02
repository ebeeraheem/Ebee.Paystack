using Ebee.Paystack.Common;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Ebee.Paystack.Core;

/// <summary>
/// HTTP client implementation for Paystack API communication
/// </summary>
internal class PaystackHttpClient : IPaystackHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly PaystackConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions;

    public PaystackHttpClient(HttpClient httpClient, IOptions<PaystackConfiguration> configuration)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _configuration = configuration.Value ?? throw new ArgumentNullException(nameof(configuration));

        // Configure JSON serialization options
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        // Set base address
        _httpClient.BaseAddress = new Uri(_configuration.BaseUrl);

        // Set timeout
        _httpClient.Timeout = TimeSpan.FromSeconds(_configuration.TimeoutInSeconds);

        // Set default headers
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _configuration.SecretKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue(productName: "Ebee.Paystack", productVersion: "1.0.0"));
    }

    public async Task<PaystackResponse<T>> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(endpoint, cancellationToken);
        return await ProcessResponse<T>(response);
    }

    public async Task<PaystackResponse<T>> PostAsync<T>(string endpoint, object? payload = null, CancellationToken cancellationToken = default)
    {
        var content = CreateJsonContent(payload);
        var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
        return await ProcessResponse<T>(response);
    }

    public async Task<PaystackResponse<T>> PutAsync<T>(string endpoint, object? payload = null, CancellationToken cancellationToken = default)
    {
        var content = CreateJsonContent(payload);
        var response = await _httpClient.PutAsync(endpoint, content, cancellationToken);
        return await ProcessResponse<T>(response);
    }

    public async Task<PaystackResponse<T>> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(endpoint, cancellationToken);
        return await ProcessResponse<T>(response);
    }

    private StringContent? CreateJsonContent(object? payload)
    {
        if (payload == null) return null;

        var json = JsonSerializer.Serialize(payload, _jsonOptions);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private async Task<PaystackResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            // Try to parse error response
            try
            {
                var errorResponse = JsonSerializer
                    .Deserialize<PaystackResponse<object>>(responseContent, _jsonOptions);

                throw new PaystackException(
                    errorResponse?.Message ?? "Request failed",
                    (int)response.StatusCode,
                    responseContent);
            }
            catch (JsonException)
            {
                throw new PaystackException(
                    $"Request failed with status {response.StatusCode}",
                    (int)response.StatusCode,
                    responseContent);
            }
        }

        try
        {
            var result = JsonSerializer.Deserialize<PaystackResponse<T>>(responseContent, _jsonOptions);
            return result ?? throw new PaystackException("Failed to deserialize response");
        }
        catch (JsonException ex)
        {
            throw new PaystackException("Failed to deserialize response", ex);
        }
    }
}
