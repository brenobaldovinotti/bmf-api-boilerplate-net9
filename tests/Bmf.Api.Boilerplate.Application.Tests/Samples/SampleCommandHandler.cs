using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;

namespace Bmf.Api.Boilerplate.Application.Tests.Samples;

public sealed class SampleCommandHandler : IRequestHandler<SampleCommand, string>
{
    public List<string> Calls { get; } = [];
    public bool ShouldThrow { get; set; }

    public Task<string> Handle(SampleCommand request, CancellationToken ct)
    {
        Calls.Add(request.Payload);
        return ShouldThrow ?
            throw new InvalidOperationException("boom") :
            Task.FromResult($"ok:{request.Payload}");
    }
}
