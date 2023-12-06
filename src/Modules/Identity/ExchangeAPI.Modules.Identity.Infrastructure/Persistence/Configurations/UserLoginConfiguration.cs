using ExchangeAPI.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Configurations;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable("AspNetUserLogins");

        builder.HasKey(k => new { k.UserId, k.ProviderKey, k.LoginProvider }).HasName("PK_dbo.AspNetUserLogins");

        builder.Property(p => p.ProviderKey).HasMaxLength(256);

        builder.Property(p => p.LoginProvider).HasMaxLength(256);

        builder.Property(p => p.UserId).HasMaxLength(128);

        builder.HasOne(d => d.User)
            .WithMany(p => p.UserLogins)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
    }
}