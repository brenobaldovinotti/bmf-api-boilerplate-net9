namespace Bmf.ApiBoilerplate.Domain.Abstractions;

/// <summary>
/// Lightweight result type for success/failure flows without exceptions.
/// </summary>
public readonly struct Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    private Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
    {
        return new(true, null);
    }

    public static Result Failure(Error error)
    {
        return new(false, error);
    }

    public static Result<T> Success<T>(T value)
    {
        return Result<T>.Success(value);
    }

    public static Result<T> Failure<T>(Error error)
    {
        return Result<T>.Failure(error);
    }
}
