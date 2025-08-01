using Auctify.Libraries.Domain.ValueObjects;
using StackExchange.Redis;

namespace Auctify.CurrencyExchangeService.WebAPI.Extensions;
internal static class RedisMappingExtensions {
    public static HashEntry ToHashEntry(this ExchangeRate rate) {
        return new HashEntry(rate.Pair.ToString(), (double)rate.Value.Value);
    }

    public static HashEntry[] ToHashEntries(this IEnumerable<ExchangeRate> rates) {
        return [.. rates.Select(rate => rate.ToHashEntry())];
    }
}