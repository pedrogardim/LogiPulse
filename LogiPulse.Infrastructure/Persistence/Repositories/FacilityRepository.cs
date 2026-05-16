using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.Persistence.Repositories;

public class FacilityRepository(LogiPulseDbContext context) : IFacilityRepository
{
    public async Task<IReadOnlyList<Facility>> ListAsync(
        FacilityType? facilityType,
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        var query = context.Facilities.AsNoTracking();

        if (facilityType != null && facilityType != FacilityType.Unknown)
            query = query.Where(f => f.Type == facilityType);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(f =>
                f.Name.Contains(search) || f.Code.Contains(search) || f.ExternalId.Contains(search));

        var skipCount = (page - 1) * pageSize;
        query = query.Skip(skipCount).Take(pageSize);

        return await query.ToListAsync(cancellationToken);
    }

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