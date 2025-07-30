using Auctify.Libraries.Domain.Abstractions;

namespace Auctify.CatalogService.Domain;
public sealed class AuctionItem : Aggregate<AuctionItemId> {
    public AuctionItem() {
    }

#pragma warning disable CS0628 // New protected member declared in sealed type
    protected AuctionItem(AuctionItemId id) : base(id) { }
#pragma warning restore CS0628 // New protected member declared in sealed type
}