using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Mediator;
using Bmf.Api.Boilerplate.Application.Ports;
using Bmf.Api.Boilerplate.Application.Tests.Fakes;
using Bmf.Api.Boilerplate.Application.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bmf.Api.Boilerplate.Application.Tests.DI;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void Registers_Mediator_Handlers_Validators_And_Pipeline()
    {
        ServiceCollection sc = new();
        _ = sc.AddApplication(typeof(SampleCommandHandler).Assembly);

        // supply behavior deps
        _ = sc.AddSingleton<ICorrelationContext>(new FakeCorrelationContext("corr"));
        _ = sc.AddSingleton(typeof(ILogging<>), typeof(FakeLogging<>));
        _ = sc.AddSingleton<IUserContext>(new FakeUserContext("u1", true));
        _ = sc.AddSingleton<IAuthorizationService, FakeAuthorizationService>();
        _ = sc.AddSingleton<IIdempotencyStore, FakeIdempotencyStore>();
        _ = sc.AddSingleton<IOutbox, FakeOutbox>();
        _ = sc.AddSingleton<IUnitOfWork, FakeUnitOfWork>();

        ServiceProvider sp = sc.BuildServiceProvider();

        _ = sp.GetRequiredService<IMediator>().Should().NotBeNull();
        _ = sp.GetRequiredService<IRequestHandler<SampleCommand, string>>().Should().NotBeNull();

        // the validator should be discoverable via FluentValidation's service
        Type vt = typeof(FluentValidation.IValidator<>).MakeGenericType(typeof(SampleCommand));
        object v = sp.GetRequiredService(vt);
        _ = v.Should().NotBeNull();
    }
}
