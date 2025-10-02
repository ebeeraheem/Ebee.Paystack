using System.Text.Json.Serialization;

namespace Ebee.Paystack.Common;

/// <summary>
/// Meta information for paginated responses
/// </summary>
public class PaystackMeta
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("skipped")]
    public int Skipped { get; set; }

    [JsonPropertyName("perPage")]
    public int PerPage { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pageCount")]
    public int PageCount { get; set; }
}
