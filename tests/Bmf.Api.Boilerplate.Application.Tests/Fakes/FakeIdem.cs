using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeIdem : IIdempotencyStore
{
    private readonly HashSet<Guid> _seen = [];
    public bool PretendDuplicate { get; set; }

    public Task<bool> IsDuplicateAsync(Guid requestId, CancellationToken ct)
    {
        return Task.FromResult(PretendDuplicate || _seen.Contains(requestId));
    }

    public Task RegisterAsync(Guid requestId, CancellationToken ct)
    {
        _ = _seen.Add(requestId);
        return Task.CompletedTask;
    }
}
