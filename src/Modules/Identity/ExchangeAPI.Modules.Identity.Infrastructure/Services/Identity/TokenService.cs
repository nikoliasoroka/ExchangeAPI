using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;
using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services.Identity;
using ExchangeAPI.Modules.Identity.Application.Features.Account.Commands.Login;
using ExchangeAPI.Modules.Identity.Domain.Entities;
using ExchangeAPI.Shared.Common.Constants;
using ExchangeAPI.Shared.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Services.Identity;

internal class TokenService : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IUserService _userService;

    public TokenService(
        UserManager<AppUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<UserLoginResponse> GenerateWebTokenResponse(AppUser user, bool rememberMe = false, CancellationToken cancellationToken = default)
    {
        var refreshTokenExpirationTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetime);

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = refreshTokenExpirationTime;

        await _userManager.UpdateAsync(user);
        var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

        var claims = await GetClaimsAsync(user, CustomClaimTypes.Web);

        var token = GenerateJwtTokenAsync(claims);

        return new UserLoginResponse
        {
            Token = token,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiryTime = refreshTokenExpirationTime,
            TwoFactorEnabled = user.TwoFactorEnabled,
            TwoFactorConfigured = providers.Contains("Authenticator"),
        };
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException(ErrorModel.InvalidToken);
        }

        return principal;
    }

    #region Private

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private string? GenerateJwtTokenAsync(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

#if (DEBUG)
        var expiresAt = DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenLifetime);
#else
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenLifetime);
#endif

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            notBefore: DateTime.UtcNow,
            claims: claims,
            expires: expiresAt,
            signingCredentials: signinCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);

        return encryptedToken;
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync(AppUser user, string claimType)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var permissions = await _userService.GetUserAccessRights(user.Id);

        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

        var permissionClaims = permissions.Select(permission => new Claim(CustomClaimTypes.Permissions, permission)).ToList();

        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FullUsername ?? string.Empty),
                new(CustomClaimTypes.IsEmailConfirmed, user.EmailConfirmed.ToString()),
                new(CustomClaimTypes.ClaimType, claimType)
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims)
            .ToList();

        return claims;
    }

    #endregion
}