using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Entities.Vehicles;

namespace LogiPulse.Infrastructure.Persistence.Configurations;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("drivers");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.ExternalId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.LicenseNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.LicenseExpiryDate)
            .IsRequired();

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.IsActive)
            .IsRequired();
        
        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<Driver>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(x => new { x.ExternalId, x.TenantId }).IsUnique();

        builder.HasIndex(x => new { x.TenantId, x.LicenseNumber });
    }
}