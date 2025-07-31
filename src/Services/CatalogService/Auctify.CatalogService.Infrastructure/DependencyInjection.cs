using Auctify.CatalogService.Application.Interfaces;
using Auctify.CatalogService.Application.Repositories;
using Auctify.CatalogService.Infrastructure.Persistence;
using Auctify.CatalogService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auctify.CatalogService.Infrastructure;
public static class DependencyInjection {
    public static IServiceCollection AddCatalogServiceInfrastructure(this IServiceCollection services) {
        services.AddDbContext<CatalogDbContext>(options => {
            options.UseInMemoryDatabase("CatalogDb");
        });
        services.AddScoped<IAuctionItemRepository, AuctionItemRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<CatalogDbContext>());
        return services;
    }
}