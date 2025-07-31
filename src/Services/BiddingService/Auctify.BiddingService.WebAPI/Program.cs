using Auctify.BiddingService.WebAPI.Features.PlaceBid;
using Auctify.BiddingService.WebAPI.Hubs;
using Auctify.Libraries.Shared.Contracts;
using Wiaoj.Corvus.Abstractions;
using Wiaoj.Corvus.Extensions.DependencyInjection;
using Wiaoj.Serialization.Extensions.DependencyInjection;

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

    siloBuilder.AddAdoNetGrainStorage("AuctifyStorage", options => {
        options.Invariant = "Npgsql";
        options.ConnectionString = builder.Configuration.GetConnectionString("PostgreSQL");
    });
});

builder.Services.AddWiaojSerializer(s => s.UseSystemTextJson<CorvusDefaultSerializerKey>());
builder.Services.AddCorvus(configurator => {
    configurator.Serialization(x => x.Add<CorvusDefaultSerializerKey>("application/json"));
    configurator.UseRabbitMqTransport();
    configurator.ForEvent<NewBidPlacedEvent>(eventConfig => {
        eventConfig.PublishTo(x => x.UseRabbitMq().RouteToQueue("new-bid-placed-event"));
    });
});


builder.Services.AddSignalR();

WebApplication app = builder.Build();

app.UseCors("AllowBlazorApp");
app.UseHttpsRedirection();

app.MapHub<AuctionHub>("/auctionHub");

app.MapPlaceBidEndpoint();

await app.RunAsync(default(CancellationToken));