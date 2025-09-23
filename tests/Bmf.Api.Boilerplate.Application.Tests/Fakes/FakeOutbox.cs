using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Domain.Primitives;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeOutbox : IOutbox
{
    public List<IDomainEvent> Stored { get; } = [];

    public Task EnqueueAsync(IEnumerable<IDomainEvent> eventsToStore, CancellationToken ct)
    {
        Stored.AddRange(eventsToStore);
        return Task.CompletedTask;
    }
}
