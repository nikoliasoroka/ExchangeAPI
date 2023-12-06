using ExchangeAPI.Shared.Common.Models.Result.Implementations;

namespace ExchangeAPI.Shared.Common.Models.Result.Abstractions;

public interface IResult
{
    bool Success { get; }

    ErrorInfo ErrorInfo { get; }
}