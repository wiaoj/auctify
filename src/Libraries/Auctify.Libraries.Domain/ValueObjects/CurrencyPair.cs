using System.Diagnostics;
using Auctify.Libraries.Domain.Abstractions.ValueObjects;
using Auctify.Libraries.Domain.Extensions;
using Wiaoj;

namespace Auctify.Libraries.Domain.ValueObjects;
[DebuggerDisplay("{ToString(),nq}")]
public sealed record CurrencyPair : IValueObject<CurrencyPair, Currency, Currency> {
    public Currency From { get; }
    public Currency To { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private CurrencyPair() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private CurrencyPair(Currency from, Currency to) {
        this.From = from;
        this.To = to;
    }

    public static CurrencyPair New(Currency c1, Currency c2) {
        Preca.Extensions.ThrowIfLeftOrRightNull(c1, c2);
        Preca.Extensions.ThrowIfEquals(c1, c2);
        return new(c1, c2);
    }

    public override string ToString() {
        return $"{this.From.Value}:{this.To.Value}";
    }
}