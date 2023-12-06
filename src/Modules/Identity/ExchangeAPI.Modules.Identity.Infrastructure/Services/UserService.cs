using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Context;
using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;
using ExchangeAPI.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Services;

internal class UserService : IUserService
{
    private readonly IDataContext _context;

    public UserService(IDataContext context)
    {
        _context = context;
    }

    public async Task<bool> UserHasActionType(string type, AppUser user)
    {
        if (IsUserSuperAdmin(user))
            return true;

        var accessRights = await GetUserAccessRights(user.Id);

        var actionType = ActionTypes.GetByName(type);

        return accessRights.Contains(actionType.Name);
    }

    public bool IsUserSuperAdmin(AppUser user)
    {
        return user.UserRoles.Any() && user.UserRoles.Any(c => c.Role.Name == SystemRoles.Administrator.Name);
    }

    public async Task<List<string>> GetUserAccessRights(string userId)
    {
        try
        {
            var userRoleIds = await _context.UserRoles
                .Where(x => x.UserId.Equals(userId))
                .Select(x => x.RoleId)
                .ToListAsync();

            var accessRights = await _context.AccessRights
                .Where(x => userRoleIds.Contains(x.RoleId))
                .Select(x => x.ActionType.Name)
                .Distinct()
                .ToListAsync();

            return accessRights;
        }
        catch (Exception ex)
        {
            return new List<string>();
        }
    }
}
