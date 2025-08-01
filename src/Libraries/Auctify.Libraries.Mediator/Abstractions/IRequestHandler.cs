using Auctify.Libraries.Domain.Abstractions.DomainEvents;

namespace Auctify.Libraries.Mediator.Abstractions;
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse> {
    Task<TResponse> HandleAsync(IRequestContext<TRequest> context, CancellationToken cancellationToken);
}

public interface IRequestContext<out TRequest> where TRequest : IRequest {
    TRequest Request { get; } 
    CancellationToken CancellationToken { get; }    
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request); 
    Task PublishAsync(params IEnumerable<IDomainEvent> domainEvents);
}