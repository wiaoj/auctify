using Orleans.Providers;

namespace Auctify.BiddingService.WebAPI.Grains;
[GrainType("bidding")]
[StorageProvider(ProviderName = "AuctifyStorage")]
public sealed class BiddingGrain : Grain<BiddingState>, IBiddingGrain {
    public async Task<bool> PlaceBid(decimal amount) {
        if(amount > this.State.CurrentPrice) {
            this.State.CurrentPrice = amount;
            await WriteStateAsync();
            return true;
        }

        return false;
    }

    public Task<decimal> GetCurrentPrice() {
        return Task.FromResult(this.State.CurrentPrice);
    }
}

[GenerateSerializer]
[Alias("Auctify.BiddingService.WebAPI.Grains.BiddingState")]
public sealed record BiddingState {
    private const decimal InitialPrice = 100;

    [Id(0)]
    public decimal CurrentPrice { get; set; } = InitialPrice;
}