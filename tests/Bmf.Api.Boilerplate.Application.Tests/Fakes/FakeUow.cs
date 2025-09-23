using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeUow : IUnitOfWork
{
    public int Begin, Commit, Rollback;
    public Task BeginAsync(CancellationToken ct) { Begin++; return Task.CompletedTask; }
    public Task CommitAsync(CancellationToken ct) { Commit++; return Task.CompletedTask; }
    public Task RollbackAsync(CancellationToken ct) { Rollback++; return Task.CompletedTask; }
}
