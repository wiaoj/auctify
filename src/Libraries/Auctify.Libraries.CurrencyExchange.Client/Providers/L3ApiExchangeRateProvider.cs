using System.Net.Http.Json;
using Auctify.Libraries.CurrencyExchange.Client.Abstractions;
using Auctify.Libraries.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Auctify.Libraries.CurrencyExchange.Client.Providers;
internal sealed class L3ApiExchangeRateProvider(HttpClient httpClient, ILogger<L3ApiExchangeRateProvider> logger) : IExchangeRateProvider {
    public async Task<ExchangeRate?> GetRateAsync(CurrencyPair pair, CancellationToken cancellationToken) {
        try {
            HttpResponseMessage response = await httpClient.GetAsync($"api/rates/{pair.From.Value}/{pair.To.Value}", cancellationToken);

            if(!response.IsSuccessStatusCode) {
                logger.LogError("CurrencyExchange API failed with status code {StatusCode} for {Pair}.", response.StatusCode, pair);
                return null;
            }

            decimal rateDecimal = await response.Content.ReadFromJsonAsync<decimal>(cancellationToken: cancellationToken);
            return ExchangeRate.New(pair, Rate.New(rateDecimal));
        }
        catch(HttpRequestException ex) {
            logger.LogError(ex, "HTTP request to CurrencyExchange API failed for {Pair}.", pair);
            return null;
        }
    }
}