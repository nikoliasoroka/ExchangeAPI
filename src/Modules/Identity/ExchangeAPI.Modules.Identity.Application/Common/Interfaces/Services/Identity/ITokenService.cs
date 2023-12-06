using ExchangeAPI.Modules.Identity.Application.Features.Account.Commands.Login;
using ExchangeAPI.Modules.Identity.Domain.Entities;
using System.Security.Claims;

namespace ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services.Identity;

public interface ITokenService
{
    Task<UserLoginResponse> GenerateWebTokenResponse(AppUser user, bool rememberMe = false, CancellationToken cancellationToken = default);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}