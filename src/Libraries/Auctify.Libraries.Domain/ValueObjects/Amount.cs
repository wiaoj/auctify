using System.Numerics;
using Auctify.Libraries.Domain.Abstractions.ValueObjects;
using Auctify.Libraries.Domain.Extensions;
using Wiaoj;

namespace Auctify.Libraries.Domain.ValueObjects;
public sealed record Amount : IValueObject<Amount, decimal>,
    IAdditionOperators<Amount, Amount, Amount>,
    ISubtractionOperators<Amount, Amount, Amount>,
    IMultiplyOperators<Amount, Amount, Amount>,
    IDivisionOperators<Amount, Amount, Amount>,
    IComparisonOperators<Amount, Amount, bool>,
    IEqualityOperators<Amount, Amount, bool> {
    public decimal Value { get; init; }

    private Amount() { }
    private Amount(decimal value) { 
        Value = value;
    }

    public static Amount New(decimal value) {
        Preca.ThrowIfNegative(value); 
        return new(Math.Round(value, 2));
    }

    public static Amount operator +(Amount left, Amount right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return New(left.Value + right.Value);
    }

    public static Amount operator *(Amount left, Amount right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return New(left.Value * right.Value);
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