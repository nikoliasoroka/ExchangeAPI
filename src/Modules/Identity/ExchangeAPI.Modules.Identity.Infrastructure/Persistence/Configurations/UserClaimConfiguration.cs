using ExchangeAPI.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Configurations;

public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder
            .Property(p => p.ClaimType)
            .HasMaxLength(256);

        builder
            .Property(p => p.ClaimValue)
            .HasMaxLength(256);

        builder
            .Property(p => p.UserId)
            .HasMaxLength(128);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.UserClaims)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
    }
}