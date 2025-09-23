using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Domain.Primitives;

namespace Bmf.Api.Boilerplate.Application.Behaviors;

/// <summary>
/// Wraps command handling in a unit-of-work transaction and enqueues any raised domain events to the outbox.
/// Queries pass straight through without a transaction.
/// </summary>
public sealed class TransactionOutboxBehavior<TRequest, TResponse>(IUnitOfWork uow, IOutbox outbox)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        // Only commands participate in transactional/outbox flow.
        if (request is not ICommand<TResponse>)
        {
            return await next().ConfigureAwait(false);
        }

        await uow.BeginAsync(ct).ConfigureAwait(false);

        try
        {
            TResponse response = await next().ConfigureAwait(false);

            await uow.SaveChangesAsync(ct).ConfigureAwait(false);

            IReadOnlyCollection<IDomainEvent> eventsToPublish = uow.CollectDomainEvents();
            if (eventsToPublish.Count > 0)
            {
                await outbox.EnqueueAsync(eventsToPublish, ct).ConfigureAwait(false);
                uow.ClearDomainEvents();
            }

            await uow.CommitAsync(ct).ConfigureAwait(false);
            return response;
        }
        catch
        {
            await uow.RollbackAsync(ct).ConfigureAwait(false);
            throw;
        }
    }
}
