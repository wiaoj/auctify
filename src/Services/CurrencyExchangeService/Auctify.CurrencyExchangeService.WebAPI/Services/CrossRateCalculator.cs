using Auctify.CurrencyExchangeService.WebAPI.Services.Abstractions;
using Auctify.Libraries.Domain.ValueObjects;
using StackExchange.Redis;

namespace Auctify.CurrencyExchangeService.WebAPI.Services;
internal sealed class CrossRateCalculator : ICrossRateCalculator {
    public IEnumerable<ExchangeRate> Calculate(IEnumerable<ExchangeRate> baseRatesInTry) {
        Dictionary<Currency, Rate> tryRates = baseRatesInTry
            .ToDictionary(x => x.Pair.From, x => x.Value);

        if(!tryRates.ContainsKey(Currency.TRY))
            tryRates[Currency.TRY] = Rate.New(1m);

        return [.. tryRates.Keys
            .SelectMany(
                fromCurrency => tryRates.Keys.Where(toCurrency => toCurrency != fromCurrency),
                (fromCurrency, toCurrency) => {
                    decimal cross = tryRates[fromCurrency].Value / tryRates[toCurrency].Value;
                    return ExchangeRate.New(
                        CurrencyPair.New(fromCurrency, toCurrency),
                        Rate.New(cross)
                    );
                }
            )];
    }
}