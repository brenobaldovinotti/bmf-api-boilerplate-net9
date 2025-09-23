using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeAuthorizationService : IAuthorizationService
{
    public List<string> Calls { get; } = [];
    public bool ShouldThrowForbidden { get; set; }

    public Task EnsureAuthorizedAsync(IUserContext user, string policy, object request, CancellationToken ct)
    {
        Calls.Add(policy);

        return ShouldThrowForbidden
            ? throw new UnauthorizedAccessException("Forbidden")
            : !user.IsAuthenticated ? throw new UnauthorizedAccessException("Not authenticated") : Task.CompletedTask;
    }
}
