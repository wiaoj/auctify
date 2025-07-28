namespace Auctify.AuctionService.WebAPI.Grains;
[Alias("Auctify.AuctionService.WebAPI.Grains.IAuctionGrain")]
public interface IAuctionGrain : IGrainWithStringKey {
    [Alias(nameof(PlaceBid))]
    Task<bool> PlaceBid(decimal newPrice);
    [Alias(nameof(GetCurrentPrice))]
    Task<decimal> GetCurrentPrice();
}