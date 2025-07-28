using Auctify.AuctionService.WebAPI.Grains;
using Auctify.AuctionService.WebAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args); 

builder.Host.UseOrleans(siloBuilder => {
    siloBuilder.UseLocalhostClustering();
});

builder.Services.AddSignalR();

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.MapHub<AuctionHub>("/auctionHub");
 

RouteGroupBuilder auctionGroup = app.MapGroup("api/auctions");

auctionGroup.MapPost("{id}/bid", async (
    string id,
    [FromBody] decimal amount,
    IGrainFactory _grainFactory,
    IHubContext<AuctionHub> _hubContext) => {
        IAuctionGrain auctionGrain = _grainFactory.GetGrain<IAuctionGrain>(id);

        bool bidAccepted = await auctionGrain.PlaceBid(amount);

        if (bidAccepted) {
            await _hubContext.Clients.All.SendAsync("NewBidReceived", id, amount);
            return Results.Ok(new { Message = "Teklif kabul edildi." });
        }

        return Results.BadRequest(new { Message = "Teklif, mevcut fiyattan yüksek olmalýdýr." });
    });

app.Run();