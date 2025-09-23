using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeUserContext(string? userId, IReadOnlyCollection<string> roles) : IUserContext
{
    public string? UserId { get; } = userId;

    public bool IsAuthenticated => throw new NotImplementedException();
}
