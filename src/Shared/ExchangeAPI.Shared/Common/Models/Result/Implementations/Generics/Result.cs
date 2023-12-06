using ExchangeAPI.Shared.Common.Models.Result.Abstractions.Generics;

namespace ExchangeAPI.Shared.Common.Models.Result.Implementations.Generics;

public class Result<TData> : IResult<TData>
{
    public TData Data { get; private set; }

    public bool Success { get; private set; }

    public ErrorInfo ErrorInfo { get; private set; }

    private Result()
    { }

    public static Result<TData> CreateSuccess() =>
        new() { Success = true };

    public static Result<TData> CreateFailed(string message, string? stackTrace = null) =>
        new() { ErrorInfo = new ErrorInfo(message, stackTrace) };

    public static Result<TData> CreateSuccess(TData data) =>
        new()
        {
            Data = data,
            Success = true,
        };

    public virtual Result<TData> AddError(string message)
    {
        ErrorInfo.AddError(message);

        return this;
    }

    public virtual Result<TData> AddErrors(IEnumerable<string> collection)
    {
        ErrorInfo.AddErrors(collection);

        return this;
    }
}