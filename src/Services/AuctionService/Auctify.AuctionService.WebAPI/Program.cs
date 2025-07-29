using Auctify.AuctionService.WebAPI.Grains;
using Auctify.AuctionService.WebAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy("AllowBlazorApp", policy => {
        policy.WithOrigins("https://localhost:7063")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); 
    });
});

builder.Host.UseOrleans(siloBuilder => {
    siloBuilder.UseLocalhostClustering();
});

builder.Services.AddSignalR();

WebApplication app = builder.Build();

app.UseCors("AllowBlazorApp");

app.UseHttpsRedirection();

app.MapHub<AuctionHub>("/auctionhub");


RouteGroupBuilder auctionGroup = app.MapGroup("api/auctions");

auctionGroup.MapGet("{id}", async (string id, IGrainFactory grainFactory) => {
    IAuctionGrain auctionGrain = grainFactory.GetGrain<IAuctionGrain>(id);
    AuctionDetails details = await auctionGrain.GetAuctionDetails();
    return Results.Ok(details);
});

auctionGroup.MapPost("{id}/bids", async (
    string id,
    [FromBody] BidRequest bid,
    IGrainFactory _grainFactory,
    IHubContext<AuctionHub> _hubContext) => {
        IAuctionGrain auctionGrain = _grainFactory.GetGrain<IAuctionGrain>(id);

        bool bidAccepted = await auctionGrain.PlaceBid(bid.Amount);

        if (bidAccepted) {
            await _hubContext.Clients.All.SendAsync("NewBidReceived", id, bid.Amount);
            return Results.Ok(new { Message = "Teklif kabul edildi." });
        }

        return Results.BadRequest(new { Message = "Teklif, mevcut fiyattan yüksek olmalýdýr." });
    });

app.Run();

public sealed record BidRequest {
    public decimal Amount { get; set; }
}