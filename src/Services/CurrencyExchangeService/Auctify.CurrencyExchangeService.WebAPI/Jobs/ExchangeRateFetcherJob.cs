using Auctify.CurrencyExchangeService.WebAPI.Services.Abstractions;

namespace Auctify.CurrencyExchangeService.WebAPI.Jobs;
public sealed class ExchangeRateFetcherJob(IExchangeRateService exchangeRateService, ILogger<ExchangeRateFetcherJob> logger) {
    public async Task ExecuteAsync() {
        logger.LogInformation("ExchangeRateFetcherJob triggered. Calling RefreshAllRatesAsync.");
        await exchangeRateService.RefreshAllRatesAsync();
        logger.LogInformation("ExchangeRateFetcherJob finished.");
    }
}