using Bmf.ApiBoilerplate.Domain.Time;
using FluentAssertions;
using Xunit;

namespace Bmf.ApiBoilerplate.Domain.Tests.Time;

/// <summary>
/// Lightweight tests to ensure SystemClock behaves as expected and demonstrate IClock usage.
/// </summary>
public class ClockTests
{
    [Fact]
    public void SystemClock_UtcNow_is_close_to_DateTime_UtcNow()
    {
        SystemClock clock = new();

        DateTime t1 = DateTime.UtcNow;
        DateTime actual = clock.UtcNow;
        DateTime t2 = DateTime.UtcNow;

        _ = actual.Should().BeOnOrAfter(t1).And.BeOnOrBefore(t2);
        _ = actual.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public async Task SystemClock_progresses_forward()
    {
        SystemClock clock = new();

        DateTime a = clock.UtcNow;
        await Task.Delay(5);
        DateTime b = clock.UtcNow;

        _ = b.Should().BeAfter(a);
    }

    [Fact]
    public void FakeClock_enables_deterministic_time_in_tests()
    {
        DateTime seed = new(2030, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        FakeClock fake = new(seed);

        _ = fake.UtcNow.Should().Be(seed);

        fake.Advance(TimeSpan.FromMinutes(1));
        _ = fake.UtcNow.Should().Be(seed.AddMinutes(1));
    }

    private sealed class FakeClock(DateTime utcNow) : IClock
    {
        public DateTime UtcNow { get; private set; } = utcNow.Kind == DateTimeKind.Utc
            ? utcNow : DateTime.SpecifyKind(utcNow, DateTimeKind.Utc);

        public void Advance(TimeSpan delta)
        {
            UtcNow = UtcNow.Add(delta);
        }

        public void Set(DateTime utcNow)
        {
            UtcNow = utcNow.Kind == DateTimeKind.Utc
            ? utcNow : DateTime.SpecifyKind(utcNow, DateTimeKind.Utc);
        }
    }
}
