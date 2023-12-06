using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Context;
using ExchangeAPI.Modules.Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Context;

public class DataContext : IdentityDbContext<
    AppUser,
    Role,
    string,
    UserClaim,
    UserRole,
    UserLogin,
    IdentityRoleClaim<string>,
    UserToken>,
    IDataContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    { }

    public DbSet<AppUser> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<ActionType> ActionTypes { get; set; }
    public DbSet<AccessRight> AccessRights { get; set; }
    public DbSet<UserToken> Tokens { get; set; }
    public DbSet<Activity> Activity { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}