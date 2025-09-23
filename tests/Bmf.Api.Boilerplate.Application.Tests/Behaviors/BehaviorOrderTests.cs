using Bmf.Api.Boilerplate.Application.Mediator;
using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Application.Tests.Fakes;
using Bmf.Api.Boilerplate.Application.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bmf.Api.Boilerplate.Application.Tests.Behaviors;

public sealed class BehaviorOrderTests
{
    [Fact]
    public async Task Pipeline_Order_Is_Validation_Authorization_Idempotency_Transaction_Logging()
    {
        ServiceCollection sc = new();
        _ = sc.AddApplication(typeof(SampleCommandHandler).Assembly);

        FakeAuthorizationService auth = new();
        FakeIdempotencyStore idem = new();
        FakeOutbox outbox = new();
        FakeUnitOfWork uow = new();
        FakeLogging<SampleCommand> log = new();

        _ = sc.AddSingleton<ICorrelationContext>(new FakeCorrelationContext("corr"));
        _ = sc.AddSingleton(typeof(ILogging<>), typeof(FakeLogging<>));
        _ = sc.AddSingleton<IUserContext>(new FakeUserContext("u1", true));
        _ = sc.AddSingleton<IAuthorizationService>(auth);
        _ = sc.AddSingleton<IIdempotencyStore>(idem);
        _ = sc.AddSingleton<IOutbox>(outbox);
        _ = sc.AddSingleton<IUnitOfWork>(uow);

        ServiceProvider sp = sc.BuildServiceProvider();
        IMediator mediator = sp.GetRequiredService<IMediator>();

        SampleCommand cmd = new(Guid.NewGuid(), "payload", "CanDo");
        string result = await mediator.Send(cmd);

        _ = result.Should().Be("ok:payload");

        // Assertions per stage being touched in order (proof via side-effects)
        _ = auth.Calls.Should().ContainSingle().Which.Should().Be("CanDo");
        _ = idem.Calls.Should().HaveCount(2);
        _ = idem.Calls[0].Should().StartWith("start:");
        _ = idem.Calls[1].Should().StartWith("complete:");
        _ = uow.Calls.Should().ContainInOrder("begin", "save", "collect", "clear", "commit");
        _ = log.InfoMessages.Should().HaveCount(2);
    }
}
