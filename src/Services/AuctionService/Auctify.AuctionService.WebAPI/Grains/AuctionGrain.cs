namespace Auctify.AuctionService.WebAPI.Grains;
public sealed class AuctionGrain : Grain, IAuctionGrain {
    private decimal _currentPrice = 100;
    private readonly string _title = "Van Gogh Tablosu";

    public Task<decimal> GetCurrentPrice() {
        return Task.FromResult(this._currentPrice);
    }

    public Task<bool> PlaceBid(decimal newPrice) {
        if (newPrice > this._currentPrice) {
            this._currentPrice = newPrice;
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public Task<AuctionDetails> GetAuctionDetails() {
        return Task.FromResult(new AuctionDetails {
            Title = this._title,
            CurrentPrice = this._currentPrice
        });
    }
}