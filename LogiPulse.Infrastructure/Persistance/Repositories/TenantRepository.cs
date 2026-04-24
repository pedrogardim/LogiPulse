using LogiPulse.Domain.Entities.Tenants;

namespace LogiPulse.Infrastructure.Persistance.Repositories;

public class TenantRepository(LogiPulseDbContext context) : ITenantRepository
{
    public async Task AddAsync(Tenant tenant)
    {
        await context.Tenants.AddAsync(tenant);
    }
}