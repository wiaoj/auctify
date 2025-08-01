using Auctify.CurrencyExchangeService.WebAPI.Constants;
using Auctify.CurrencyExchangeService.WebAPI.Extensions;
using Auctify.CurrencyExchangeService.WebAPI.Services.Abstractions;
using Auctify.Libraries.Domain.ValueObjects;
using StackExchange.Redis;

namespace Auctify.CurrencyExchangeService.WebAPI.Services;
internal sealed class RedisExchangeRateRepository(IConnectionMultiplexer redis, TimeProvider timeProvider) : IExchangeRateRepository { 
    private async Task<string?> GetActiveHashKeyAsync() {
        return await redis.GetDatabase().StringGetAsync(RedisKeys.CurrentRatesPointer);
    }

    public async Task<bool> HasRatesAsync() {
        return !string.IsNullOrEmpty(await GetActiveHashKeyAsync());
    }

    public async Task<ExchangeRate?> GetRateAsync(CurrencyPair pair) {
        IDatabase db = redis.GetDatabase();
        string? activeKey = await GetActiveHashKeyAsync();
        if(string.IsNullOrEmpty(activeKey)) return null;

        string field = pair.ToString();
        RedisValue rateValue = await db.HashGetAsync(activeKey, field);
        if(!rateValue.HasValue) return null;

        Rate rate = Rate.New((decimal)rateValue);
        return ExchangeRate.New(pair, rate);
    }

    public async Task<IEnumerable<ExchangeRate>> GetAllCurrentRatesAsync() {
        IDatabase db = redis.GetDatabase();
        string? activeKey = await GetActiveHashKeyAsync();
        if(string.IsNullOrEmpty(activeKey)) return [];

        HashEntry[] hashEntries = await db.HashGetAllAsync(activeKey);

        return [.. hashEntries.Select(CreateExchangeRate)];
    }

    private static ExchangeRate CreateExchangeRate(HashEntry entry) {
        CurrencyPair pair = createCurrencyPair(entry);
        Rate rate = Rate.New((decimal)entry.Value);
        return ExchangeRate.New(pair, rate);

        static CurrencyPair createCurrencyPair(HashEntry entry) {
            string[] currencies = entry.Name.ToString().Split(':');
            Currency from = Currency.New(currencies[0]);
            Currency to = Currency.New(currencies[1]);
            CurrencyPair pair = CurrencyPair.New(from, to);
            return pair;
        }
    }

    public async Task SaveAllRatesAsync(IEnumerable<ExchangeRate> rates) {
        IDatabase db = redis.GetDatabase(); 
        string newHashKey = RedisKeys.CreateDataHashKey(timeProvider);

        HashEntry[] hashEntries = rates.ToHashEntries();

        await db.HashSetAsync(newHashKey, hashEntries);
        await db.KeyExpireAsync(newHashKey, TimeSpan.FromDays(2));
        await db.StringSetAsync(RedisKeys.CurrentRatesPointer, newHashKey);
    }
}