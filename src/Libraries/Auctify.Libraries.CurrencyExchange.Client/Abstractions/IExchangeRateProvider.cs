using Auctify.Libraries.Domain.ValueObjects;

namespace Auctify.Libraries.CurrencyExchange.Client.Abstractions;
internal interface IExchangeRateProvider {
    Task<ExchangeRate?> GetRateAsync(CurrencyPair pair, CancellationToken cancellationToken);
}