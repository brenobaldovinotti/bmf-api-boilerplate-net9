using Bmf.Api.Boilerplate.Application.Ports;
using System.Collections.Concurrent;

namespace Bmf.Api.Boilerplate.Application.Tests.Fakes;

public sealed class FakeLogger<T>(ConcurrentQueue<string> trace) : ILogging<T>
{
    public void Info(string messageTemplate, params object[] args)
    {
        trace.Enqueue("Log.Info");
        _ = args;
    }

    public void Warning(string messageTemplate, params object[] args)
    {
        trace.Enqueue("Log.Warn");
        _ = args;
    }

    public void Error(Exception exception, string messageTemplate, params object[] args)
    {
        trace.Enqueue("Log.Error");
        _ = exception;
        _ = args;
    }
}
