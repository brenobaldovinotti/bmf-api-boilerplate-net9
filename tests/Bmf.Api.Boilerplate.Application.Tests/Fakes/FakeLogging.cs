using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeLogging<T> : ILogging<T>
{
    public List<string> InfoMessages { get; } = [];
    public List<string> Warnings { get; } = [];
    public List<string> Errors { get; } = [];

    public void Info(string messageTemplate, params object[] args)
    {
        InfoMessages.Add(string.Format(messageTemplate.Replace("{RequestType}", "{0}"), args));
    }

    public void Warning(string messageTemplate, params object[] args)
    {
        Warnings.Add(string.Format(messageTemplate, args));
    }

    public void Error(Exception exception, string messageTemplate, params object[] args)
    {
        Errors.Add($"{exception.GetType().Name}:{string.Format(messageTemplate.Replace("{RequestType}", "{0}"), args)}");
    }
}
