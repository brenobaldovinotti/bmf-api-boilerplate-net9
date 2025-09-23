namespace Bmf.Api.Boilerplate.Application.Ports;

/// <summary>Marker for requests that require authorization.</summary>
public interface IAuthorizableRequest
{
    /// <summary>Named policy to evaluate.</summary>
    string Policy { get; }
}
