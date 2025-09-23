using Bmf.Api.Boilerplate.Application.Ports;
using System.Collections.Concurrent;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeIdempotencyStore : IIdempotencyStore
{
    private readonly ConcurrentDictionary<Guid, byte> _seen;

    public FakeIdempotencyStore()
    {
        _seen = new ConcurrentDictionary<Guid, byte>();
    }

    public Task<bool> TryStartAsync(Guid requestId, CancellationToken ct)
    {
        bool added = _seen.TryAdd(requestId, 1);
        return Task.FromResult(added);
    }

    public Task CompleteAsync(Guid requestId, CancellationToken ct)
    {
        _ = _seen.TryGetValue(requestId, out _);
        return Task.CompletedTask;
    }
}
