namespace Bmf.ApiBoilerplate.Domain.Primitives;

/// <summary>Aggregate root maintains pending domain events.</summary>
public abstract class AggregateRoot<TId>(TId id) : Entity<TId>(id)
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    /// <summary>Record a new event to be dispatched transactionally.</summary>
    protected void Raise(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    /// <summary>Clear all pending events (after persistence).</summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
