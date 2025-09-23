using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Samples;

public sealed class SampleCommand(Guid requestId, string payload, string policy)
    : ICommand<string>, IAuthorizableRequest, IIdempotentRequest
{
    public Guid RequestId { get; } = requestId;

    public string Payload { get; } = payload;

    public string Policy { get; } = policy;
}
