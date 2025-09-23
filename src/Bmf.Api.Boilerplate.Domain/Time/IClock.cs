namespace Bmf.Api.Boilerplate.Domain.Time;

/// <summary>
/// Abstract time source for testability.
/// </summary>
public interface IClock
{
    DateTime UtcNow { get; }
}
