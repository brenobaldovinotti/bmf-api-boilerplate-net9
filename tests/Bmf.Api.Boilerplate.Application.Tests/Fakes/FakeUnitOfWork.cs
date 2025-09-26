using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Domain.Primitives;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

/// <summary>Fake UoW that records calls and holds in-memory domain events.</summary>
public sealed class FakeUnitOfWork : IUnitOfWork
{
    private readonly List<string> _calls = [];

    public IReadOnlyList<string> Calls => _calls;
    public List<IDomainEvent> DomainEvents { get; } = [];

    public Task BeginAsync(CancellationToken ct)
    {
        _calls.Add("begin");
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        _calls.Add("save");
        return Task.CompletedTask;
    }

    public Task CommitAsync(CancellationToken ct)
    {
        _calls.Add("commit");
        return Task.CompletedTask;
    }

    public Task RollbackAsync(CancellationToken ct)
    {
        _calls.Add("rollback");
        return Task.CompletedTask;
    }

    public IReadOnlyCollection<IDomainEvent> CollectDomainEvents()
    {
        _calls.Add("collect");
        return DomainEvents.AsReadOnly();
    }

    public void AddEvent(IDomainEvent e)
    {
        DomainEvents.Add(e);
    }

    public void ClearDomainEvents()
    {
        _calls.Add("clear");
        DomainEvents.Clear();
    }
}
