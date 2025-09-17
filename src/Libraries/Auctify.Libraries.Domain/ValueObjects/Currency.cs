using System.Text.RegularExpressions;
using Auctify.Libraries.Domain.Abstractions.ValueObjects;
using Wiaoj;

namespace Auctify.Libraries.Domain.ValueObjects;
// Represents a currency code, typically a 3-letter ISO 4217 code.
// https://en.wikipedia.org/wiki/ISO_4217
public sealed partial record Currency : IValueObject<Currency, string> {
    public static readonly Currency USD = new("USD");
    public static readonly Currency EUR = new("EUR");
    public static readonly Currency TRY = new("TRY");
    public string Value { get; private set; }

    private Currency(string value) {
        this.Value = value;
    }

#pragma warning disable CS8618 
    private Currency() { }
#pragma warning restore CS8618 

    public static Currency New(string value) {
        Preca.ThrowIfNullOrWhiteSpace(value);
        string upperCode = value.ToUpperInvariant();

        Preca.ThrowIfFalse(
            ISO4217Regex().IsMatch(upperCode),
            static () => new ArgumentException("Currency code must be 3 alphabetic characters."));

        return new Currency(upperCode);
    }

    [GeneratedRegex("^[A-Z]{3}$")]
    private static partial Regex ISO4217Regex();
}