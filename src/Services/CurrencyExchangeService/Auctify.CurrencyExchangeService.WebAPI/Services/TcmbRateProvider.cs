using System.Xml.Serialization;
using Auctify.CurrencyExchangeService.WebAPI.Constants;
using Auctify.CurrencyExchangeService.WebAPI.Models;
using Auctify.CurrencyExchangeService.WebAPI.Services.Abstractions;
using Auctify.Libraries.Domain.ValueObjects;

namespace Auctify.CurrencyExchangeService.WebAPI.Services;
internal sealed class TcmbRateProvider(HttpClient httpClient, ILogger<TcmbRateProvider> logger) : IBaseRateProvider { 
    public async Task<IEnumerable<ExchangeRate>> GetBaseRatesInTryAsync() {
        try { 
            using Stream stream = await httpClient.GetStreamAsync(TcmbConstants.TodayRatesEndpoint);
            XmlSerializer serializer = new(typeof(TcmbCurrencyRates));
            TcmbCurrencyRates deserializedData = (TcmbCurrencyRates)serializer.Deserialize(stream)!;

            if(deserializedData == null) return [];

            IEnumerable<ExchangeRate> rates = deserializedData.Currencies
                .Where(c => SupportedCurrencies.Codes.Contains(c.Code) && c.ForexSelling > 0)
                .Select(c => {
                    decimal rateValue = c.Unit > 1 ? c.ForexSelling / c.Unit : c.ForexSelling;
                    Currency fromCurrency = Currency.New(c.Code);
                    CurrencyPair pair = CurrencyPair.New(fromCurrency, Currency.TRY);
                    Rate rate = Rate.New(rateValue);
                    return ExchangeRate.New(pair, rate);
                });

            return [.. rates];
        }
        catch(Exception ex) {
            logger.LogError(ex, "Failed to fetch or deserialize base rates from TCMB.");
            return [];
        }
    }
}