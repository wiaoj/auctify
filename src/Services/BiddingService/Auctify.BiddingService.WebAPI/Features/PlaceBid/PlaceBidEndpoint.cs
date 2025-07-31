using Auctify.BiddingService.WebAPI.Grains;
using Auctify.BiddingService.WebAPI.Hubs;
using Auctify.Libraries.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Wiaoj.Corvus.Abstractions;

namespace Auctify.BiddingService.WebAPI.Features.PlaceBid;
public static class PlaceBidEndpoint {
    public static IEndpointRouteBuilder MapPlaceBidEndpoint(this IEndpointRouteBuilder app) {
        app.MapPost("/api/bids/{auctionId}",
            async (
                [FromRoute] string auctionId,
                [FromBody] PlaceBidRequest request,
                [FromServices] IGrainFactory grainFactory,
                [FromServices] IHubContext<AuctionHub> hubContext,
                [FromServices] IBus corvusBus) => {
                    IBiddingGrain biddingGrain = grainFactory.GetGrain<IBiddingGrain>(auctionId);
                    bool bidAccepted = await biddingGrain.PlaceBid(request.Amount);

                    if(bidAccepted) {
                        await hubContext.Clients.All.SendAsync("NewBidReceived", auctionId, request.Amount);

                        NewBidPlacedEvent newBidEvent = new(Guid.Parse(auctionId), request.Amount);
                        await corvusBus.PublishAsync(newBidEvent);

                        PlaceBidResponse response = new(auctionId, request.Amount, "Teklif kabul edildi.");
                        return Results.Ok(response);
                    }

                    return Results.BadRequest("Teklif, mevcut fiyattan yüksek olmalıdır.");
                })
            .WithName("PlaceBid")
            .WithTags("Bidding");

        return app;
    }
}