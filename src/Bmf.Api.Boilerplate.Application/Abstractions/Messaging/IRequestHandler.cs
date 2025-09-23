namespace Bmf.Api.Boilerplate.Application.Abstractions.Messaging;

/// <summary>Handles a request and returns a response.</summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>Handle the incoming request.</summary>
    /// <param name="request">Request instance.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<TResponse> Handle(TRequest request, CancellationToken ct);
}
