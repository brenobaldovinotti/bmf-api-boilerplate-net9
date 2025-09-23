using Bmf.Api.Boilerplate.Application.Mediator;
using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Application.Tests.Fakes;
using Bmf.Api.Boilerplate.Application.Tests.Samples;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bmf.Api.Boilerplate.Application.Tests.Behaviors;

public sealed class ValidationBehaviorTests
{
    [Fact]
    public async Task Throws_When_Invalid()
    {
        ServiceCollection sc = new();
        _ = sc.AddApplication(typeof(SampleCommandHandler).Assembly);

        _ = sc.AddSingleton<ICorrelationContext>(new FakeCorrelationContext("corr"));
        _ = sc.AddSingleton(typeof(ILogging<>), typeof(FakeLogging<>));
        _ = sc.AddSingleton<IUserContext>(new FakeUserContext("u1", true));
        _ = sc.AddSingleton<IAuthorizationService, FakeAuthorizationService>();
        _ = sc.AddSingleton<IIdempotencyStore, FakeIdempotencyStore>();
        _ = sc.AddSingleton<IOutbox, FakeOutbox>();
        _ = sc.AddSingleton<IUnitOfWork, FakeUnitOfWork>();

        ServiceProvider sp = sc.BuildServiceProvider();
        IMediator mediator = sp.GetRequiredService<IMediator>();

        SampleCommand invalid = new(Guid.NewGuid(), string.Empty, "P");
        Func<Task> act = async () => _ = await mediator.Send(invalid);

        _ = await act.Should().ThrowAsync<ValidationException>();
    }
}
