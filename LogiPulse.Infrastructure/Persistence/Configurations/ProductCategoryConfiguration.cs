using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Tenants;

namespace LogiPulse.Infrastructure.Persistence.Configurations;

public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("product_categories");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.ExternalId, x.TenantId }).IsUnique();

        builder.OwnsMany(x => x.Requirements, requirements =>
        {
            requirements.ToJson();
        });
        
        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}