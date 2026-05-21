using LogiPulse.Domain.Entities.Dispatches;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.Persistence.Repositories;

public class DispatchRepository(LogiPulseDbContext context) : IDispatchRepository
{
    public async Task<IReadOnlyList<Dispatch>> ListAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        var query = context.Dispatches.AsNoTracking();

        if (!string.IsNullOrEmpty(search))
            query =
                query.Where(d => d.ExternalId.Contains(search));

        var skipCount = (page - 1) * pageSize;
        query = query.Skip(skipCount).Take(pageSize);

        return await query
            .Include(d => d.Product)
            .Include(d => d.Vehicle)
            .Include(d => d.Driver)
            .Include(d => d.OriginFacility)
            .Include(d => d.DestinationFacility)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Dispatch dispatch, CancellationToken cancellationToken)
    {
        await context.Dispatches.AddAsync(dispatch, cancellationToken);
    }

    public async Task<bool> ExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await context.Dispatches.AnyAsync(d => d.ExternalId == externalId, cancellationToken);
    }

    public async Task<Dispatch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Dispatches.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public void Remove(Dispatch dispatch)
    {
        context.Dispatches.Remove(dispatch);
    }
}