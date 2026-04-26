using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Vehicles;

namespace LogiPulse.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");
        builder.HasKey(x => x.Id);

        builder.OwnsMany(x => x.Capabilities, capabilities =>
        {
            capabilities.ToJson();
        });

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ExternalId, x.TenantId }).IsUnique();
    }
}