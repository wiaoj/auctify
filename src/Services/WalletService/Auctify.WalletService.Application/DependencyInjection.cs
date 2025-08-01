using System.Reflection;
using Auctify.Libraries.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Wiaoj;

namespace Auctify.WalletService.Application;
public static class DependencyInjection {
    public static IServiceCollection AddWalletApplicationServices(this IServiceCollection services) {
        Preca.ThrowIfNull(services);
        services.AddAuctifyMediator(x => {
            x.RegisterHandlersFromAssemblies(Assembly.GetExecutingAssembly());
        });
        return services;
    }
}