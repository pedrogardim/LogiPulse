namespace LogiPulse.Domain.Entities.Vehicles;

public interface IVehicleRepository
{
    public Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken);

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
    
    public Task<bool> ExistsByTenantIdAndExternalIdAsync(
        Guid tenantId,
        string externalId,
        CancellationToken cancellationToken);

    public Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public void Remove(Vehicle vehicle);
}