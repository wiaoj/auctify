using Auctify.Libraries.Domain.Abstractions;

namespace Auctify.CatalogService.Domain;
public sealed class AuctionItem : Aggregate<AuctionItemId> {
    public string Name { get; private set; } = null!;
    private AuctionItem() { }

#pragma warning disable CS0628 // New protected member declared in sealed type
    protected AuctionItem(AuctionItemId id) : base(id) { }
#pragma warning restore CS0628 // New protected member declared in sealed type

    public static AuctionItem New(string name) {
        AuctionItem item = new(AuctionItemId.New());
        item.SetCreatedAt(DateTimeOffset.UtcNow);
        item.Name = name;
        return item;
    } 
}