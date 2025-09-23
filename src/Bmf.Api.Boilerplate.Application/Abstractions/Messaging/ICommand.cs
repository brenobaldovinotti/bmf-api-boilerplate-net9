namespace Bmf.Api.Boilerplate.Application.Abstractions.Messaging;

/// <summary>Command request that changes state and returns a response.</summary>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse> { }
