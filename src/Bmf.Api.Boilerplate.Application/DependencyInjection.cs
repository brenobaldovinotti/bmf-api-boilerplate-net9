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

        // Handlers: any closed IRequestHandler<,> in the given assembly
        IEnumerable<Type> handlerTypes = scanAssembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Select(t => new { Impl = t, Ifaces = t.GetInterfaces() })
            .SelectMany(x => x.Ifaces.Select(i => new { x.Impl, Iface = i }))
            .Where(x => x.Iface.IsGenericType && x.Iface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
            .Select(x => x.Impl);

        foreach (Type impl in handlerTypes.Distinct())
        {
            foreach (Type serviceType in impl.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
            {
                _ = services.AddScoped(serviceType, impl);
            }
        }

        // Validators: register IValidator<T> implementations
        IEnumerable<Type> validatorTypes = scanAssembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Select(t => new { Impl = t, Ifaces = t.GetInterfaces() })
            .SelectMany(x => x.Ifaces.Select(i => new { x.Impl, Iface = i }))
            .Where(x => x.Iface.IsGenericType && x.Iface.GetGenericTypeDefinition() == typeof(IValidator<>))
            .Select(x => x.Impl);

        foreach (Type impl in validatorTypes.Distinct())
        {
            foreach (Type serviceType in impl.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
            {
                _ = services.AddScoped(serviceType, impl);
            }
        }

        return services;
    }
}
