using Auctify.CatalogService.Application.Repositories;
using Auctify.CatalogService.Domain;

namespace Auctify.CatalogService.Application; 
public sealed record GetAuctionItemByIdQuery(Guid Id);

public sealed record GetAuctionItemByIdQueryResult {
    public Guid Id { get; internal set; }
    public   string Name { get; internal set; }
    public DateTimeOffset CreatedAt { get; internal set; }
}

public sealed class GetAuctionItemByIdQueryHandler {
    private readonly IAuctionItemRepository _repository;

    public GetAuctionItemByIdQueryHandler(IAuctionItemRepository repository) {
        _repository = repository;
    }

    public async ValueTask<GetAuctionItemByIdQueryResult> HandleAsync(GetAuctionItemByIdQuery query, CancellationToken cancellationToken) {
        AuctionItem auctionItem = await _repository.GetByIdAsync(AuctionItemId.From(query.Id));
         
        return new GetAuctionItemByIdQueryResult {
            Id = auctionItem.Id.Value,
            Name = auctionItem.Name,
            CreatedAt = auctionItem.CreatedAt
        };
    }
}