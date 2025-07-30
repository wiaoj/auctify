namespace Auctify.CatalogService.Application; 
public sealed record GetAuctionItemByIdQuery {

}

public sealed record GetAuctionItemByIdQueryResult {

}

public sealed class GetAuctionItemByIdQueryHandler {
    public ValueTask<GetAuctionItemByIdQueryResult> HandleAsync(GetAuctionItemByIdQuery query, CancellationToken cancellationToken) {

        return ValueTask.FromResult(new GetAuctionItemByIdQueryResult());
    }
} 