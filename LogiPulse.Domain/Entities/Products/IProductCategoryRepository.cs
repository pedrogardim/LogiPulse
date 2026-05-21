namespace LogiPulse.Domain.Entities.Products;

public interface IProductCategoryRepository
{
    public Task<IReadOnlyList<ProductCategory>> ListAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    public Task AddAsync(ProductCategory productCategory, CancellationToken cancellationToken);

    public Task<bool> ExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken);

    public Task<ProductCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public void Remove(ProductCategory productCategory);
}