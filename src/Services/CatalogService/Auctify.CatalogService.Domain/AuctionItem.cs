using Auctify.Libraries.Domain.Abstractions;
using Wiaoj;

namespace Auctify.CatalogService.Domain;
public sealed class AuctionItem : Aggregate<AuctionItemId> {
    public string Title { get; private set; } = null!;
    public decimal StartingPrice { get; private set; }
    public decimal? CurrentPrice { get; private set; }

    private AuctionItem() { }
    private AuctionItem(AuctionItemId id) : base(id) { }

    public static AuctionItem New(AuctionItemId id, string name) {
        AuctionItem item = new(id);
        item.SetCreatedAt(DateTimeOffset.UtcNow);

        item.Title = name;
        item.StartingPrice = 100;
        item.CurrentPrice = null;

        return item;
    }

    public void SetCurrentPrice(decimal newPrice) {
        Preca.ThrowIfZeroOrNegative(newPrice);
        if(this.CurrentPrice is null || newPrice > this.CurrentPrice) {
            this.CurrentPrice = newPrice;
            SetUpdatedAt(DateTimeOffset.UtcNow);
        }
        else {
            throw new InvalidOperationException("New price must be greater than the current price.");
        }
    }
}