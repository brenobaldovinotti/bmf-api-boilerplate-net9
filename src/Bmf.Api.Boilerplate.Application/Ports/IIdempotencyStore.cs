namespace Bmf.Api.Boilerplate.Application.Ports;

/// <summary>Manages idempotency keys and results.</summary>
public interface IIdempotencyStore
{
    /// <summary>Try to start a request scope; returns false if already processed.</summary>
    Task<bool> TryStartAsync(Guid requestId, CancellationToken ct);

    /// <summary>Mark the request as completed successfully.</summary>
    Task CompleteAsync(Guid requestId, CancellationToken ct);
}
