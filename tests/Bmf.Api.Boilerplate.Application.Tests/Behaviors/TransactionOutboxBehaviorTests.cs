using Bmf.Api.Boilerplate.Application.Mediator;
using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Application.Tests.Fakes;
using Bmf.Api.Boilerplate.Application.Tests.Samples;
using Bmf.Api.Boilerplate.Domain.Primitives;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bmf.Api.Boilerplate.Application.Tests.Behaviors;

public sealed class TransactionOutboxBehaviorTests
{
    private sealed record TestEvent(string Name) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    [Fact]
    public async Task Commands_Are_Transactional_And_Events_Are_Enqueued()
    {
        ServiceCollection sc = new();
        _ = sc.AddApplication(typeof(SampleCommandHandler).Assembly);

        FakeUnitOfWork uow = new();
        uow.AddEvent(new TestEvent("e1"));
        FakeOutbox outbox = new();

        _ = sc.AddSingleton<ICorrelationContext>(new FakeCorrelationContext("corr"));
        _ = sc.AddSingleton(typeof(ILogging<>), typeof(FakeLogging<>));
        _ = sc.AddSingleton<IUserContext>(new FakeUserContext("u1", true));
        _ = sc.AddSingleton<IAuthorizationService, FakeAuthorizationService>();
        _ = sc.AddSingleton<IIdempotencyStore, FakeIdempotencyStore>();
        _ = sc.AddSingleton<IOutbox>(outbox);
        _ = sc.AddSingleton<IUnitOfWork>(uow);

        ServiceProvider sp = sc.BuildServiceProvider();
        IMediator mediator = sp.GetRequiredService<IMediator>();

        SampleCommand cmd = new(Guid.NewGuid(), "p", "P");
        string r = await mediator.Send(cmd);

        _ = r.Should().Be("ok:p");
        _ = uow.Calls.Should().ContainInOrder("begin", "save", "collect", "clear", "commit");
        _ = outbox.Enqueued.Should().HaveCount(1);
    }

    [Fact]
    public async Task Queries_Bypass_Transaction()
    {
        ServiceCollection sc = new();
        _ = sc.AddApplication(typeof(SampleQueryHandler).Assembly);

        FakeUnitOfWork uow = new();

        _ = sc.AddSingleton<ICorrelationContext>(new FakeCorrelationContext("corr"));
        _ = sc.AddSingleton(typeof(ILogging<>), typeof(FakeLogging<>));
        _ = sc.AddSingleton<IUserContext>(new FakeUserContext("u1", true));
        _ = sc.AddSingleton<IAuthorizationService, FakeAuthorizationService>();
        _ = sc.AddSingleton<IIdempotencyStore, FakeIdempotencyStore>();
        _ = sc.AddSingleton<IOutbox, FakeOutbox>();
        _ = sc.AddSingleton<IUnitOfWork>(uow);

        ServiceProvider sp = sc.BuildServiceProvider();
        IMediator mediator = sp.GetRequiredService<IMediator>();

        int val = await mediator.Send(new SampleQuery(10));
        _ = val.Should().Be(11);

        _ = uow.Calls.Should().BeEmpty();
    }
}
