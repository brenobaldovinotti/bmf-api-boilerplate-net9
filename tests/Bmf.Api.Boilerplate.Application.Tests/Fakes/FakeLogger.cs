using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeLogger<T> : ILogging<T>
{
    public readonly List<string> Messages = [];

    public void Info(string message)
    {
        Messages.Add("I:" + message);
    }

    public void Warn(string message)
    {
        Messages.Add("W:" + message);
    }

    public void Error(string message, Exception? ex = null)
    {
        Messages.Add("E:" + message);
    }
}
