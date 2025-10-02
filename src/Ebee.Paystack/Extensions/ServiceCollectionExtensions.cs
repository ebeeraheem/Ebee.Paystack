using Ebee.Paystack.Clients.Banks;
using Ebee.Paystack.Clients.Transactions;
using Ebee.Paystack.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ebee.Paystack.Extensions;

/// <summary>
/// Extension methods for configuring Paystack services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Paystack services to the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration section containing Paystack settings</param>
    /// <returns>The service collection for chaining</returns>
    /// <example>
    /// <code>
    /// services.AddPaystack(configuration.GetSection("Paystack"));
    /// </code>
    /// </example>
    public static IServiceCollection AddPaystack(this IServiceCollection services, IConfigurationSection configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        // Configure PaystackConfiguration
        services.Configure<PaystackConfiguration>(configuration);

        // Validate configuration
        services.AddSingleton<IValidateOptions<PaystackConfiguration>, PaystackConfigurationValidator>();
        
        // Register HTTP client
        services.AddHttpClient<IPaystackHttpClient, PaystackHttpClient>();

        // Register API clients
        services.AddScoped<IBanksClient, BanksClient>();
        services.AddScoped<ITransactionsClient, TransactionsClient>();

        // Register main client
        services.AddScoped<IPaystackClient, PaystackClient>();

        return services;
    }

    /// <summary>
    /// Adds Paystack services to the dependency injection container with configuration action
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Action to configure Paystack options</param>
    /// <returns>The service collection for chaining</returns>
    /// <example>
    /// <code>
    /// services.AddPaystack(options =>
    /// {
    ///     options.SecretKey = "sk_test_...";
    ///     options.EnableLogging = true;
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddPaystack(this IServiceCollection services, Action<PaystackConfiguration> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        // Configure PaystackConfiguration
        services.Configure(configureOptions);
        
        // Validate configuration
        services.AddSingleton<IValidateOptions<PaystackConfiguration>, PaystackConfigurationValidator>();

        // Register HTTP client
        services.AddHttpClient<IPaystackHttpClient, PaystackHttpClient>();

        // Register API clients
        services.AddScoped<IBanksClient, BanksClient>();
        services.AddScoped<ITransactionsClient, TransactionsClient>();

        // Register main client
        services.AddScoped<IPaystackClient, PaystackClient>();

        return services;
    }
}
