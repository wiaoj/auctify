using System.Numerics;
using Auctify.Libraries.Domain.Abstractions.ValueObjects;
using Auctify.Libraries.Domain.Extensions;
using Wiaoj;
using Wiaoj.Extensions;

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
        // Amount ve Currency'nin null olamayacağını garanti altına alıyoruz.
        this.Amount = Preca.Extensions.ThrowIfNull(amount);
        this.Currency = Preca.Extensions.ThrowIfNull(currency);
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

    public override string ToString() {
        return $"{this.Amount.Value:F2} {this.Currency.Value}";
    }
}