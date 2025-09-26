using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;

namespace Bmf.Api.Boilerplate.Application.Tests.Samples;

public sealed class SampleQuery(int value) : IQuery<int>
{
    public int Value { get; } = value;
}
