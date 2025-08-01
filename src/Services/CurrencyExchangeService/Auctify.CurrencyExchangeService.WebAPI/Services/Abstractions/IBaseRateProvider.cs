using Auctify.Libraries.Domain.ValueObjects;

namespace Auctify.CurrencyExchangeService.WebAPI.Services.Abstractions;
public interface IBaseRateProvider {
    Task<IEnumerable<ExchangeRate>> GetBaseRatesInTryAsync();
}