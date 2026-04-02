using Microsoft.EntityFrameworkCore;
using LogiPulse.Domain.Entities.Dispatch;
using LogiPulse.Domain.Entities.CargoType;

namespace LogiPulse.Infrastructure.Persistance;

using LogiPulse.Domain.Entities;
public class LogiPulseDbContext(DbContextOptions<LogiPulseDbContext> options) : DbContext(options)
{
    public virtual DbSet<Dispatch> Dispatch { get; set; }
    public virtual DbSet<CargoType> CargoType { get; set; }
}