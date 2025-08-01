using System.Net;
using Auctify.Libraries.CurrencyExchange.Client.Abstractions;
using Auctify.Libraries.CurrencyExchange.Client.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using StackExchange.Redis;
using Wiaoj;

namespace Auctify.Libraries.CurrencyExchange.Client;
/// <summary>
/// Provides extension methods for setting up the Auctify Currency Converter client in an <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjection {
    /// <summary>
    /// Adds the Auctify Currency Converter client to the DI container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configureClient">An action to configure the underlying <see cref="HttpClient"/>.
    /// This is typically used to set the <see cref="HttpClient.BaseAddress"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddAuctifyCurrencyConverter(this IServiceCollection services, Action<HttpClient> configureClient) {
        Preca.ThrowIfNull(services);
        Preca.ThrowIfNull(configureClient);

        services.AddMemoryCache();

        // 1. API Client
        services.AddHttpClient<L3ApiExchangeRateProvider>(configureClient)
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy());
         
        // 2. Redis 
        services.AddScoped<L2RedisCachingRateProvider>(provider =>
            new L2RedisCachingRateProvider(
                provider.GetRequiredService<L3ApiExchangeRateProvider>(),
                provider.GetRequiredService<IConnectionMultiplexer>(),
                provider.GetRequiredService<ILogger<L2RedisCachingRateProvider>>()
            ));

        // 3. In-Memory 
        services.AddScoped<IExchangeRateProvider, L1InMemoryCachingRateProvider>(provider =>
            new L1InMemoryCachingRateProvider(
                provider.GetRequiredService<L2RedisCachingRateProvider>(),
                provider.GetRequiredService<IMemoryCache>(),
                provider.GetRequiredService<ILogger<L1InMemoryCachingRateProvider>>()
            ));

        services.AddScoped<ICurrencyConverter, CurrencyConverter>();

        return services;
    }

    private static Polly.Retry.AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy() {
        // Defines a retry policy that handles transient HTTP errors.
        // It will retry 3 times with an exponential backoff (1s, 2s, 4s).
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound) // Also retry on 404 Not Found
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}