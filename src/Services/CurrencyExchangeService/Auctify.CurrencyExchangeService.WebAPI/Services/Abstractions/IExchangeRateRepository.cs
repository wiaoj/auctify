using Auctify.Libraries.Domain.ValueObjects;

namespace Auctify.CurrencyExchangeService.WebAPI.Services.Abstractions;
public interface IExchangeRateRepository { 
    Task<ExchangeRate?> GetRateAsync(CurrencyPair pair);
    Task SaveAllRatesAsync(IEnumerable<ExchangeRate> rates, TimeProvider timeProvider);
    Task<bool> HasRatesAsync(); 
    Task<IEnumerable<ExchangeRate>> GetAllCurrentRatesAsync();
}