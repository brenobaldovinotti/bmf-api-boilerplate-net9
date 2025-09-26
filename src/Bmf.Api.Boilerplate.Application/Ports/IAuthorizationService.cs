namespace Bmf.Api.Boilerplate.Application.Ports;

/// <summary>Evaluates authorization policies.</summary>
public interface IAuthorizationService
{
    /// <summary>Ensure current user is authorized for the request and policy.</summary>
    Task EnsureAuthorizedAsync(IUserContext user, string policy, object request, CancellationToken ct);
}

