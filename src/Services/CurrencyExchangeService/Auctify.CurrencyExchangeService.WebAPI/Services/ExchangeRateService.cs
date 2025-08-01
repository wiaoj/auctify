using Auctify.CurrencyExchangeService.WebAPI.Services.Abstractions;
using Auctify.Libraries.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace Auctify.CurrencyExchangeService.WebAPI.Services;
internal sealed class ExchangeRateService(
    IBaseRateProvider rateProvider,
    ICrossRateCalculator calculator,
    IExchangeRateRepository rateRepository,
    IMemoryCache memoryCache,
    ILogger<ExchangeRateService> logger) : IExchangeRateService {
    private const string InMemoryCacheKey = "LastKnownGoodRates";
    public async Task<ExchangeRate?> GetRateAsync(Currency from, Currency to) {
        CurrencyPair pair = CurrencyPair.New(from, to);
        ExchangeRate? rate = await rateRepository.GetRateAsync(pair);

        if(rate is not null) {
            return rate;
        }

        logger.LogWarning("Rate not found in Redis. Checking in-memory fallback cache...");
        if(memoryCache.TryGetValue(InMemoryCacheKey, out Dictionary<string, ExchangeRate>? lastKnownRates)) {
            if(lastKnownRates is null) {
                return null;
            }

            if(lastKnownRates.TryGetValue(pair.ToString(), out ExchangeRate? fallbackRate)) {
                logger.LogError("FALLBACK USED! Serving stale rate from in-memory cache.");
                return fallbackRate;
            }
        }
          
        return null;
    }

    public Task<IEnumerable<ExchangeRate>> GetAllCurrentRatesAsync() {
        return rateRepository.GetAllCurrentRatesAsync();
    }

    public async Task RefreshAllRatesAsync() {
        logger.LogInformation("Rate refresh process started.");
        IEnumerable<ExchangeRate> baseRates = await rateProvider.GetBaseRatesInTryAsync();
        if(!baseRates.Any()) {
            logger.LogWarning("Base rates could not be fetched. Aborting refresh.");
            return;
        }

        IEnumerable<ExchangeRate> crossRates = calculator.Calculate(baseRates);
        if(!crossRates.Any()) {
            logger.LogWarning("Cross rates could not be calculated. Aborting refresh.");
            return;
        }

        await rateRepository.SaveAllRatesAsync(crossRates);
        logger.LogInformation("Rate refresh process completed successfully.");

        MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(2));

        memoryCache.Set(InMemoryCacheKey, crossRates.ToDictionary(r => r.Pair.ToString()), cacheEntryOptions);

        logger.LogInformation("Rates successfully cached in both Redis and in-memory fallback.");
    }
}