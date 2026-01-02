namespace SourceAPI.Common;

/// <summary>
/// 操作の成功/失敗を表すResult型
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? ErrorMessage { get; }
    public Exception? Exception { get; }

    protected Result(bool isSuccess, string? errorMessage = null, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Exception = exception;
    }

    public static Result Success() => new(true);

    public static Result Failure(string errorMessage, Exception? exception = null)
        => new(false, errorMessage, exception);
}

/// <summary>
/// 値を持つ操作の成功/失敗を表すResult型
/// </summary>
/// <typeparam name="T">成功時の値の型</typeparam>
public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value = default, string? errorMessage = null, Exception? exception = null)
        : base(isSuccess, errorMessage, exception)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value);

    public new static Result<T> Failure(string errorMessage, Exception? exception = null)
        => new(false, default, errorMessage, exception);
}
