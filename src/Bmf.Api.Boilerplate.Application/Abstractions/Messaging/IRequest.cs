namespace Bmf.Api.Boilerplate.Application.Abstractions.Messaging;

/// <summary>Represents a request that produces a response.</summary>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface IRequest<out TResponse> { }
