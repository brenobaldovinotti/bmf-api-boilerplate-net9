using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Mediator;
using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Application.Tests.Fakes;
using Bmf.Api.Boilerplate.Application.Tests.Handlers;
using Bmf.Api.Boilerplate.Application.Tests.Requests;
using Bmf.Api.Boilerplate.Application.Tests.Validators;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using Xunit;

namespace Bmf.Api.Boilerplate.Application.Tests.Pipeline;
public sealed class PipelineOrderTests
{
    [Fact]
    public async Task Behaviors_execute_in_the_configured_order_and_handler_is_invoked()
    {
        ConcurrentQueue<string> trace = new ConcurrentQueue<string>();

        ServiceCollection services = new ServiceCollection();

        // Application core
        _ = services.AddApplication(typeof(EchoHandler).Assembly);

        // Replace ports with fakes that record order
        _ = services.AddSingleton<IUserContext>(_ => new FakeUserContext("user-1", new[] { "Admin" }));
        _ = services.AddSingleton<IIdempotencyStore, FakeIdempotencyStore>();
        _ = services.AddSingleton<IUnitOfWork>(_ => new FakeUnitOfWork(trace));
        _ = services.AddSingleton<IOutbox, FakeOutbox>();
        _ = services.AddSingleton<ICorrelationContext, FakeCorrelationContext>();
        _ = services.AddSingleton(typeof(IAppLogger<>), typeof(FakeLogger<>));

        // Inject a simple AuthZ rule that records when it runs
        _ = services.AddTransient(typeof(IAuthorizationRule<Echo>), _ => new FakeAuthorizationRule<Echo>(trace));

        // Validators for Echo
        _ = services.AddTransient<IValidator<Echo>, EchoValidator>();

        // Decorate the real behaviors to record order without changing behavior implementation
        _ = services.Decorate(typeof(IPipelineBehavior<,>), typeof(RecordingBehavior<,>), trace);

        ServiceProvider provider = services.BuildServiceProvider();
        IMediator mediator = provider.GetRequiredService<IMediator>();

        Echo req = new Echo("hello");

        string result = await mediator.Send(req, CancellationToken.None).ConfigureAwait(false);

        result.Should().Be("hello");

        // Expected order:
        // Validation → AuthZ → Idempotency → Tx.Begin → [Handler] → Tx.Commit → Logging
        string[] actual = trace.ToArray();

        actual.Should().ContainInOrder(new[]
        {
            "Validation",
            "AuthZ",
            "Idempotency",
            "Tx.Begin",
            "Handler",
            "Tx.Commit",
            "Log.Info" // from LoggingTracingBehavior
        });
    }
}
