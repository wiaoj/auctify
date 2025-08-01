namespace Auctify.CurrencyExchangeService.WebAPI.Constants;
internal static class RedisKeys {
    public const string CurrentRatesPointer = "exchange-rates:pointer";
    public static string DataHashKeyPrefix => "exchange-rates:data:";
    public static string CreateDataHashKey() => $"{DataHashKeyPrefix}{DateTime.UtcNow:yyyyMMddHHmmss}";
}