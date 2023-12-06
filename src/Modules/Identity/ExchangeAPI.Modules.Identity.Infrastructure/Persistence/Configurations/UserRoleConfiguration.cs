using ExchangeAPI.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("AspNetUserRoles");

        builder.HasKey(t => new { t.UserId, t.RoleId }).HasName("PK_dbo.AspNetUserRoles");

        builder.Property(x => x.UserId).HasMaxLength(128);

        builder.Property(x => x.RoleId).HasMaxLength(128);

        builder.HasOne(d => d.Role)
            .WithMany(p => p.UserRoles)
            .HasForeignKey(d => d.RoleId)
            .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

        builder.HasOne(d => d.User)
            .WithMany(p => p.UserRoles)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");
    }
}