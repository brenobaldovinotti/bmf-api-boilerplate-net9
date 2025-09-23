namespace Bmf.Api.Boilerplate.Domain.Time;

/// <summary>
/// Production clock implementation.
/// </summary>
public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
