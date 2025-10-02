namespace Ebee.Paystack.Clients.Transactions.Models;

/// <summary>
/// Request model for listing transactions with filtering options
/// </summary>
public class ListTransactionsRequest
{
    /// <summary>
    /// Number of records to fetch per page (default: 50, max: 100)
    /// </summary>
    public int PerPage { get; set; } = 50;

    /// <summary>
    /// Page number to retrieve (starts from 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Filter by customer ID or email
    /// </summary>
    public string? Customer { get; set; }

    /// <summary>
    /// Filter by status (success, failed, abandoned, etc.)
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Start date for filtering (ISO 8601 format)
    /// </summary>
    public DateTime? From { get; set; }

    /// <summary>
    /// End date for filtering (ISO 8601 format)
    /// </summary>
    public DateTime? To { get; set; }

    /// <summary>
    /// Filter by amount (in kobo)
    /// </summary>
    public decimal? Amount { get; set; }
}