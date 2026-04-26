using LogiPulse.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.Persistence.Repositories;

public class TenantRepository(LogiPulseDbContext context) : ITenantRepository
{
    public async Task AddAsync(Tenant tenant)
    {
        await context.Tenants.AddAsync(tenant);
    }
    
    public async Task<bool> ExistsByTaxCodeAsync(string taxCode)
    {
        return await context.Tenants.AnyAsync(t => t.TaxCode == taxCode);
    }
}