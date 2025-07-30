using Auctify.CatalogService.Application;
using Auctify.CatalogService.Application.Repositories;
using Auctify.CatalogService.Infrastructure.Persistence.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<GetAuctionItemByIdQueryHandler>(); 
builder.Services.AddScoped<IAuctionItemRepository, AuctionItemRepository>();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var catalogApi= app.MapGroup("api/catalog");

catalogApi.MapGet("{id:guid}", async (Guid id, GetAuctionItemByIdQueryHandler handler, CancellationToken cancellationToken) => {
    GetAuctionItemByIdQuery query = new(id);
    GetAuctionItemByIdQueryResult result = await handler.HandleAsync(query, cancellationToken);
    return Results.Ok(result);
});

await app.RunAsync(default(CancellationToken));