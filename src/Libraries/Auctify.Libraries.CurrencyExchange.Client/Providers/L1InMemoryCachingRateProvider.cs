using Auctify.Libraries.CurrencyExchange.Client.Abstractions;
using Auctify.Libraries.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Auctify.Libraries.CurrencyExchange.Client.Providers;
internal sealed class L1InMemoryCachingRateProvider(
    IExchangeRateProvider nextProvider,
    IMemoryCache memoryCache,
    ILogger<L1InMemoryCachingRateProvider> logger) : IExchangeRateProvider {
    private readonly MemoryCacheEntryOptions memoryCacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

    public async Task<ExchangeRate?> GetRateAsync(CurrencyPair pair, CancellationToken cancellationToken) {
        if(memoryCache.TryGetValue(pair.ToString(), out ExchangeRate? rate)) {
            logger.LogDebug("Rate for {Pair} found in L1 (In-Memory) cache.", pair);
            return rate;
        }

        logger.LogTrace("Rate for {Pair} not found in L1 cache. Delegating to next provider.", pair);
        ExchangeRate? rateFromNext = await nextProvider.GetRateAsync(pair, cancellationToken);

        if(rateFromNext is not null) {
            logger.LogDebug("Populating L1 cache for {Pair}.", pair);
            memoryCache.Set(pair.ToString(), rateFromNext, this.memoryCacheOptions);
        }

        return rateFromNext;
    }
}