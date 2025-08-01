using Auctify.Libraries.Domain.Abstractions.DomainEvents;

namespace Auctify.Libraries.Mediator.Abstractions;
public interface IMediator {
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : IDomainEvent;
}