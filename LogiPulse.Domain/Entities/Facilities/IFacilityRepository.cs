namespace LogiPulse.Domain.Entities.Facilities;

public interface IFacilityRepository
{
    public Task<IReadOnlyList<Facility>> ListAsync(
        FacilityType? facilityType,
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    public Task AddAsync(Facility facility, CancellationToken cancellationToken);

    public Task<bool> ExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken);

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken);

    public Task<Facility?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}