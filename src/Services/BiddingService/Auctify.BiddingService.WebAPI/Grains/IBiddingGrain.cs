namespace Auctify.BiddingService.WebAPI.Grains; 
[Alias("Auctify.BiddingService.WebAPI.Grains.IBiddingGrain")]
public interface IBiddingGrain : IGrainWithStringKey {
    [Alias(nameof(PlaceBid))]
    Task<bool> PlaceBid(decimal newPrice);
    [Alias(nameof(GetCurrentPrice))]
    Task<decimal> GetCurrentPrice();
}