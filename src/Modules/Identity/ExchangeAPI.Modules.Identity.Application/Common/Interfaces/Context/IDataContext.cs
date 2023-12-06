using ExchangeAPI.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Context;

public interface IDataContext
{
    public DatabaseFacade Database { get; }
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    EntityEntry Entry(object entity);

    public DbSet<AppUser> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<ActionType> ActionTypes { get; set; }
    public DbSet<AccessRight> AccessRights { get; set; }
    public DbSet<UserToken> Tokens { get; set; }
    public DbSet<Activity> Activity { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}