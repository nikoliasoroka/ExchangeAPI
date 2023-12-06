using ExchangeAPI.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Configurations;

public class AccessRightConfiguration : IEntityTypeConfiguration<AccessRight>
{
    public void Configure(EntityTypeBuilder<AccessRight> builder)
    {
        builder.ToTable("AccessRights");

        builder.HasKey(x => new { x.RoleId, x.ActionTypeId });

        builder.Property(t => t.RoleId).HasMaxLength(128);

        builder.Property(t => t.ActionTypeId);

        builder.HasOne(r => r.Role)
            .WithMany(a => a.AccessRights)
            .HasForeignKey(r => r.RoleId)
            .HasConstraintName("FK_dbo.AccessRights_dbo.AspNetRoles_RoleId");

        builder.HasOne(a => a.ActionType)
            .WithMany(a => a.AccessRights)
            .HasForeignKey(a => a.ActionTypeId)
            .HasConstraintName("FK_dbo.AccessRights_dbo.ActionType_ActionTypeId");
    }
}