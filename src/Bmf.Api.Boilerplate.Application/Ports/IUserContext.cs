namespace Bmf.Api.Boilerplate.Application.Ports;

/// <summary>Represents the current user principal.</summary>
public interface IUserContext
{
    /// <summary>User identifier.</summary>
    string? UserId { get; }

    /// <summary>True if the user is authenticated.</summary>
    bool IsAuthenticated { get; }
}
