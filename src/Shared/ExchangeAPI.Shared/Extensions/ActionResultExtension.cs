using ExchangeAPI.Shared.Common.Models.Result.Abstractions;
using ExchangeAPI.Shared.Common.Models.Result.Abstractions.Generics;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeAPI.Shared.Extensions;

public static class ActionResultExtension
{
    public static IActionResult ToActionResult<T>(this IResult<T> result) =>
        result.Success
            ? (IActionResult)new OkObjectResult(result.Data)
            : new BadRequestObjectResult(result.ErrorInfo);

    public static IActionResult ToActionResult(this IResult result) =>
        result.Success
            ? (IActionResult)new OkResult()
            : new BadRequestObjectResult(result.ErrorInfo);
}