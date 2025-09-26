using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Domain.Primitives;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeOutbox : IOutbox
{
    public List<IDomainEvent> Enqueued { get; } = [];

    public Task EnqueueAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct)
    {
        Enqueued.AddRange(domainEvents);
        return Task.CompletedTask;
    }
}
