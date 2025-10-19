namespace Application.Abstractions.Results;
public class Result
{
    protected Result(bool isSuccess, string errorMessage, object? data = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Data = data;
    }

    public bool IsSuccess { get; }
    public string ErrorMessage { get; }
    public object? Data { get; }

    public static Result Success(object? data = null) => new(true, string.Empty, data);
    public static Result Error(string errorMessage, object? data = null) => new(false, errorMessage, data);
    public static Result Error(object? data = null) => new(false, string.Empty, data);
}
