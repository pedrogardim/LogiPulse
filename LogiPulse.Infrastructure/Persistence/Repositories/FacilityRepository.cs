using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.Persistence.Repositories;

public class FacilityRepository(LogiPulseDbContext context) : IFacilityRepository
{
    public async Task AddAsync(Facility facility, CancellationToken cancellationToken)
    {
        await context.Facilities.AddAsync(facility, cancellationToken);
    }

    public async Task<bool> ExistsByTenantIdAndExternalIdAsync(
        Guid tenantId,
        string externalId,
        CancellationToken cancellationToken = default)
    {
        return await context.Facilities.AnyAsync(d =>
                d.TenantId == tenantId &&
                d.ExternalId == externalId,
            cancellationToken);
    }

    public async Task<bool> ExistsByTenantIdAndCodeAsync(
        Guid tenantId,
        string code,
        CancellationToken cancellationToken = default)
    {
        return await context.Facilities.AnyAsync(d =>
                d.TenantId == tenantId &&
                d.Code == code,
            cancellationToken);
    }
}