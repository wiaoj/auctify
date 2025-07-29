namespace Auctify.AuctionService.WebAPI.Grains;
[Alias("Auctify.AuctionService.WebAPI.Grains.IAuctionGrain")]
public interface IAuctionGrain : IGrainWithStringKey {
    [Alias(nameof(PlaceBid))]
    Task<bool> PlaceBid(decimal newPrice);
    [Alias(nameof(GetCurrentPrice))]
    Task<decimal> GetCurrentPrice();

    [Alias(nameof(GetAuctionDetails))]
    Task<AuctionDetails> GetAuctionDetails();
}

[GenerateSerializer]
[Alias("Auctify.AuctionService.WebAPI.Grains.AuctionDetails")]
public sealed record AuctionDetails {
    [Id(0)]
    public required string Title { get; init; }
    [Id(1)]
    public decimal CurrentPrice { get; init; }
}