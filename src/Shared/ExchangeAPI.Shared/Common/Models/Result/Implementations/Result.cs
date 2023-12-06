using ExchangeAPI.Shared.Common.Models.Result.Abstractions;

namespace ExchangeAPI.Shared.Common.Models.Result.Implementations;

public class Result : IResult
{
    public bool Success { get; private set; }

    public ErrorInfo ErrorInfo { get; private set; }

    private Result() { }

    public static Result CreateSuccess() =>
        new() { Success = true };

    public static Result CreateFailed(string message, string? stackTrace = null) =>
        new() { ErrorInfo = new ErrorInfo(message, stackTrace) };

    public virtual Result AddError(string message)
    {
        ErrorInfo.AddError(message);

        return this;
    }

    public virtual Result AddErrors(IEnumerable<string> collection)
    {
        ErrorInfo.AddErrors(collection);

        return this;
    }
}