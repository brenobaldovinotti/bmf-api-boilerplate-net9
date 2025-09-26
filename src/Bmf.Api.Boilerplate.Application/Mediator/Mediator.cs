using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bmf.Api.Boilerplate.Application.Mediator;

/// <summary>Simple in-house mediator that resolves handlers and executes the pipeline.</summary>
/// <remarks>Creates a new mediator.</remarks>
public sealed class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private static readonly MethodInfo _sendCoreMethod =
        typeof(Mediator).GetMethod(nameof(SendCore), BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("SendCore method not found.");

    private readonly IServiceProvider _sp = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    /// <inheritdoc />
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        if (request is not null)
        {
            // Use reflection only to jump into a strongly-typed generic method.
            MethodInfo closed = _sendCoreMethod.MakeGenericMethod(request.GetType(), typeof(TResponse));
            object? result = closed.Invoke(this, [request, ct]);

            // Invoke returns Task<TResponse>; no exceptions from the handler should be wrapped,
            // because they're thrown after we await inside SendCore (not during reflection).
            return (Task<TResponse>)result!;
        }

        throw new ArgumentNullException(nameof(request));
    }

    /// <summary>Strongly-typed execution path: builds the pipeline and calls the handler.</summary>
    private async Task<TResponse> SendCore<TRequest, TResponse>(TRequest request, CancellationToken ct)
        where TRequest : IRequest<TResponse>
    {
        // Resolve the handler and behaviors with the correct closed generic types.
        IRequestHandler<TRequest, TResponse> handler =
            _sp.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        IEnumerable<IPipelineBehavior<TRequest, TResponse>> behaviors =
            _sp.GetServices<IPipelineBehavior<TRequest, TResponse>>();

        // Terminal delegate invokes the handler directly (no reflection here).
        Task<TResponse> terminal()
        {
            return handler.Handle(request, ct);
        }

        // Build outer → inner chain based on registration order.
        RequestHandlerDelegate<TResponse> pipeline = behaviors
            .Reverse()
            .Aggregate((RequestHandlerDelegate<TResponse>)terminal, (next, behavior) => () => behavior.Handle(request, next, ct));

        return await pipeline().ConfigureAwait(false);
    }
}
