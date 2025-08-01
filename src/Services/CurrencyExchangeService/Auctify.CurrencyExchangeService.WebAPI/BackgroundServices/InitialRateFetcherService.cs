using Auctify.CurrencyExchangeService.WebAPI.Jobs;
using Auctify.CurrencyExchangeService.WebAPI.Services.Abstractions;
using Hangfire;

namespace Auctify.CurrencyExchangeService.WebAPI.BackgroundServices;
public sealed class InitialRateFetcherService(IServiceProvider serviceProvider, ILogger<InitialRateFetcherService> logger) : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        logger.LogInformation("Initial Rate Fetcher is running.");

        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

        using IServiceScope scope = serviceProvider.CreateScope();
        IExchangeRateRepository rateRepo = scope.ServiceProvider.GetRequiredService<IExchangeRateRepository>();
        IBackgroundJobClient backgroundJobClient = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();

        if(!await rateRepo.HasRatesAsync()) {
            logger.LogInformation("Initial rate data not found. Triggering immediate background job.");
            backgroundJobClient.Enqueue<ExchangeRateFetcherJob>(job => job.ExecuteAsync());
        }
    }
}