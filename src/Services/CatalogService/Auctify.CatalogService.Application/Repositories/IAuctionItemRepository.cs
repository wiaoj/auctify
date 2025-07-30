using Auctify.CatalogService.Domain;

namespace Auctify.CatalogService.Application.Repositories;
public interface IAuctionItemRepository {
    Task<AuctionItem> GetByIdAsync(AuctionItemId id);
}
