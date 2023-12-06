using ExchangeAPI.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable("UserTokens");

        builder.HasKey(k => new { k.UserId, k.LoginProvider, k.Name }).HasName("PK_UserTokens");

        builder.Property(x => x.UserId).HasMaxLength(128);

        builder.Property(x => x.LoginProvider).HasMaxLength(256);

        builder.Property(e => e.Name).HasMaxLength(450);

        builder.Property(p => p.ExpiryTime).HasColumnType("datetime");

        builder.HasOne(d => d.User)
            .WithMany(p => p.UserTokens)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK_UserTokens_AspNetUsers_UserId");
    }
}