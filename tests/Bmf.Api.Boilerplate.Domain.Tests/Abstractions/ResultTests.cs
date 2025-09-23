using Bmf.Api.Boilerplate.Domain.Abstractions;
using FluentAssertions;
using Xunit;

namespace Bmf.Api.Boilerplate.Domain.Tests.Abstractions;

public class ResultTests
{
    [Fact]
    public void Success_without_value_is_success()
    {
        Result r = Result.Success();
        _ = r.IsSuccess.Should().BeTrue();
        _ = r.Error.Should().BeNull();
    }

    [Fact]
    public void Success_with_value_holds_value()
    {
        int data = 7;
        Result<int> r = Result.Success(data);
        _ = r.IsSuccess.Should().BeTrue();
        _ = r.Data.Should().Be(data);
        _ = r.Error.Should().BeNull();
    }

    [Fact]
    public void Failure_contains_error_and_throws_on_unwrap()
    {
        Error err = new("boom", "failure");
        Result<int> r = Result.Failure<int>(err);
        _ = r.IsSuccess.Should().BeFalse();
        _ = r.Error.Should().Be(err);
        _ = r.Invoking(x => x.UnwrapOrThrow()).Should().Throw<InvalidOperationException>();
    }
}
