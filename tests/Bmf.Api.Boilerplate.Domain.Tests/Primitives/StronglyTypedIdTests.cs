using Bmf.Api.Boilerplate.Domain.Primitives;
using FluentAssertions;
using Xunit;

namespace Bmf.Api.Boilerplate.Domain.Tests.Primitives;

public sealed record TestId(Guid Value) : StronglyTypedId<TestId>(Value);

public class StronglyTypedIdTests
{
    [Fact]
    public void Equality_is_by_value_and_converts_to_guid()
    {
        Guid g = Guid.NewGuid();
        TestId a = new(g);
        TestId b = new(g);
        _ = a.Should().Be(b);
        Guid asGuid = a;
        _ = asGuid.Should().Be(g);
        _ = a.Value.ToString().Should().Be(g.ToString());
    }

    [Fact]
    public void TryParse_handles_valid_and_invalid_inputs()
    {
        bool ok = StronglyTypedId<TestId>.TryParse(Guid.NewGuid().ToString(), out Guid parsed);
        _ = ok.Should().BeTrue();
        _ = parsed.Should().NotBe(Guid.Empty);

        bool bad = StronglyTypedId<TestId>.TryParse("x", out Guid invalid);
        _ = bad.Should().BeFalse();
        _ = invalid.Should().Be(Guid.Empty);
    }
}
