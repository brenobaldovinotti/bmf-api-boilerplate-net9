using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Ports;
using System.Diagnostics;

namespace Bmf.Api.Boilerplate.Application.Behaviors;

/// <summary>Logs request execution and emits tracing Activity.</summary>
public sealed class LoggingTracingBehavior<TRequest, TResponse>(ILogging<TRequest> logger, ICorrelationContext correlation)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        ActivitySource source = new("Bmf.Api.Boilerplate.Application");
        using Activity? activity = source.StartActivity(typeof(TRequest).Name);

        if (activity is not null)
        {
            if (correlation.CorrelationId is not null)
            {
                _ = activity.SetTag("correlation.id", correlation.CorrelationId);
            }

            if (correlation.CausationId is not null)
            {
                _ = activity.SetTag("causation.id", correlation.CausationId);
            }
        }

        logger.Info("Handling {RequestType}", typeof(TRequest).FullName ?? typeof(TRequest).Name);

        try
        {
            TResponse response = await next().ConfigureAwait(false);
            logger.Info("Handled {RequestType}", typeof(TRequest).FullName ?? typeof(TRequest).Name);
            return response;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Failure in {RequestType}", typeof(TRequest).FullName ?? typeof(TRequest).Name);
            throw;
        }
    }
}
