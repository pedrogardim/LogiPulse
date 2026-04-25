using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Vehicles;

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

        builder.HasOne<Tenant>()
            .WithMany(t => t.Dispatches)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Vehicle>()
            .WithMany(t => t.Dispatches)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.OriginFacility)
            .WithMany()
            .HasForeignKey(x => x.OriginFacilityId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.DestinationFacility)
            .WithMany()
            .HasForeignKey(x => x.DestinationFacilityId)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => x.VehicleId);
        builder.HasIndex(x => x.CurrentStatus);
        builder.HasIndex(x => new { x.ExternalId, x.TenantId }).IsUnique();
    }
}