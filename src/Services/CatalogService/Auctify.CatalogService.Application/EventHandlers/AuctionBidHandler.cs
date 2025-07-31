using Auctify.CatalogService.Application.Interfaces;
using Auctify.CatalogService.Application.Repositories;
using Auctify.CatalogService.Domain;
using Auctify.Libraries.Shared.Contracts;
using Wiaoj.Corvus.Abstractions;

namespace Auctify.CatalogService.Application.EventHandlers;
public sealed class AuctionBidHandler(IAuctionItemRepository repository, IUnitOfWork unitOfWork) : IHandler<NewBidPlacedEvent> {
    public async ValueTask HandleAsync(IMessageContext<NewBidPlacedEvent> messageContext) {
        NewBidPlacedEvent message = messageContext.Message;
        AuctionItem? auctionItem = await repository.GetByIdAsync(AuctionItemId.From(message.AuctionId));

        if(auctionItem is null) {
            // Log the error or handle it as needed
            return;
        }
         
        auctionItem.SetCurrentPrice(message.NewPrice);
        repository.Update(auctionItem);
        await unitOfWork.SaveChangesAsync(messageContext.CancellationToken);
    }
}