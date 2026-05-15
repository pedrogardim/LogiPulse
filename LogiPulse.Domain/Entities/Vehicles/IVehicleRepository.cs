namespace LogiPulse.Domain.Entities.Vehicles;

public interface IVehicleRepository
{
    public Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken);

    public Task<bool> ExistsByTenantIdAndExternalIdAsync(
        Guid tenantId,
        string externalId,
        CancellationToken cancellationToken);
}