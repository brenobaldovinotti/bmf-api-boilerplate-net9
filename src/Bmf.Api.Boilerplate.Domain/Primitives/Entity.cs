namespace Bmf.Api.Boilerplate.Domain.Primitives;

/// <summary>
/// Base type for entities.
/// </summary>
public abstract class Entity<TId>(TId id)
{
    public TId Id { get; protected init; } = id;

    public override string ToString()
    {
        return $"{GetType().Name} [{Id}]";
    }
}
