using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Shared;

namespace LogiPulse.Infrastructure.Persistance.Configurations;

public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
{
    public void Configure(EntityTypeBuilder<Facility> builder)
    {
        builder.ToTable("facilities");
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Address, a =>
        {
            a.Property(p => p.Street).HasColumnName("address_street");
            a.Property(p => p.Number).HasColumnName("address_number");
            a.Property(p => p.Complement).HasColumnName("address_complement");
            a.Property(p => p.City).HasColumnName("address_city");
            a.Property(p => p.State).HasColumnName("address_state");
            a.Property(p => p.ZipCode).HasColumnName("address_zip_code");
            a.Property(p => p.Country).HasColumnName("address_country");
        });
        
        builder.Property(x => x.Location)
            .HasColumnType("geography(Point, 4326)");
        
        builder.HasIndex(x => new { x.ExternalId, x.TenantId }).IsUnique();
        builder.HasIndex(x => new { x.Code, x.TenantId }).IsUnique();
        
        builder.HasOne<Tenant>()
            .WithMany(t => t.Facilities)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}