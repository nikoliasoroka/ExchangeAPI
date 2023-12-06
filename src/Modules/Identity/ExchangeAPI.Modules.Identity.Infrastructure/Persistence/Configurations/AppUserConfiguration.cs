using ExchangeAPI.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AspNetUsers");

        builder.Property(p => p.Id).HasMaxLength(128);

        builder.Property(p => p.Email).HasMaxLength(256).IsRequired(false);

        builder.Property(p => p.PasswordHash).HasMaxLength(256).IsRequired(false);

        builder.Property(p => p.PhoneNumber).HasMaxLength(256).IsRequired(false);

        builder.Property(p => p.UserName).HasMaxLength(256);

        builder.HasIndex(e => e.UserName).IsUnique();

        builder.Property(p => p.FullName).HasMaxLength(256);

        builder.Property(p => p.Firstname).HasMaxLength(256);

        builder.Property(p => p.SecurityStamp).HasMaxLength(256).IsRequired(false);

        builder.Property(p => p.FullUsername).HasMaxLength(256);

        builder.Property(p => p.LockoutEndDateUtc).HasColumnType("datetime");

        builder.Property(p => p.TwoFactorEnd).HasColumnType("datetime");

        builder.Property(p => p.LastModifiedDate).HasColumnType("datetime").IsRequired();

        builder.Property(p => p.LastActivityTime).HasColumnType("datetime").IsRequired();

        builder.Property(p => p.RefreshToken).HasMaxLength(256);

        builder.Property(p => p.RefreshTokenExpiryTime).HasColumnType("datetime");
    }
}