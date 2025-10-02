using Ebee.Paystack.Common;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<PaystackHttpClient> _logger;

    public PaystackHttpClient(
        HttpClient httpClient,
        IOptions<PaystackConfiguration> configuration,
        ILogger<PaystackHttpClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _configuration = configuration.Value ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
        LogRequest("GET", endpoint);

        var response = await _httpClient.GetAsync(endpoint, cancellationToken);

        await LogResponse("GET", endpoint, response);

        return await ProcessResponse<T>(response);
    }

    public async Task<PaystackResponse<T>> PostAsync<T>(string endpoint, object? payload = null, CancellationToken cancellationToken = default)
    {
        var content = CreateJsonContent(payload);

        LogRequest("POST", endpoint, payload);

        var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);

        await LogResponse("POST", endpoint, response);

        return await ProcessResponse<T>(response);
    }

    public async Task<PaystackResponse<T>> PutAsync<T>(string endpoint, object? payload = null, CancellationToken cancellationToken = default)
    {
        var content = CreateJsonContent(payload);

        LogRequest("PUT", endpoint, payload);

        var response = await _httpClient.PutAsync(endpoint, content, cancellationToken);

        await LogResponse("PUT", endpoint, response);

        return await ProcessResponse<T>(response);
    }

    public async Task<PaystackResponse<T>> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        LogRequest("DELETE", endpoint);

        var response = await _httpClient.DeleteAsync(endpoint, cancellationToken);

        await LogResponse("DELETE", endpoint, response);

        return await ProcessResponse<T>(response);
    }

    private void LogRequest(string method, string endpoint, object? payload = null)
    {
        if (!_configuration.EnableLogging || !ShouldLog(Microsoft.Extensions.Logging.LogLevel.Information))
            return;

        var sanitizedPayload = payload is not null ? SanitizePayload(payload) : null;

        _logger.LogInformation(
            "Paystack API Request: {Method} {Endpoint} {Payload}",
            method,
            endpoint,
            sanitizedPayload is not null ? JsonSerializer.Serialize(sanitizedPayload, _jsonOptions) : null);
    }

    private async Task LogResponse(string method, string endpoint, HttpResponseMessage response)
    {
        if (!_configuration.EnableLogging)
            return;

        var responseContent = await response.Content.ReadAsStringAsync();
        var sanitizedContent = SanitizeResponseContent(responseContent);

        var logLevel = response.IsSuccessStatusCode
            ? Microsoft.Extensions.Logging.LogLevel.Information
            : Microsoft.Extensions.Logging.LogLevel.Warning;

        if (!ShouldLog(logLevel))
            return;

        _logger.Log(logLevel,
            "Paystack API Response: {Method} {Endpoint} {StatusCode} {ResponseTime}ms {Content}",
            method,
            endpoint,
            (int)response.StatusCode,
            response.Headers.Date?.ToString("O") ?? "Unknown",
            sanitizedContent);
    }

    private bool ShouldLog(Microsoft.Extensions.Logging.LogLevel logLevel)
    {
        return _configuration.LogLevel switch
        {
            LogLevel.None => false,
            LogLevel.Error => logLevel >= Microsoft.Extensions.Logging.LogLevel.Error,
            LogLevel.Warning => logLevel >= Microsoft.Extensions.Logging.LogLevel.Warning,
            LogLevel.Information => logLevel >= Microsoft.Extensions.Logging.LogLevel.Information,
            LogLevel.Debug => logLevel >= Microsoft.Extensions.Logging.LogLevel.Debug,
            LogLevel.Trace => logLevel >= Microsoft.Extensions.Logging.LogLevel.Trace,
            _ => false
        };
    }

    private object? SanitizePayload(object payload)
    {
        try
        {
            var json = JsonSerializer.Serialize(payload, _jsonOptions);
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);

            return SanitizeJsonElement(jsonElement);
        }
        catch
        {
            return "[Could not sanitize payload]";
        }
    }

    private string SanitizeResponseContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return content;

        try
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);
            var sanitized = SanitizeJsonElement(jsonElement);
            return JsonSerializer.Serialize(sanitized, _jsonOptions);
        }
        catch
        {
            // If it's not JSON or parsing fails, return truncated content
            return content.Length > 1000 ? content[..1000] + "..." : content;
        }
    }

    private object? SanitizeJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => SanitizeJsonObject(element),
            JsonValueKind.Array => element.EnumerateArray().Select(SanitizeJsonElement).ToArray(),
            JsonValueKind.String => SanitizeStringValue(element.GetString() ?? string.Empty),
            JsonValueKind.Number => element.GetDecimal(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => element.ToString()
        };
    }

    private Dictionary<string, object?> SanitizeJsonObject(JsonElement element)
    {
        var result = new Dictionary<string, object?>();

        foreach (var property in element.EnumerateObject())
        {
            var key = property.Name.ToLowerInvariant();

            // List of sensitive fields to mask
            var sensitiveFields = new[]
            {
                "secret_key", "secretkey", "authorization_code", "authorizationcode",
                "access_code", "accesscode", "pin", "cvv", "card_number", "cardnumber",
                "account_number", "accountnumber", "bvn", "password", "token"
            };

            if (sensitiveFields.Contains(key))
            {
                result[property.Name] = MaskSensitiveValue(property.Value.GetString() ?? string.Empty);
            }
            else
            {
                result[property.Name] = SanitizeJsonElement(property.Value);
            }
        }

        return result;
    }

    private static string SanitizeStringValue(string value)
    {
        // Don't log very long strings in full
        return value.Length > 500 ? value[..50] + "..." : value;
    }

    private static string MaskSensitiveValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        return value.Length switch
        {
            <= 4 => new string('*', value.Length),
            <= 8 => value[..2] + new string('*', value.Length - 4) + value[^2..],
            _ => value[..4] + new string('*', Math.Min(value.Length - 8, 10)) + value[^4..]
        };
    }

    private StringContent? CreateJsonContent(object? payload)
    {
        if (payload is null) return null;

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
