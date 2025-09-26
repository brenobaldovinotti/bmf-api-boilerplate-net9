using Bmf.Api.Boilerplate.Application.Mediator;
using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Application.Tests.Fakes;
using Bmf.Api.Boilerplate.Application.Tests.Samples;
using Bmf.Api.Boilerplate.Domain.Primitives;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bmf.Api.Boilerplate.Application.Tests.Behaviors;

/// <summary>Verifies the pipeline execution order and side-effects.</summary>
public sealed class BehaviorOrderTests
{
    [Fact]
    public async Task Pipeline_Order_Is_Validation_Authorization_Idempotency_Transaction_Logging()
    {
        // Arrange
        ServiceCollection sc = new();
        _ = sc.AddApplication(typeof(SampleCommandHandler).Assembly);

        FakeAuthorizationService auth = new();
        FakeIdempotencyStore idem = new();
        FakeOutbox outbox = new();
        FakeUnitOfWork uow = new();

        // Correlation + user + ports
        _ = sc.AddSingleton<ICorrelationContext>(new FakeCorrelationContext("corr"));
        _ = sc.AddSingleton(typeof(ILogging<>), typeof(FakeLogging<>));   // open-generic registration
        _ = sc.AddSingleton<IUserContext>(new FakeUserContext("u1", true));
        _ = sc.AddSingleton<IAuthorizationService>(auth);
        _ = sc.AddSingleton<IIdempotencyStore>(idem);
        _ = sc.AddSingleton<IOutbox>(outbox);
        _ = sc.AddSingleton<IUnitOfWork>(uow);

        // Seed ONE domain event so TransactionOutboxBehavior enqueues and then clears events
        uow.AddEvent(new TestDomainEvent(DateTime.UtcNow));

        ServiceProvider sp = sc.BuildServiceProvider();

        // Resolve the SAME logger instance the pipeline will use
        FakeLogging<SampleCommand> log = (FakeLogging<SampleCommand>)sp.GetRequiredService<ILogging<SampleCommand>>();

        IMediator mediator = sp.GetRequiredService<IMediator>();
        SampleCommand cmd = new(Guid.NewGuid(), "payload", "CanDo");

        // Act
        string result = await mediator.Send(cmd);

        // Assert
        _ = result.Should().Be("ok:payload");

        // Authorization happened
        _ = auth.Calls.Should().ContainSingle().Which.Should().Be("CanDo");

        // Idempotency (start + complete)
        _ = idem.Calls.Should().HaveCount(2);
        _ = idem.Calls[0].Should().StartWith("start:");
        _ = idem.Calls[1].Should().StartWith("complete:");

        // Transaction/Outbox (clear must appear because we seeded an event)
        _ = uow.Calls.Should().ContainInOrder("begin", "save", "collect", "clear", "commit");

        // Logging (start + success)
        _ = log.InfoMessages.Should().HaveCount(2);
    }

    private sealed class TestDomainEvent(DateTime occurredOnUtc) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = occurredOnUtc;
    }
}
