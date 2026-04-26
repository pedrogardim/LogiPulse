using Microsoft.EntityFrameworkCore;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Entities.Vehicles;

namespace LogiPulse.Infrastructure.Persistence;

public class LogiPulseDbContext(DbContextOptions<LogiPulseDbContext> options) : DbContext(options)
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
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogiPulseDbContext).Assembly);
    }
}