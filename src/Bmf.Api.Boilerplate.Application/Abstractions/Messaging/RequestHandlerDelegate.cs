namespace Bmf.Api.Boilerplate.Application.Abstractions.Messaging;

/// <summary>Terminal delegate used to advance the pipeline.</summary>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
