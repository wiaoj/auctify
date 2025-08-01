using System.Reflection;
using Auctify.Libraries.Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Wiaoj;

namespace Auctify.Libraries.Mediator;
public static class DependencyInjection {
    public static IServiceCollection AddAuctifyMediator(this IServiceCollection services, Action<MediatorOptions> configure) {
        Preca.ThrowIfNull(services);
        Preca.ThrowIfNull(configure);

        MediatorOptions options = new();
        configure(options);

        services.TryAddSingleton<IMediator, Implementations.Mediator>();

        RegisterHandlers(services, options);

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, MediatorOptions options) {
        IEnumerable<Type> allTypes = options.AssembliesToScan
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract);

        foreach(Type? type in allTypes) {
            Type[] interfaces = type.GetInterfaces();

            foreach(Type @interface in interfaces) {
                if(!@interface.IsGenericType)
                    continue;

                if(@interface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)) {
                    services.TryAdd(new ServiceDescriptor(@interface, type, options.HandlersLifetime));
                }
                else if(@interface.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)) {
                    services.TryAddEnumerable(new ServiceDescriptor(@interface, type, options.HandlersLifetime));
                }
            }
        }
    }
}

public sealed class MediatorOptions {
    internal List<Assembly> AssembliesToScan { get; } = [];
    public ServiceLifetime HandlersLifetime { get; set; } = ServiceLifetime.Scoped;

    /// <summary>
    /// Belirtilen assembly içindeki tüm IRequestHandler ve IDomainEventHandler implementasyonlarını kaydeder.
    /// </summary>
    /// <param name="assembly">Taranacak assembly.</param>
    public void RegisterHandlersFromAssemblies(params IEnumerable<Assembly> assemblies) {
        Preca.ThrowIfNull(assemblies);
        foreach(Assembly assembly in assemblies) {
            if(assembly == null || this.AssembliesToScan.Contains(assembly))
                continue;

            this.AssembliesToScan.Add(assembly);
        }
    }
}