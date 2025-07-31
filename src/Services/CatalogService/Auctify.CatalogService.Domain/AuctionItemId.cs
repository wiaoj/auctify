using Auctify.Libraries.Domain.Abstractions.ValueObjects;
using Wiaoj;

namespace Auctify.CatalogService.Domain;
public sealed record AuctionItemId : IId<AuctionItemId, Guid> {
    public Guid Value { get; }

    private AuctionItemId() { }
    private AuctionItemId(Guid value) {
        this.Value = value;
    }

    public static AuctionItemId From(Guid value) {
        Preca.ThrowIfDefault(value);
        return new(value);
    }

    public static AuctionItemId New() {
        return new(Guid.CreateVersion7());
    }
}