namespace Auctify.CurrencyExchangeService.WebAPI.Constants;
internal static class RedisKeys {
    public const string CurrentRatesPointer = "exchange-rates:pointer";
    public static string DataHashKeyPrefix => "exchange-rates:data:";
    public static string CreateDataHashKey(TimeProvider timeProvider) => $"{DataHashKeyPrefix}{timeProvider.GetUtcNow():yyyyMMddHHmmss}";
}