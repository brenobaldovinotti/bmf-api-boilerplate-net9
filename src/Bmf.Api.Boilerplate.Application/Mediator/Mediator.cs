using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bmf.Api.Boilerplate.Application.Mediator;

/// <summary>
/// Minimal mediator that resolves the matching handler and composes registered pipeline behaviors in registration order.
/// </summary>
public sealed class Mediator(IServiceProvider serviceProvider) : IMediator
{
    /// <inheritdoc />
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        Type requestType = request.GetType();
        Type handlerInterfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        object handler = serviceProvider.GetRequiredService(handlerInterfaceType);

        Task<TResponse> terminal()
        {
            MethodInfo? handleMethod = handlerInterfaceType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle)) ??
                throw new MissingMethodException(handlerInterfaceType.FullName, "Handle");

            object? result = handleMethod.Invoke(handler, [request, ct]);
            return (Task<TResponse>)result!;
        }

        Type behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        IEnumerable<object> behaviors = serviceProvider.GetServices(behaviorType).Where(b => b != null)!;

        RequestHandlerDelegate<TResponse> next = terminal;

        foreach (object behavior in behaviors.Reverse())
        {
            MethodInfo? handle = behaviorType.GetMethod(nameof(IPipelineBehavior<IRequest<TResponse>, TResponse>.Handle)) ??
                throw new MissingMethodException(behaviorType.FullName, "Handle");

            RequestHandlerDelegate<TResponse> capturedNext = next;
            next = () =>
            {
                object? invokeResult = handle.Invoke(behavior, [request, capturedNext, ct]);
                return (Task<TResponse>)invokeResult!;
            };
        }

        return next();
    }
}
