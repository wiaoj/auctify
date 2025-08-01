using System.Numerics;
using Auctify.Libraries.Domain.Abstractions.ValueObjects;
using Auctify.Libraries.Domain.Extensions;
using Wiaoj;

namespace Auctify.Libraries.Domain.ValueObjects;
public sealed record Money : IValueObject<Money, Amount, Currency>,
    IAdditionOperators<Money, Money, Money>,
    ISubtractionOperators<Money, Money, Money> {
    public Amount Amount { get; }
    public Currency Currency { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Money() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private Money(Amount amount, Currency currency) {
        this.Amount = amount;
        this.Currency = currency;
    }

    public static Money New(Amount amount, Currency currency) {
        Preca.ThrowIfNull(amount);
        Preca.ThrowIfNull(currency);
        return new Money(amount, currency);
    }

    public static Money operator +(Money left, Money right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        Preca.ThrowIf(left.Currency != right.Currency);

        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        Preca.ThrowIf(left.Currency != right.Currency);
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    /// <summary>
    /// Converts this Money instance to another currency using a specified exchange rate.
    /// </summary>
    /// <param name="rate">The exchange rate to apply. The rate's 'From' currency must match this money's currency.</param>
    /// <returns>A new Money instance in the target currency.</returns>
    public static Money operator *(Money money, ExchangeRate rate) {
        Preca.ThrowIfNull(money);
        Preca.ThrowIfNull(rate);
        Preca.ThrowIf(money.Currency != rate.Pair.From,
            () => new ArgumentException("Money currency must match the 'From' currency of the exchange rate."));

        Amount newAmount = money.Amount * rate.Value;

        return new(newAmount, rate.Pair.To);
    }

    public override string ToString() {
        return $"{this.Amount.Value:F2} {this.Currency.Value}";
    }
}