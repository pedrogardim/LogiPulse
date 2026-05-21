using LogiPulse.Domain.Entities.Drivers;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.Persistence.Repositories;

public class DriverRepository(LogiPulseDbContext context) : IDriverRepository
{
    public async Task AddAsync(Driver driver, CancellationToken cancellationToken)
    {
        await context.Drivers.AddAsync(driver, cancellationToken);
    }

    public async Task<bool> ExistsByTenantIdAndUserIdAndExternalIdAsync(
        Guid tenantId,
        Guid? userId,
        string externalId,
        CancellationToken cancellationToken = default)
    {
        return await context.Drivers.AnyAsync(d =>
                d.TenantId == tenantId &&
                d.UserId == userId &&
                d.ExternalId == externalId,
            cancellationToken);
    }

    public async Task<Driver?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Drivers.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public void Remove(Driver driver)
    {
        context.Drivers.Remove(driver);
    }
}