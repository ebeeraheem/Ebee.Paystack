using Microsoft.Extensions.Options;

namespace Ebee.Paystack.Extensions;

/// <summary>
/// Validator for PaystackConfiguration options
/// </summary>
internal class PaystackConfigurationValidator : IValidateOptions<PaystackConfiguration>
{
    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, PaystackConfiguration options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var failures = new List<string>();

        if (string.IsNullOrWhiteSpace(options.SecretKey))
            failures.Add("SecretKey is required");

        if (string.IsNullOrWhiteSpace(options.BaseUrl))
            failures.Add("BaseUrl is required");

        if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _))
            failures.Add("BaseUrl must be a valid URI");

        if (options.TimeoutInSeconds <= 0)
            failures.Add("TimeoutInSeconds must be greater than zero");

        if (failures.Count > 0)
            return ValidateOptionsResult.Fail(failures);

        return ValidateOptionsResult.Success;
    }
}
