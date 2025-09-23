using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeCorrelationContext(string correlationId, string? causationId = null) : ICorrelationContext
{
    public string CorrelationId { get; } = correlationId;

    public string? CausationId { get; } = causationId;
}
