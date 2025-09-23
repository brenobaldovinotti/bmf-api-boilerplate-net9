namespace Bmf.Api.Boilerplate.Application.Ports;

/// <summary>Correlation and tracing information.</summary>
public interface ICorrelationContext
{
    string CorrelationId { get; }
    string? CausationId { get; }
}
