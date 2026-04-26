using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Tenants;

namespace LogiPulse.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.ExternalId, x.TenantId }).IsUnique();
        builder.HasIndex(x => new { x.Code, x.TenantId }).IsUnique();

        builder.OwnsMany(x => x.Requirements, requirements =>
        {
            requirements.ToJson();
        });

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}