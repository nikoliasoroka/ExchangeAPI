using ExchangeAPI.Modules.Identity.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Services.Identity;

internal class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserEmail => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
    public string? FullUserName => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
    public string[]? UserRoles => _httpContextAccessor.HttpContext?.User.Claims.Where(c => c.Type.Equals(ClaimTypes.Role)).Select(x => x.Value).ToArray();
}