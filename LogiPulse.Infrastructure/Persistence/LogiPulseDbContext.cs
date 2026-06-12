using LogiPulse.Application.Common;
using Microsoft.EntityFrameworkCore;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Entities.Vehicles;

namespace LogiPulse.Infrastructure.Persistence;

public class LogiPulseDbContext(DbContextOptions<LogiPulseDbContext> options, IUserContext userContext)
    : DbContext(options)
{
    public virtual DbSet<Dispatch> Dispatches { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductCategory> ProductCategories { get; set; }
    public virtual DbSet<Tenant> Tenants { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Facility> Facilities { get; set; }
    public virtual DbSet<Vehicle> Vehicles { get; set; }
    public virtual DbSet<Driver> Drivers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Repository methods are tenant-scoped by default.
        // Any method that bypasses tenant isolation must use the suffix WithoutTenantFilter.

        base.OnModelCreating(modelBuilder);

        if (!userContext.BypassTenantFilter)
        {
            modelBuilder.Entity<Dispatch>()
                .HasQueryFilter(d => d.TenantId == userContext.TenantId);

            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => p.TenantId == userContext.TenantId);

            modelBuilder.Entity<ProductCategory>()
                .HasQueryFilter(pc => pc.TenantId == userContext.TenantId);

            modelBuilder.Entity<Tenant>()
                .HasQueryFilter(t => t.Id == userContext.TenantId);

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => u.TenantId == userContext.TenantId);

            modelBuilder.Entity<Facility>()
                .HasQueryFilter(f => f.TenantId == userContext.TenantId);

            modelBuilder.Entity<Vehicle>()
                .HasQueryFilter(v => v.TenantId == userContext.TenantId);

            modelBuilder.Entity<Driver>()
                .HasQueryFilter(d => d.TenantId == userContext.TenantId);
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogiPulseDbContext).Assembly);
    }
}