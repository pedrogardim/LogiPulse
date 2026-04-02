using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogiPulse.Domain.Entities.Product;

namespace LogiPulse.Infrastructure.Persistance.Configurations;

public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("product_categories");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.ExternalId, x.TenantId }).IsUnique();

        builder.OwnsMany(x => x.Rules, rules =>
        {
            rules.ToJson();
        });
    }
}