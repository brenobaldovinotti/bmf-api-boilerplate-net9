using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeIdempotencyStore : IIdempotencyStore
{
    private readonly HashSet<Guid> _seen = [];

    public List<string> Calls { get; } = [];

    public Task<bool> TryStartAsync(Guid requestId, CancellationToken ct)
    {
        Calls.Add($"start:{requestId}");
        return Task.FromResult(_seen.Add(requestId));
    }

    public Task CompleteAsync(Guid requestId, CancellationToken ct)
    {
        Calls.Add($"complete:{requestId}");
        return Task.CompletedTask;
    }
}
