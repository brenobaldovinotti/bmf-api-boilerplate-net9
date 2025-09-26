using Bmf.Api.Boilerplate.Application.Mediator;
using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Application.Tests.Fakes;
using Bmf.Api.Boilerplate.Application.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bmf.Api.Boilerplate.Application.Tests.Behaviors;

public sealed class AuthorizationBehaviorTests
{
    [Fact]
    public async Task Throws_When_Not_Authenticated()
    {
        ServiceCollection sc = new();
        _ = sc.AddApplication(typeof(SampleCommandHandler).Assembly);

        _ = sc.AddSingleton<ICorrelationContext>(new FakeCorrelationContext("corr"));
        _ = sc.AddSingleton(typeof(ILogging<>), typeof(FakeLogging<>));
        _ = sc.AddSingleton<IUserContext>(new FakeUserContext(null, false));
        _ = sc.AddSingleton<IAuthorizationService, FakeAuthorizationService>();
        _ = sc.AddSingleton<IIdempotencyStore, FakeIdempotencyStore>();
        _ = sc.AddSingleton<IOutbox, FakeOutbox>();
        _ = sc.AddSingleton<IUnitOfWork, FakeUnitOfWork>();

        ServiceProvider sp = sc.BuildServiceProvider();
        IMediator mediator = sp.GetRequiredService<IMediator>();

        SampleCommand cmd = new(Guid.NewGuid(), "x", "Any");
        Func<Task> act = async () => _ = await mediator.Send(cmd);

        _ = await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Throws_When_Policy_Denied()
    {
        ServiceCollection sc = new();
        _ = sc.AddApplication(typeof(SampleCommandHandler).Assembly);

        FakeAuthorizationService auth = new() { ShouldThrowForbidden = true };

        _ = sc.AddSingleton<ICorrelationContext>(new FakeCorrelationContext("corr"));
        _ = sc.AddSingleton(typeof(ILogging<>), typeof(FakeLogging<>));
        _ = sc.AddSingleton<IUserContext>(new FakeUserContext("u1", true));
        _ = sc.AddSingleton<IAuthorizationService>(auth);
        _ = sc.AddSingleton<IIdempotencyStore, FakeIdempotencyStore>();
        _ = sc.AddSingleton<IOutbox, FakeOutbox>();
        _ = sc.AddSingleton<IUnitOfWork, FakeUnitOfWork>();

        ServiceProvider sp = sc.BuildServiceProvider();
        IMediator mediator = sp.GetRequiredService<IMediator>();

        SampleCommand cmd = new(Guid.NewGuid(), "x", "Denied");
        Func<Task> act = async () => _ = await mediator.Send(cmd);

        _ = await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
