using System.Reflection;
using Auctify.Libraries.Domain.Abstractions.DomainEvents;
using Auctify.Libraries.Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Wiaoj;

namespace Auctify.Libraries.Mediator.Implementations;
internal sealed class Mediator(IServiceProvider serviceProvider) : IMediator {
    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) {
        Preca.ThrowIfNull(request);
        Type requestType = request.GetType();

        Type handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        object handler = serviceProvider.GetRequiredService(handlerType);

        MethodInfo handleMethod = handler.GetType().GetMethod("HandleAsync")!;

        Type contextType = typeof(RequestContext<>).MakeGenericType(requestType);
        object context = Activator.CreateInstance(contextType, request, this, cancellationToken)!;

        return (Task<TResponse>)handleMethod.Invoke(handler, [context, cancellationToken])!;
    }

    public Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : IDomainEvent {
        Preca.ThrowIfNull(domainEvent);
        return PublishInternal((dynamic)domainEvent, cancellationToken);
    }
     
    private Task PublishInternal<TEvent>(TEvent domainEvent, CancellationToken cancellationToken) where TEvent : IDomainEvent {
        IEnumerable<IDomainEventHandler<TEvent>> handlers = serviceProvider.GetServices<IDomainEventHandler<TEvent>>();
         
        IEnumerable<Task> tasks = handlers.Select(handler => handler.HandleAsync(domainEvent, cancellationToken));

        return Task.WhenAll(tasks);
    }
}