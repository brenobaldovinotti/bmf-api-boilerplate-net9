using Bmf.Api.Boilerplate.Application.Mediator;
using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Application.Tests.Fakes;
using Bmf.Api.Boilerplate.Application.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bmf.Api.Boilerplate.Application.Tests.Mediator;

public sealed class MediatorTests
{
    [Fact]
    public async Task Send_Invokes_Handler()
    {
        ServiceCollection sc = new();
        _ = sc.AddApplication(typeof(SampleCommandHandler).Assembly);

        // Required behavior dependencies
        _ = sc.AddSingleton<ICorrelationContext>(new FakeCorrelationContext("corr"));
        _ = sc.AddSingleton(typeof(ILogging<>), typeof(FakeLogging<>));
        _ = sc.AddSingleton<IUserContext>(new FakeUserContext("u1", true));
        _ = sc.AddSingleton<IAuthorizationService, FakeAuthorizationService>();
        _ = sc.AddSingleton<IIdempotencyStore, FakeIdempotencyStore>();
        _ = sc.AddSingleton<IOutbox, FakeOutbox>();
        _ = sc.AddSingleton<IUnitOfWork, FakeUnitOfWork>();

        ServiceProvider sp = sc.BuildServiceProvider();
        IMediator mediator = sp.GetRequiredService<IMediator>();

        SampleQuery q = new(41);
        int result = await mediator.Send(q);

        _ = result.Should().Be(42);
    }
}
