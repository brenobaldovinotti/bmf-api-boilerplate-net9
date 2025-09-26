namespace Bmf.Api.Boilerplate.Application.Ports;

/// <summary>Application logger abstraction (Serilog planned in Infra).</summary>
public interface ILogging<T>
{
    void Info(string messageTemplate, params object[] args);
    void Warning(string messageTemplate, params object[] args);
    void Error(Exception exception, string messageTemplate, params object[] args);
}
