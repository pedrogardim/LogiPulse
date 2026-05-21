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

    public async Task<bool> ExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await context.Facilities.AnyAsync(d => d.ExternalId == externalId, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await context.Facilities.AnyAsync(d => d.Code == code, cancellationToken);
    }

    public async Task<Facility?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Facilities.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public void Remove(Facility facility)
    {
        context.Facilities.Remove(facility);
    }
}