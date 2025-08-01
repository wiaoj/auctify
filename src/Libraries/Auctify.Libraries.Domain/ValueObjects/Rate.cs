using System.Diagnostics;
using System.Numerics;
using Auctify.Libraries.Domain.Abstractions.ValueObjects;
using Auctify.Libraries.Domain.Extensions;
using Wiaoj;

namespace Auctify.Libraries.Domain.ValueObjects;
[DebuggerDisplay("{ToString(),nq}")]
public sealed record Rate : IValueObject<Rate, decimal>,
    IValueObject<Rate, decimal, int>,
    IComparisonOperators<Rate, Rate, bool>,
    IEqualityOperators<Rate, Rate, bool> {
    public decimal Value { get; init; }
    private const int Decimals = 6;

    private Rate() { }
    private Rate(decimal value) {
        this.Value = value;
    }

    public static Rate New(decimal value) {
        return New(value, Decimals);
    } 

    public static Rate New(decimal value, int decimals) {
        Preca.ThrowIfNegative(value);
        return new(Math.Round(value, decimals));
    }  

    public static bool operator >(Rate left, Rate right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return left.Value > right.Value;
    }

    public static bool operator >=(Rate left, Rate right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return left.Value >= right.Value;
    }

    public static bool operator <(Rate left, Rate right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return left.Value < right.Value;
    }

    public static bool operator <=(Rate left, Rate right) {
        Preca.Extensions.ThrowIfLeftOrRightNull(left, right);
        return left.Value <= right.Value;
    }
}