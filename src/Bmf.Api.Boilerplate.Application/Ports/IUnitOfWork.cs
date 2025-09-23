using Bmf.Api.Boilerplate.Domain.Primitives;

namespace Bmf.Api.Boilerplate.Application.Ports;

/// <summary>Transaction and domain-events boundary.</summary>
public interface IUnitOfWork
{
    Task BeginAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task CommitAsync(CancellationToken ct);
    Task RollbackAsync(CancellationToken ct);

    /// <summary>Collect domain events accumulated during the unit of work.</summary>
    IReadOnlyCollection<IDomainEvent> CollectDomainEvents();

    /// <summary>Clear domain events after enqueuing.</summary>
    void ClearDomainEvents();
}
