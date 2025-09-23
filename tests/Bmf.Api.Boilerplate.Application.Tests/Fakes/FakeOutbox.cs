using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeOutbox : IOutbox
{
    public int Flushes;
    public Task FlushAsync(CancellationToken ct) { Flushes++; return Task.CompletedTask; }
}
