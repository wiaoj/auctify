using Auctify.Libraries.Domain.ValueObjects;

namespace Auctify.Libraries.CurrencyExchange.Client.Abstractions;
/// <summary>
/// Defines the primary interface for converting currencies, to be used by other services.
/// </summary>
public interface ICurrencyConverter {
    /// <summary>
    /// Converts a given amount of money from a source currency to a target currency.
    /// </summary>
    /// <param name="source">The source money to be converted.</param>
    /// <param name="targetCurrency">The target currency to convert to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="ConversionResult"/> containing the outcome of the conversion operation.</returns>
    Task<ConversionResult> ConvertAsync(Money source, Currency targetCurrency, CancellationToken cancellationToken = default);
}