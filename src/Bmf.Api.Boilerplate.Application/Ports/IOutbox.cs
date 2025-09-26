using Bmf.Api.Boilerplate.Domain.Primitives;

namespace Bmf.Api.Boilerplate.Application.Ports;

/// <summary>Persists domain events to an outbox for delivery.</summary>
public interface IOutbox
{
    Task EnqueueAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct);
}
