using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Tenants;

namespace LogiPulse.Infrastructure.Persistance.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.TaxCode)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.LegalName)
            .HasMaxLength(200);
        
        builder.HasIndex(x => x.TaxCode)
            .IsUnique();
    }
}