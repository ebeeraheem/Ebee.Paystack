using Ebee.Paystack.Clients.Transactions.Models;
using Ebee.Paystack.Common;
using Ebee.Paystack.Core;

namespace Ebee.Paystack.Clients.Transactions;

/// <summary>
/// Client for Paystack Transactions API operations
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TransactionsClient"/> class
/// </remarks>
/// <param name="httpClient">The HTTP client for making API requests</param>
/// <exception cref="ArgumentNullException">Thrown when httpClient is null</exception>
public class TransactionsClient(IPaystackHttpClient httpClient) : ITransactionsClient
{
    private readonly IPaystackHttpClient _httpClient = httpClient
        ?? throw new ArgumentNullException(nameof(httpClient));

    /// <inheritdoc />
    public async Task<PaystackResponse<InitializedTransaction>> InitializeTransactionAsync(
        InitializeTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required", nameof(request));

        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero", nameof(request));

        const string endpoint = "/transaction/initialize";
        return await _httpClient.PostAsync<InitializedTransaction>(endpoint, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaystackResponse<Transaction>> VerifyTransactionAsync(
        string reference,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("Reference is required", nameof(reference));

        var endpoint = $"/transaction/verify/{Uri.EscapeDataString(reference)}";
        return await _httpClient.GetAsync<Transaction>(endpoint, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaystackResponse<Transaction[]>> ListTransactionsAsync(
        ListTransactionsRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        request ??= new ListTransactionsRequest();

        // Validate perPage parameter
        if (request.PerPage is < 1 or > 100)
            throw new ArgumentOutOfRangeException(nameof(request), "PerPage must be between 1 and 100");

        if (request.Page < 1)
            throw new ArgumentOutOfRangeException(nameof(request), "Page must be greater than zero");

        // Build query parameters
        var queryParams = new List<string>();

        if (request.PerPage != 50)
            queryParams.Add($"perPage={request.PerPage}");

        if (request.Page != 1)
            queryParams.Add($"page={request.Page}");

        if (!string.IsNullOrWhiteSpace(request.Customer))
            queryParams.Add($"customer={Uri.EscapeDataString(request.Customer)}");

        if (!string.IsNullOrWhiteSpace(request.Status))
            queryParams.Add($"status={Uri.EscapeDataString(request.Status)}");

        if (request.From.HasValue)
            queryParams.Add($"from={request.From.Value:yyyy-MM-ddTHH:mm:ss.fffZ}");

        if (request.To.HasValue)
            queryParams.Add($"to={request.To.Value:yyyy-MM-ddTHH:mm:ss.fffZ}");

        if (request.Amount.HasValue)
            queryParams.Add($"amount={request.Amount.Value}");

        var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
        var endpoint = $"/transaction{queryString}";

        return await _httpClient.GetAsync<Transaction[]>(endpoint, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaystackResponse<Transaction>> FetchTransactionAsync(
        int transactionId,
        CancellationToken cancellationToken = default)
    {
        if (transactionId <= 0)
            throw new ArgumentException("Transaction ID must be greater than zero", nameof(transactionId));

        var endpoint = $"/transaction/{transactionId}";
        return await _httpClient.GetAsync<Transaction>(endpoint, cancellationToken);
    }
}
