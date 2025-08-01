namespace Auctify.Libraries.Mediator.Abstractions;
/// <summary>
/// Represents a request that returns a response.
/// </summary>
public interface IRequest<out TResponse> : IRequest;

/// <summary>
/// Represents a request that does not return a value.
/// </summary>
public interface IRequest; 