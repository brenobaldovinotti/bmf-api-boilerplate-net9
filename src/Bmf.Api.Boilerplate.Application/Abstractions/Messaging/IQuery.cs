namespace Bmf.Api.Boilerplate.Application.Abstractions.Messaging;

/// <summary>Query request producing a response.</summary>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse> { }
