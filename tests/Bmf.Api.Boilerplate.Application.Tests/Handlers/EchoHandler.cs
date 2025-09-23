using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Tests.Requests;

namespace Bmf.Api.Boilerplate.Application.Tests.Handlers;

public sealed class EchoHandler : IRequestHandler<Echo, string>
{
    public Task<string> Handle(Echo request, CancellationToken ct)
    {
        return Task.FromResult(request.Message);
    }
}
