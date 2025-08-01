using System.Numerics;
using Auctify.Libraries.Domain.Abstractions.ValueObjects;
using Auctify.Libraries.Domain.Extensions;
using Wiaoj;

namespace Auctify.Libraries.Domain.ValueObjects;
public sealed record Amount : IValueObject<Amount, decimal>,
    IAdditionOperators<Amount, Amount, Amount>,
    ISubtractionOperators<Amount, Amount, Amount>,
    IMultiplyOperators<Amount, Amount, Amount>,
    IMultiplyOperators<Amount, Rate, Amount>,
    IDivisionOperators<Amount, Amount, Amount>,
    IComparisonOperators<Amount, Amount, bool>,
    IEqualityOperators<Amount, Amount, bool> {
    public decimal Value { get; init; }
    private const int Decimals = 2;

    private Amount() { }
    private Amount(decimal value) {
        this.Value = value;
    }

    public static Amount New(decimal value) {
        Preca.ThrowIfNegative(value);
        return new(Math.Round(value, Decimals));
    }

    public static Amount operator +(Amount left, Amount right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return New(left.Value + right.Value);
    }

    public static Amount operator *(Amount left, Amount right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return New(left.Value * right.Value);
    }

    public static Amount operator *(Amount amount, Rate rate) {
        Preca.ThrowIfNull(amount);
        Preca.ThrowIfNull(rate);

        decimal newValue = amount.Value * rate.Value;
        return New(newValue);
    }

    public static Amount operator -(Amount left, Amount right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return New(left.Value - right.Value);
    }

    public static Amount operator /(Amount left, Amount right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return New(left.Value / right.Value);
    }

    public static bool operator >(Amount left, Amount right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return left.Value > right.Value;
    }

    public static bool operator >=(Amount left, Amount right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return left.Value >= right.Value;
    }

    public static bool operator <(Amount left, Amount right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return left.Value < right.Value;
    }

    public static bool operator <=(Amount left, Amount right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return left.Value <= right.Value;
    }
}