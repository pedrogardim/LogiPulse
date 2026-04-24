namespace LogiPulse.Domain.Entities.Tenants;

public interface ITenantRepository
{
    public Task AddAsync(Tenant tenant);
}