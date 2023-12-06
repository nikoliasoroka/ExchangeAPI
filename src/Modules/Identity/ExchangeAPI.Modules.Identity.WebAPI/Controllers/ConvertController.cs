using ExchangeAPI.Modules.Identity.Application.Features.Convert.Queries;
using ExchangeAPI.Shared;
using ExchangeAPI.Shared.Common.Models.Result.Implementations;
using ExchangeAPI.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExchangeAPI.Modules.Identity.WebAPI.Controllers;

[Route("web/convert")]
public class ConvertController : ApiControllerBase
{
    [SwaggerOperation("[WEB] Convert any money value from one currency to another")]
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(ConvertResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorInfo), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromQuery] ConvertCommand command, CancellationToken cancellationToken)
        => (await Mediator.Send(command, cancellationToken)).ToActionResult();
}
