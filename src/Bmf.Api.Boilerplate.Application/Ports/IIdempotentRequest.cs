namespace Bmf.Api.Boilerplate.Application.Ports;

/// <summary>Marker for idempotent requests.</summary>
public interface IIdempotentRequest
{
    Guid RequestId { get; }
}

