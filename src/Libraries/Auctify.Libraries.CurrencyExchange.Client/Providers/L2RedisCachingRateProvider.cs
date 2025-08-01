using Auctify.Libraries.CurrencyExchange.Client.Abstractions;
using Auctify.Libraries.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Wiaoj;

namespace Auctify.Libraries.CurrencyExchange.Client.Providers;
/// <summary>
/// A decorator that adds a Redis caching layer (L2 Cache) on top of another IExchangeRateProvider.
/// It attempts to resolve the rate from Redis first. If not found, it delegates the call
/// to the next provider in the chain and caches the result back into Redis.
/// </summary>
internal sealed class L2RedisCachingRateProvider(
    IExchangeRateProvider nextProvider,
    IConnectionMultiplexer redis,
    ILogger<L2RedisCachingRateProvider> logger) : IExchangeRateProvider {
    private const string CurrentRatesKeyPointer = "exchange-rates:pointer";

    public async Task<ExchangeRate?> GetRateAsync(CurrencyPair pair, CancellationToken cancellationToken) {
        Preca.ThrowIfNull(pair);

        try {
            // 1. Attempt to get the rate from Redis cache.
            ExchangeRate? rateFromRedis = await GetFromRedisAsync(pair);
            if(rateFromRedis is not null) {
                logger.LogDebug("Rate for {Pair} found in L2 (Redis) cache.", pair);
                return rateFromRedis;
            }

            // 2. Cache miss: Delegate to the next provider in the chain (e.g., the API provider).
            logger.LogWarning("Rate for {Pair} not found in Redis. Delegating to the next provider.", pair);
            ExchangeRate? rateFromNextProvider = await nextProvider.GetRateAsync(pair, cancellationToken);

            // 3. Backfill: If the next provider returned a rate, cache it in Redis for future requests.
            if(rateFromNextProvider is not null) {
                // This is a fire-and-forget operation; we don't need to wait for it.
                _ = BackfillRedisAsync(pair, rateFromNextProvider);
            }

            return rateFromNextProvider;
        }
        catch(Exception ex) {
            // If Redis itself fails, we must still try the next provider.
            logger.LogError(ex, "An error occurred while accessing Redis cache. Bypassing cache and delegating to the next provider.");
            return await nextProvider.GetRateAsync(pair, cancellationToken);
        }
    }

    private Task<RedisValue> GetActiveHashKeyAsync() {
        return redis.GetDatabase().StringGetAsync(CurrentRatesKeyPointer);
    }

    private async Task<ExchangeRate?> GetFromRedisAsync(CurrencyPair pair) {
        IDatabase db = redis.GetDatabase();
        string? activeHashKey = await GetActiveHashKeyAsync();

        if(string.IsNullOrEmpty(activeHashKey)) {
            logger.LogTrace("Redis pointer key '{Key}' not found.", CurrentRatesKeyPointer);
            return null;
        }

        RedisValue rateValue = await db.HashGetAsync(activeHashKey, pair.ToString());
        if(!rateValue.HasValue) {
            logger.LogTrace("Rate for {Pair} not found in Redis hash '{HashKey}'.", pair, activeHashKey);
            return null;
        }

        return ExchangeRate.New(pair, Rate.New((decimal)rateValue));
    }

    private async Task BackfillRedisAsync(CurrencyPair pair, ExchangeRate rate) {
        try {
            IDatabase db = redis.GetDatabase();
            string? activeHashKey = await GetActiveHashKeyAsync();

            if(!string.IsNullOrEmpty(activeHashKey)) {
                // Use When.NotExists to prevent overwriting a value that might have been
                // set by another process in a race condition.
                bool added = await db.HashSetAsync(activeHashKey, pair.ToString(), (double)rate.Value.Value, When.NotExists);
                if(added) {
                    logger.LogInformation("Backfilled rate for {Pair} into Redis cache key {Key}.", pair, activeHashKey);
                }
            }
        }
        catch(Exception ex) {
            // Log the error but don't let it crash the main flow.
            logger.LogError(ex, "Failed to backfill Redis cache for {Pair}.", pair);
        }
    }
}