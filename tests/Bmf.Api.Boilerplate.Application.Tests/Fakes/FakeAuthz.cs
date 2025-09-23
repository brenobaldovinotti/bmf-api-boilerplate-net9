using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeAuthz : IAuthorizationService
{
    public bool Throw { get; set; }
    public Task AuthorizeAsync<TRequest>(TRequest request, CancellationToken ct)
    {
        return Throw ? throw new UnauthorizedAccessException("nope") : Task.CompletedTask;
    }
}
