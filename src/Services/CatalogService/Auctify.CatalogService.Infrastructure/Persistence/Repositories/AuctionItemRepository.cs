using Auctify.CatalogService.Application.Repositories;
using Auctify.CatalogService.Domain;

namespace Auctify.CatalogService.Infrastructure.Persistence.Repositories;
internal sealed class AuctionItemRepository : IAuctionItemRepository {
    public Task<AuctionItem> GetByIdAsync(AuctionItemId id) { 
        return Task.FromResult(AuctionItem.New("Test Müzayedesi"));
    }
}
