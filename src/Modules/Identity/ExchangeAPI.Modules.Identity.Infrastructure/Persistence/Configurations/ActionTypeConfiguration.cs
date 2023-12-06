using ExchangeAPI.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Persistence.Configurations;

public class ActionTypeConfiguration : IEntityTypeConfiguration<ActionType>
{
    public void Configure(EntityTypeBuilder<ActionType> builder)
    {
        builder.ToTable("ActionType");

        builder.Property(t => t.Name).HasMaxLength(50);

        builder.Property(t => t.Description).HasMaxLength(200);
    }
}