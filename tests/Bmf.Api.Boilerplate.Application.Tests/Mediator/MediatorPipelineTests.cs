using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Abstractions.Samples;
using Bmf.Api.Boilerplate.Application.DependencyInjection;
using Bmf.Api.Boilerplate.Application.Mediator;
using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Application.Tests.Fakes;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Xunit;

namespace Bmf.Api.Boilerplate.Application.Tests.Mediator;

public class MediatorPipelineTests
{
    private static ServiceProvider BuildServices(Action<ServiceCollection>? mutate = null)
    {
        ServiceCollection services = new();

        // ports fakes
        _ = services.AddSingleton<IAuthorizationService, FakeAuthz>();
        _ = services.AddSingleton<IIdempotencyStore, FakeIdempotencyStore>();
        _ = services.AddSingleton<IUnitOfWork, FakeUnitOfWork>();
        _ = services.AddSingleton<IOutbox, FakeOutbox>();
        _ = services.AddSingleton(typeof(ILogging<>), typeof(FakeLogger<>));
        _ = services.AddSingleton<ICorrelationContext, FakeCorrelationContext>();

        // app core
        _ = services.AddApplicationCore(Assembly.GetExecutingAssembly(), typeof(Echo).Assembly);

        mutate?.Invoke(services);
        return services.BuildServiceProvider(validateScopes: true);
    }

    [Fact]
    public async Task Pipeline_runs_in_configured_order_and_calls_handler()
    {
        ServiceProvider sp = BuildServices();
        IMediator mediator = sp.GetRequiredService<IMediator>();

        string response = await mediator.Send(new Echo("ok"));

        _ = response.Should().Be("ok");

        // Verify ports got touched
        FakeUnitOfWork? uow = sp.GetRequiredService<IUnitOfWork>() as FakeUnitOfWork;
        FakeOutbox? outbox = sp.GetRequiredService<IOutbox>() as FakeOutbox;
        _ = uow!.Begin.Should().Be(1);
        _ = uow.Commit.Should().Be(1);
        _ = uow.Rollback.Should().Be(0);
        _ = outbox!.Flushes.Should().Be(1);
    }

    [Fact]
    public async Task Validation_runs_first_and_short_circuits_on_error()
    {
        ServiceProvider sp = BuildServices();
        IMediator mediator = sp.GetRequiredService<IMediator>();

        Func<Task<string>> act = async () => await mediator.Send(new Echo(""));

        _ = await act.Should().ThrowAsync<FluentValidation.ValidationException>();

        // No UoW when validation fails
        FakeUnitOfWork? uow = sp.GetRequiredService<IUnitOfWork>() as FakeUnitOfWork;
        _ = uow!.Begin.Should().Be(0);
        _ = uow.Commit.Should().Be(0);
        _ = uow.Rollback.Should().Be(0);
    }

    [Fact]
    public async Task Authorization_runs_after_validation_and_can_block()
    {
        ServiceProvider sp = BuildServices();
        FakeAuthz? authz = sp.GetRequiredService<IAuthorizationService>() as FakeAuthz;
        authz!.Throw = true;

        IMediator mediator = sp.GetRequiredService<IMediator>();
        Func<Task<string>> act = async () => await mediator.Send(new Echo("ok"));

        _ = await act.Should().ThrowAsync<UnauthorizedAccessException>();

        FakeUnitOfWork? uow = sp.GetRequiredService<IUnitOfWork>() as FakeUnitOfWork;
        _ = uow!.Begin.Should().Be(0); // did not start tx
    }

    [Fact]
    public async Task Idempotency_blocks_duplicates_before_transaction()
    {
        ServiceProvider sp = BuildServices();
        FakeIdempotencyStore? idem = sp.GetRequiredService<IIdempotencyStore>() as FakeIdempotencyStore;
        idem!.PretendDuplicate = true;

        IMediator mediator = sp.GetRequiredService<IMediator>();
        Func<Task<string>> act = async () => await mediator.Send(new Echo("ok"));

        _ = await act.Should().ThrowAsync<InvalidOperationException>();

        FakeUnitOfWork? uow = sp.GetRequiredService<IUnitOfWork>() as FakeUnitOfWork;
        _ = uow!.Begin.Should().Be(0);
    }

    [Fact]
    public async Task Transaction_rolls_back_on_handler_errors()
    {
        // Replace EchoHandler with one that throws
        ServiceProvider sp = BuildServices(services =>
        {
            _ = services.AddTransient<IRequestHandler<Echo, string>>(_ => new ThrowingEchoHandler());
        });

        IMediator mediator = sp.BuildServiceProvider().GetRequiredService<IMediator>();

        Func<Task<string>> act = async () => await mediator.Send(new Echo("boom"));
        _ = await act.Should().ThrowAsync<InvalidOperationException>();

        FakeUnitOfWork uow = sp.BuildServiceProvider().GetRequiredService<IUnitOfWork>() as FakeUnitOfWork;
        _ = uow!.Begin.Should().Be(1);
        _ = uow.Commit.Should().Be(0);
        _ = uow.Rollback.Should().Be(1);
    }

    private sealed class ThrowingEchoHandler : IRequestHandler<Echo, string>
    {
        public Task<string> Handle(Echo request, CancellationToken ct)
        {
            throw new InvalidOperationException("handler failed");
        }
    }
}
