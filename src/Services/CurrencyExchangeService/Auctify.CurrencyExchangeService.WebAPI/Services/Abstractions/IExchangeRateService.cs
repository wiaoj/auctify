using Auctify.Libraries.Domain.ValueObjects;

namespace Auctify.CurrencyExchangeService.WebAPI.Services.Abstractions;
public interface IExchangeRateService { 
    Task<ExchangeRate?> GetRateAsync(Currency from, Currency to);
    Task RefreshAllRatesAsync();
    Task<IEnumerable<ExchangeRate>> GetAllCurrentRatesAsync();
} 