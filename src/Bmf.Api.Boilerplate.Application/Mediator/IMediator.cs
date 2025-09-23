using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;

namespace Bmf.Api.Boilerplate.Application.Mediator;

/// <summary>Dispatches requests to their handlers through the pipeline.</summary>
public interface IMediator
{
    /// <summary>Send a request through the pipeline.</summary>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <param name="request">Request instance.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
}
