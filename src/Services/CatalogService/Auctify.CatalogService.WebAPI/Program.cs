using Auctify.CatalogService.Application;
using Auctify.CatalogService.Application.Repositories;
using Auctify.CatalogService.Infrastructure.Persistence.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<GetAuctionItemByIdQueryHandler>(); 
builder.Services.AddScoped<IAuctionItemRepository, AuctionItemRepository>();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();



await app.RunAsync(default(CancellationToken));