namespace Auctify.CurrencyExchangeService.WebAPI.Constants;
internal static class SupportedCurrencies {
    public static readonly IReadOnlySet<string> Codes = new HashSet<string> {
        "USD",
        "EUR",
        "TRY"
    };
}