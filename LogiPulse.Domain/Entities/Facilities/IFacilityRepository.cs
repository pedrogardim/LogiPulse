namespace LogiPulse.Domain.Entities.Facilities;

public interface IFacilityRepository
{
    public Task<IReadOnlyList<Facility>> ListAsync(
        FacilityType? facilityType,
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    );

    public Task AddAsync(Facility facility, CancellationToken cancellationToken);

    public Task<bool> ExistsByTenantIdAndExternalIdAsync(
        Guid tenantId,
        string externalId,
        CancellationToken cancellationToken);

    public Task<bool> ExistsByTenantIdAndCodeAsync(
        Guid tenantId,
        string code,
        CancellationToken cancellationToken);
}