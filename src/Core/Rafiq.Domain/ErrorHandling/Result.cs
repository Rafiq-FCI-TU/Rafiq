namespace Rafiq.Domain.ErrorHandling;

public class Result
{
    public bool IsSuccess { get; }
    public int StatusCode { get; }
    public string Message { get; }

    protected Result(bool isSuccess, int statusCode, string? message)
    {
        if ((isSuccess && statusCode != 200) || (!isSuccess && statusCode == 200)) throw new InvalidOperationException("Invalid status code for result type.");

        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Message = string.IsNullOrEmpty(message) ? GetDefaultMessageForStatusCode(statusCode) : message;
    }

    private static string GetDefaultMessageForStatusCode(int statusCode) => statusCode switch
    {
        200 => "OK",
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        500 => "Internal Server Error",
        _ => "Unexpected Error"
    };

    public static Result Success(string message = "OK") => new(true, 200, message);
    public static Result<T> Success<T>(T value, string message = "OK") => new(value, true, 200, message);

    public static Result Failure(int statusCode, string? message) => new(false, statusCode, message);
    public static Result<T> Failure<T>(int statusCode, string message) => new(default, false, statusCode, message);

    public static Result BadRequest(string message = "Bad Request") => new(false, 400, message);
    public static Result Unauthorized(string message = "Unauthorized") => new(false, 401, message);
    public static Result NotFound(string message = "Not Found") => new(false, 404, message);
    public static Result ServerError(string message = "Internal Server Error") => new(false, 500, message);
}

public class Result<T> : Result
{
    public T? Value { get; }

    internal Result(T? value, bool isSuccess, int statusCode, string? message) : base(isSuccess, statusCode, message)
    {
        Value = value;
    }
}