using ExchangeAPI.Modules.Identity.Domain.Common.Comparators;
using ExchangeAPI.Modules.Identity.Domain.Entities;
using ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Extensions;

public static class MigrationManager
{
    public static void MigrateDatabase(this IApplicationBuilder app)
    {
        try
        {
            var webApp = (WebApplication)app;
            using var scope = webApp.Services.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            appContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void SeedStandardRoles(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        try
        {
            var standardRoles = SystemRoles.All.ToList();
            var databaseRoles = context.Roles.ToList();

            var rolesNotInDatabase = standardRoles.Except(databaseRoles, new RoleEqualityComparer())
                .ToList();

            if (!rolesNotInDatabase.Any())
                return;

            context.Roles.AddRange(rolesNotInDatabase);

            context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void SeedActionTypes(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        try
        {
            var actionTypesDb = context.ActionTypes.ToList();
            var actionTypes = ActionTypes.All.ToList();

            var deleteActions = actionTypesDb.Where(x => actionTypes
                .All(c => !string.Equals(c.Name, x.Name, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();

            var insertActions = actionTypes.Where(x => actionTypesDb
                .All(c => !string.Equals(c.Name, x.Name, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();

            context.ActionTypes.AddRange(insertActions);
            context.ActionTypes.RemoveRange(deleteActions);

            context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void SeedAdminUser(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        using var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<AppUser>>();

        try
        {
            var adminRole = context.Roles.FirstOrDefault(x => x.Name.Equals(SystemRoles.Administrator.Name));

            var admin = new AppUser()
            {
                UserName = "admin",
                Id = Guid.NewGuid().ToString(),
                Email = "admin_email@test.com",
                LastActivityTime = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                EmailConfirmed = true,
                UserRoles = new List<UserRole>() { new() { RoleId = adminRole.Id } }
            };

            if (!context.Users.Any())
            {
                var result = userManager.CreateAsync(admin, "Admin1!").GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void SeedAccessRights(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        try
        {
            var standardRolesNames = SystemRoles.All.Select(r => r.Name).ToList();

            var roles = context.Roles
                .Include(r => r.AccessRights)
                    .ThenInclude(accessRight => accessRight.ActionType)
                .Where(role => standardRolesNames.Contains(role.Name))
                .ToList();

            var actionTypesDb = context.ActionTypes.ToList();

            var defaultRoleAccessRights = GetDefaultAccessRights();

            // update roles with defaultRoleAccessRights if role does not have it
            foreach (var role in roles)
            {
                if (defaultRoleAccessRights.TryGetValue(role.Name!, out var accessRights))
                {
                    var newAccessRights = accessRights
                        .Where(actionType => role.AccessRights.All(ar => ar.ActionType.Name != actionType.Name))
                        .Select(actionType => new AccessRight
                        {
                            ActionType = actionTypesDb
                                .FirstOrDefault(x => x.Name.Equals(actionType.Name, StringComparison.InvariantCultureIgnoreCase))
                        })
                        .ToList();

                    // Add the new access rights to the role
                    role.AccessRights.AddRange(newAccessRights);
                }
            }

            context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private static Dictionary<string, List<ActionType>> GetDefaultAccessRights()
    {
        var accessRights = new Dictionary<string, List<ActionType>>()
        {
            {
                SystemRoles.Administrator.Name!, ActionTypes.All.ToList()
            },
            {
                SystemRoles.CompanyManager.Name!, new List<ActionType>()
                {
                    ActionTypes.LoginBackend,
                    ActionTypes.ViewConfig
                }
            }
        };

        return accessRights;
    }
}