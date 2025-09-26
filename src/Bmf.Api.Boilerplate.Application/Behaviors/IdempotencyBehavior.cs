using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Behaviors;

/// <summary>Guarantees exactly-once semantics for idempotent requests.</summary>
public sealed class IdempotencyBehavior<TRequest, TResponse>(IIdempotencyStore store)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (request is IIdempotentRequest idempotent)
        {
            bool started = await store.TryStartAsync(idempotent.RequestId, ct).ConfigureAwait(false);
            if (!started)
            {
                throw new InvalidOperationException("Duplicate idempotent request.");
            }

            TResponse response = await next().ConfigureAwait(false);
            await store.CompleteAsync(idempotent.RequestId, ct).ConfigureAwait(false);
            return response;
        }

        return await next().ConfigureAwait(false);
    }
}
