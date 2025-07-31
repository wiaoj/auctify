using Auctify.CatalogService.Application;
using Auctify.CatalogService.Application.EventHandlers;
using Auctify.CatalogService.Infrastructure;
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

builder.Services.AddScoped<GetAuctionItemByIdQueryHandler>();
builder.Services.AddCatalogServiceInfrastructure();

builder.Services.AddWiaojSerializer(x => x.UseSystemTextJson<CorvusDefaultSerializerKey>());
builder.Services.AddCorvus(configurator => {
    configurator.AddHandlersFromAssemblies(typeof(AuctionBidHandler).Assembly);
    configurator.Serialization(x => x.Add<CorvusDefaultSerializerKey>("application/json"));
    configurator.UseRabbitMqTransport();
    configurator.ForEvent<NewBidPlacedEvent>(eventConfig => {
        eventConfig.PublishTo(x => x.UseRabbitMq().ListenOnQueue("new-bid-placed-event"));
    });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors("AllowBlazorApp");
app.UseHttpsRedirection();

RouteGroupBuilder catalogApi = app.MapGroup("api/catalog");

catalogApi.MapGet("{id:guid}", async (Guid id, GetAuctionItemByIdQueryHandler handler, CancellationToken cancellationToken) => {
    GetAuctionItemByIdQuery query = new(id);
    GetAuctionItemByIdQueryResult result = await handler.HandleAsync(query, cancellationToken);
    return Results.Ok(result);
});

await app.RunAsync(default(CancellationToken));