using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Mediator;
using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Application.Tests.Fakes;
using Bmf.Api.Boilerplate.Application.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bmf.Api.Boilerplate.Application.Tests.Behaviors;

public sealed class LoggingTracingBehaviorTests
{
    [Fact]
    public async Task Logs_Before_And_After()
    {
        ServiceCollection sc = new();
        _ = sc.AddApplication(typeof(SampleCommandHandler).Assembly);

        _ = sc.AddSingleton<ICorrelationContext>(new FakeCorrelationContext("corr", "cause"));
        _ = sc.AddSingleton(typeof(ILogging<>), typeof(FakeLogging<>));
        _ = sc.AddSingleton<IUserContext>(new FakeUserContext("u1", true));
        _ = sc.AddSingleton<IAuthorizationService, FakeAuthorizationService>();
        _ = sc.AddSingleton<IIdempotencyStore, FakeIdempotencyStore>();
        _ = sc.AddSingleton<IOutbox, FakeOutbox>();
        _ = sc.AddSingleton<IUnitOfWork, FakeUnitOfWork>();

        ServiceProvider sp = sc.BuildServiceProvider();
        IMediator mediator = sp.GetRequiredService<IMediator>();

        SampleCommand cmd = new(Guid.NewGuid(), "p", "P");
        _ = await mediator.Send(cmd);

        FakeLogging<SampleCommand> logger = (FakeLogging<SampleCommand>)sp.GetRequiredService<ILogging<SampleCommand>>();
        _ = logger.InfoMessages.Should().HaveCount(2);
        _ = logger.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Logs_Error_On_Exception()
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

        // Replace handler that throws
        SampleCommandHandler throwing = new() { ShouldThrow = true };
        _ = sc.AddScoped<IRequestHandler<SampleCommand, string>>(_ => throwing);

        ServiceProvider sp = sc.BuildServiceProvider();
        IMediator mediator = sp.GetRequiredService<IMediator>();

        Func<Task> act = async () => _ = await mediator.Send(new SampleCommand(Guid.NewGuid(), "p", "P"));
        _ = await act.Should().ThrowAsync<InvalidOperationException>();

        FakeLogging<SampleCommand> logger = (FakeLogging<SampleCommand>)sp.GetRequiredService<ILogging<SampleCommand>>();
        _ = logger.Errors.Should().HaveCount(1);
    }
}
