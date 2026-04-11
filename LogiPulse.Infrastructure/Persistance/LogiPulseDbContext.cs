using Microsoft.EntityFrameworkCore;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Products;

namespace LogiPulse.Infrastructure.Persistance;

using LogiPulse.Domain.Entities;
public class LogiPulseDbContext(DbContextOptions<LogiPulseDbContext> options) : DbContext(options)
{
    public virtual DbSet<Dispatch> Dispatches { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductCategory> ProductCategories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogiPulseDbContext).Assembly);
    }
}