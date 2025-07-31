using Auctify.CatalogService.Application.Repositories;
using Auctify.CatalogService.Domain;

namespace Auctify.CatalogService.Application;
public sealed record GetAuctionItemByIdQuery(Guid Id);

public sealed record GetAuctionItemByIdQueryResult {
    public Guid Id { get; internal set; }
    public string Title { get; internal set; } = null!;
    public decimal CurrentPrice { get; internal set; }
    public DateTimeOffset CreatedAt { get; internal set; }

    public static GetAuctionItemByIdQueryResult From(AuctionItem auctionItem) {
        return new GetAuctionItemByIdQueryResult {
            Id = auctionItem.Id.Value,
            Title = auctionItem.Title,
            CurrentPrice = auctionItem.CurrentPrice ?? auctionItem.StartingPrice,
            CreatedAt = auctionItem.CreatedAt
        };
    }
}

public sealed class GetAuctionItemByIdQueryHandler(IAuctionItemRepository repository) {
    public async ValueTask<GetAuctionItemByIdQueryResult> HandleAsync(GetAuctionItemByIdQuery query, CancellationToken cancellationToken) {
        AuctionItem? auctionItem = await repository.GetByIdAsync(AuctionItemId.From(query.Id));
        if(auctionItem is null) {
            throw new Exception("Auction not found");
        }
        return GetAuctionItemByIdQueryResult.From(auctionItem);
    }
}