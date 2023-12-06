using ExchangeAPI.Modules.Identity.Shared.DTOs;
using ExchangeAPI.Shared.Common.Models.Result.Abstractions.Generics;

namespace ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;

public interface IOpenExchangeRatesService
{
    Task<IResult<OpenExchangeResponse>> ConvertAsync(decimal value, string from, string to);
}