using ExchangeAPI.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("AspNetRoles");

        builder.HasKey(r => r.Id);

        builder.Property(p => p.Id).HasMaxLength(128);

        builder.Property(p => p.Name).HasMaxLength(256).IsRequired();

        builder.Property(x => x.DisplayName).HasMaxLength(256);
    }
}