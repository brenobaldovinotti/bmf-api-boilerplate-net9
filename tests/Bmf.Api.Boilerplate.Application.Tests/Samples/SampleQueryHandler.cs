using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;

namespace Bmf.Api.Boilerplate.Application.Tests.Samples;

public sealed class SampleQueryHandler : IRequestHandler<SampleQuery, int>
{
    public Task<int> Handle(SampleQuery request, CancellationToken ct)
    {
        return Task.FromResult(request.Value + 1);
    }
}
