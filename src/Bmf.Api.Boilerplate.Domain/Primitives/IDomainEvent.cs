namespace Bmf.Api.Boilerplate.Domain.Primitives;

/// <summary>
/// Marker for domain events emitted by aggregates.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
