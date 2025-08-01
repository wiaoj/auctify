using Auctify.Libraries.CurrencyExchange.Client.Abstractions;
using Auctify.Libraries.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Wiaoj;

namespace Auctify.Libraries.CurrencyExchange.Client;
/// <summary>
/// The main implementation of ICurrencyConverter. It acts as a facade, delegating the
/// complex task of rate retrieval to a chain of IExchangeRateProvider decorators.
/// Its primary responsibility is to orchestrate the conversion process.
/// </summary>
internal sealed class CurrencyConverter(IExchangeRateProvider rateProvider, ILogger<CurrencyConverter> logger) : ICurrencyConverter {
    public async Task<ConversionResult> ConvertAsync(Money source, Currency targetCurrency, CancellationToken cancellationToken = default) {
        Preca.ThrowIfNull(source);
        Preca.ThrowIfNull(targetCurrency);

        if(source.Currency == targetCurrency)
            return ConversionResult.Success(source);

        CurrencyPair pair = CurrencyPair.New(source.Currency, targetCurrency);

        try {
            ExchangeRate? rate = await rateProvider.GetRateAsync(pair, cancellationToken);

            if(rate is not null) {
                Money convertedMoney = source * rate;
                return ConversionResult.Success(convertedMoney);
            }
             
            logger.LogError("Could not retrieve exchange rate for {pair}.", pair);
            return ConversionResult.Fail($"Could not retrieve exchange rate for {pair}.");
        }
        catch(Exception ex) {
            logger.LogError(ex, "An unexpected error occurred during currency conversion for {Pair}.", pair);
            return ConversionResult.Fail(ex.Message);
        }
    }
}