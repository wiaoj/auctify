using Auctify.CatalogService.Application.Repositories;
using Auctify.CatalogService.Domain;
using Microsoft.EntityFrameworkCore;

namespace Auctify.CatalogService.Infrastructure.Persistence.Repositories;
internal sealed class AuctionItemRepository(CatalogDbContext catalogDbContext) : IAuctionItemRepository {
    public async Task<AuctionItem?> GetByIdAsync(AuctionItemId id) {
        AuctionItem? auctionItem = await catalogDbContext.AuctionItems 
            .FirstOrDefaultAsync(ai => ai.Id == id, cancellationToken: default);

        if(auctionItem is null) {
            auctionItem = AuctionItem.New(id, "Generated Auction");
            catalogDbContext.Add(auctionItem);
            await catalogDbContext.SaveChangesAsync(default);
             
            return auctionItem;
        }

        return auctionItem;
    }

    public void Update(AuctionItem auctionItem) {
        catalogDbContext.Update(auctionItem);
    }
}
