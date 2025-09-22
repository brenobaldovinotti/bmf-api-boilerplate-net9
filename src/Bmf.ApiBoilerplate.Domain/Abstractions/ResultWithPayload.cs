namespace Bmf.ApiBoilerplate.Domain.Abstractions;

/// <summary>
/// Result with a payload when successful.
/// </summary>
public readonly struct Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public Error? Error { get; }

    private Result(bool isSuccess, T? data, Error? error)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }

    public static Result<T> Success(T data)
    {
        return new(true, data, null);
    }

    public static Result<T> Failure(Error error)
    {
        return new(false, default, error);
    }

    /// <summary>Returns the value or throws when failed (use sparingly).</summary>
    public T UnwrapOrThrow()
    {
        return !IsSuccess ?
            throw new InvalidOperationException(Error?.ToString() ?? "Unexpected failure.") :
            Data!;
    }
}
