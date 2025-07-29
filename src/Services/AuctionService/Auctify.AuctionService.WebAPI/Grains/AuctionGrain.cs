using Orleans.Providers;

namespace Auctify.AuctionService.WebAPI.Grains;
[GrainType("auction")]
[StorageProvider(ProviderName = "AuctifyStorage")]
public sealed class AuctionGrain : Grain<AuctionState>, IAuctionGrain {
    public async Task<bool> PlaceBid(decimal amount) {
        if (amount > this.State.CurrentPrice) {
            this.State.CurrentPrice = amount;
            await WriteStateAsync();
            return true;
        }

        return false;
    }

    public Task<AuctionDetails> GetAuctionDetails() {
        return Task.FromResult(new AuctionDetails {
            Title = this.State.Title,
            CurrentPrice = this.State.CurrentPrice
        });
    }
}