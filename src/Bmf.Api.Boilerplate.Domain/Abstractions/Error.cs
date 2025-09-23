namespace Bmf.Api.Boilerplate.Domain.Abstractions;

/// <summary>
/// Canonical error with code + message.
/// </summary>
public sealed record Error(string Code, string Message)
{
    public override string ToString()
    {
        return $"{Code}: {Message}";
    }

    public static class Common
    {
        public static readonly Error Validation = new("validation", "Validation failed.");
        public static readonly Error NotFound = new("not_found", "Resource was not found.");
        public static readonly Error Conflict = new("conflict", "A conflict occurred.");
        public static readonly Error Unauthorized = new("unauthorized", "Authentication required.");
        public static readonly Error Forbidden = new("forbidden", "Not permitted to perform this action.");
    }
}
