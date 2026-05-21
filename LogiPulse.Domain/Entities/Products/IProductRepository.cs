namespace LogiPulse.Domain.Entities.Products;

public interface IProductRepository
{
    public Task<IReadOnlyList<Product>> ListAsync(
        Guid? productCategoryId,
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    public Task AddAsync(Product facility, CancellationToken cancellationToken);

    public Task<bool> ExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken);

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken);

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public void Remove(Product facility);
}