using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogiPulse.Domain.Entities.Product;

namespace LogiPulse.Infrastructure.Persistance.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.ExternalId, x.TenantId }).IsUnique();
        builder.HasIndex(x => new { x.Code, x.TenantId }).IsUnique();

        builder.OwnsMany(x => x.Rules, rules =>
        {
            rules.ToJson();
        });

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}