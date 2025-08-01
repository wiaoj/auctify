using Auctify.Libraries.Domain.ValueObjects;
using Wiaoj;

namespace Auctify.Libraries.CurrencyExchange.Client.Abstractions;
/// <summary>
/// Represents the result of a currency conversion operation.
/// </summary>
/// <param name="IsSuccess">Indicates whether the operation was successful.</param>
/// <param name="ConvertedMoney">The resulting Money object if the operation was successful.</param>
/// <param name="Error">The error message if the operation failed.</param>
public sealed record ConversionResult(bool IsSuccess, Money? ConvertedMoney, string? Error = null) {
    /// <summary>
    /// Creates a successful conversion result.
    /// </summary>
    public static ConversionResult Success(Money money) {
        Preca.ThrowIfNull(money);
        return new ConversionResult(true, money);
    }

    /// <summary>
    /// Creates a failed conversion result.
    /// </summary>
    public static ConversionResult Fail(string error) {
        Preca.ThrowIfNullOrWhiteSpace(error);
        return new ConversionResult(false, null, error);
    }
}