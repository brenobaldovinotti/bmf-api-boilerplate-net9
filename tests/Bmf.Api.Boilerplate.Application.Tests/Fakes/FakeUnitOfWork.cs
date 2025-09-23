using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Domain.Primitives;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeUnitOfWork : IUnitOfWork
{
    public List<string> Calls { get; } = [];
    private readonly List<IDomainEvent> _events = [];

    public Task BeginAsync(CancellationToken ct)
    {
        Calls.Add("begin");
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        Calls.Add("save");
        return Task.CompletedTask;
    }

    public Task CommitAsync(CancellationToken ct)
    {
        Calls.Add("commit");
        return Task.CompletedTask;
    }

    public Task RollbackAsync(CancellationToken ct)
    {
        Calls.Add("rollback");
        return Task.CompletedTask;
    }

    public IReadOnlyCollection<IDomainEvent> CollectDomainEvents()
    {
        Calls.Add("collect");
        return _events.ToList();
    }

    public void ClearDomainEvents()
    {
        Calls.Add("clear");
        _events.Clear();
    }

    public void AddEvent(IDomainEvent e)
    {
        _events.Add(e);
    }
}
