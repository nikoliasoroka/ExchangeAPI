using ExchangeAPI.Modules.Identity.Application.Features.Account.Commands.Login;
using ExchangeAPI.Modules.Identity.Application.Features.Account.Queries.Activity;
using ExchangeAPI.Shared;
using ExchangeAPI.Shared.Common.Models;
using ExchangeAPI.Shared.Common.Models.Result.Implementations;
using ExchangeAPI.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExchangeAPI.Modules.Identity.WebAPI.Controllers;

[Route("web/account")]
public class AccountController : ApiControllerBase
{
    [SwaggerOperation("[WEB] Login and get access token")]
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorInfo), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserLoginCommand command, CancellationToken cancellationToken)
        => (await Mediator.Send(command, cancellationToken)).ToActionResult();

    [SwaggerOperation("Get activity")]
    [Authorize]
    [HttpGet("activity")]
    [ProducesResponseType(typeof(PaginatedList<GetActivityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorInfo), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetActivity([FromQuery] GetActivityQuery request)
        => (await Mediator.Send(request)).ToActionResult();
}
