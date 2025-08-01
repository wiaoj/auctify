using Auctify.Libraries.Domain.ValueObjects;

namespace Auctify.CurrencyExchangeService.WebAPI.Services.Abstractions;
public interface ICrossRateCalculator { 
    IEnumerable<ExchangeRate> Calculate(IEnumerable<ExchangeRate> baseRatesInTry);
}