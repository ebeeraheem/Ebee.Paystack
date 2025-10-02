using Ebee.Paystack.Clients.Banks.Models;
using Ebee.Paystack.Common;
using Ebee.Paystack.Core;

namespace Ebee.Paystack.Clients.Banks;

/// <summary>
/// Client for Paystack Banks API operations
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BanksClient"/> class
/// </remarks>
/// <param name="httpClient">The HTTP client for making API requests</param>
/// <exception cref="ArgumentNullException">Thrown when httpClient is null</exception>
public class BanksClient(IPaystackHttpClient httpClient) : IBanksClient
{
    private readonly IPaystackHttpClient _httpClient = httpClient
        ?? throw new ArgumentNullException(nameof(httpClient));

    /// <inheritdoc />
    public async Task<PaystackResponse<Bank[]>> ListBanksAsync(
        string? country = null,
        bool useCursor = false,
        int perPage = 50,
        CancellationToken cancellationToken = default)
    {
        // Validate perPage parameter
        if (perPage is < 1 or > 100)
            throw new ArgumentOutOfRangeException(nameof(perPage), "perPage must be between 1 and 100");

        // Build query parameters
        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(country))
            queryParams.Add($"country={Uri.EscapeDataString(country)}");

        if (useCursor)
            queryParams.Add("use_cursor=true");

        if (perPage != 50)
            queryParams.Add($"perPage={perPage}");

        var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
        var endpoint = $"/bank{queryString}";

        return await _httpClient.GetAsync<Bank[]>(endpoint, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaystackResponse<ResolvedAccount>> ResolveAccountAsync(
        ResolveAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.AccountNumber))
            throw new ArgumentException("Account number is required", nameof(request));

        if (string.IsNullOrWhiteSpace(request.BankCode))
            throw new ArgumentException("Bank code is required", nameof(request));

        var queryString = $"?account_number={Uri.EscapeDataString(request.AccountNumber)}" +
                         $"&bank_code={Uri.EscapeDataString(request.BankCode)}";

        var endpoint = $"/bank/resolve{queryString}";

        return await _httpClient.GetAsync<ResolvedAccount>(endpoint, cancellationToken);
    }
}
