namespace Auctify.AuctionService.WebAPI.Grains;
public sealed class AuctionGrain : Grain, IAuctionGrain {
    private decimal _currentPrice = 100; 

    public Task<decimal> GetCurrentPrice() {
        return Task.FromResult(_currentPrice);
    }

    public Task<bool> PlaceBid(decimal newPrice) {
        if (newPrice > _currentPrice) {
            _currentPrice = newPrice;
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}