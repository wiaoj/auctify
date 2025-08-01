using System.Diagnostics;
using Auctify.Libraries.Domain.Abstractions.ValueObjects;
using Wiaoj;

namespace Auctify.Libraries.Domain.ValueObjects;
[DebuggerDisplay("{ToString(),nq}")]
public sealed record ExchangeRate : IValueObject<ExchangeRate, CurrencyPair, Rate> {
    public CurrencyPair Pair { get; }
    public Rate Value { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private ExchangeRate() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private ExchangeRate(CurrencyPair pair, Rate value) {
        this.Pair = pair;
        this.Value = value;
    }

    public static ExchangeRate New(CurrencyPair pair, Rate rate) {
        Preca.ThrowIfNull(pair);
        Preca.ThrowIfNull(rate);
        return new(pair, rate);
    }
} 