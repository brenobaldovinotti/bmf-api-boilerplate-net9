using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Domain.Primitives;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeUnitOfWork : IUnitOfWork
{
    public Task BeginAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public void ClearDomainEvents()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<IDomainEvent> CollectDomainEvents()
    {
        throw new NotImplementedException();
    }

    public Task CommitAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task RollbackAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
