using LogiPulse.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.Persistence.Repositories;

public class VehicleRepository(LogiPulseDbContext context) : IVehicleRepository
{
    public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken)
    {
        await context.Vehicles.AddAsync(vehicle, cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Vehicles.AnyAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByTenantIdAndExternalIdAsync(
        Guid tenantId,
        string externalId,
        CancellationToken cancellationToken = default)
    {
        return await context.Vehicles.AnyAsync(d =>
                d.TenantId == tenantId &&
                d.ExternalId == externalId,
            cancellationToken);
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Vehicles.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public void Remove(Vehicle vehicle)
    {
        context.Vehicles.Remove(vehicle);
    }
}