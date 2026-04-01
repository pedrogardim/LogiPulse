using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.Persistance;

using LogiPulse.Domain.Entities;
public class LogiPulseDbContext(DbContextOptions<LogiPulseDbContext> options) : DbContext(options)
{
    public virtual DbSet<Order> Order { get; set; }
    public virtual DbSet<CargoType> CargoType { get; set; }
}