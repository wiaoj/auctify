using Auctify.Libraries.Domain.Abstractions.DomainEvents;

namespace Auctify.Libraries.Mediator.Abstractions;
public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent {
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken);
}