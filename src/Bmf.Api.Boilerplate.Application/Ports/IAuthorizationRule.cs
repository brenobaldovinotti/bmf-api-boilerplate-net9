namespace Bmf.Api.Boilerplate.Application.Ports;

/// <summary>Authorization rule applied to a specific request type.</summary>
public interface IAuthorizationRule<in TRequest>
{
    Task Authorize(TRequest request, IUserContext user, CancellationToken ct);
}
