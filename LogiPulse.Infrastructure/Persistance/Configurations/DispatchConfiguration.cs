using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogiPulse.Domain.Entities.Dispatches;

namespace LogiPulse.Infrastructure.Persistance.Configurations;

public class DispatchConfiguration : IEntityTypeConfiguration<Dispatch>
{
    public void Configure(EntityTypeBuilder<Dispatch> builder)
    {
        builder.ToTable("dispatches");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CurrentStatus)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.OwnsMany(x => x.StatusHistory, history =>
        {
            history.ToJson();
            history.Property(h => h.Status).HasConversion<string>();
        });

        // Índices
        builder.HasIndex(x => new { x.ExternalId, x.TenantId }).IsUnique();
    }
}