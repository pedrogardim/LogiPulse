namespace LogiPulse.Domain.Entities.Dispatches;

public interface IDispatchRepository
{
    public Task<IReadOnlyList<Dispatch>> ListAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    public Task AddAsync(Dispatch facility, CancellationToken cancellationToken);

    public Task<bool> ExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken);

    public Task<Dispatch?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public void Remove(Dispatch facility);
}