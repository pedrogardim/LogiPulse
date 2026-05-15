namespace LogiPulse.Domain.Entities.Drivers;

public interface IDriverRepository
{
    public Task AddAsync(Driver driver, CancellationToken cancellationToken);

    public Task<bool> ExistsByTenantIdAndUserIdAndExternalIdAsync(
        Guid tenantId,
        Guid? userId,
        string externalId,
        CancellationToken cancellationToken);
}