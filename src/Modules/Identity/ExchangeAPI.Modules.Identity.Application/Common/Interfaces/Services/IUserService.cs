using ExchangeAPI.Modules.Identity.Domain.Entities;

namespace ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;

public interface IUserService
{
    Task<bool> UserHasActionType(string type, AppUser user);
    bool IsUserSuperAdmin(AppUser user);
    Task<List<string>> GetUserAccessRights(string userId);
}
