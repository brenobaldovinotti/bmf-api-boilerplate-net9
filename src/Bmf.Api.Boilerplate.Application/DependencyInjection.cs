using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Behaviors;
using Bmf.Api.Boilerplate.Application.Mediator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bmf.Api.Boilerplate.Application;

/// <summary>Application layer DI registration.</summary>
public static class DependencyInjection
{
    /// <summary>Adds mediator, behaviors, handlers, and validators to the service collection.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services, Assembly? assemblyWithHandlers = null)
    {
        Assembly scanAssembly = assemblyWithHandlers ?? Assembly.GetExecutingAssembly();

        // Mediator
        _ = services.AddSingleton<IMediator, Mediator.Mediator>();

        // Pipeline behaviors — registration order == execution order (outer → inner)
        _ = services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        _ = services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        _ = services.AddScoped(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));
        _ = services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionOutboxBehavior<,>));
        _ = services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingTracingBehavior<,>));

        // Handlers (any IRequestHandler<,> in the given assembly)
        _ = services.Scan(scan => scan
            .FromAssemblies(scanAssembly)
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // FluentValidation validators in assembly
        _ = services.AddValidatorsFromAssembly(scanAssembly);

        return services;
    }
}
