using Auctify.Libraries.Domain.Abstractions.DomainEvents;
using Auctify.Libraries.Mediator.Abstractions;
using Wiaoj;

namespace Auctify.Libraries.Mediator.Implementations;
internal sealed class RequestContext<TRequest>(
    TRequest request,
    IMediator mediator,
    CancellationToken cancellationToken) : IRequestContext<TRequest> where TRequest : IRequest {
    public TRequest Request => request;
    public CancellationToken CancellationToken => cancellationToken;

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request) {
        Preca.ThrowIfNull(request);
        return mediator.SendAsync(request, this.CancellationToken);
    }

    public async Task PublishAsync(params IEnumerable<IDomainEvent> domainEvents) {
        Preca.ThrowIfNull(domainEvents);
        foreach(IDomainEvent domainEvent in domainEvents) {
            await mediator.PublishAsync(domainEvent, this.CancellationToken);
        }
    }
}