using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Requests;

/// <summary>Simple test request used to verify pipeline order.</summary>
public sealed record Echo(string Message) : IQuery<string>, IIdempotentRequest
{
    public Guid RequestId { get; init; } = Guid.NewGuid();
}

