using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeCorrelationContext : ICorrelationContext
{
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString("n");
    public string? CausationId { get; init; }
}
