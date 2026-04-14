using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Users;

namespace LogiPulse.Infrastructure.Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
        
        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.Users)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(x => x.EntraId).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}