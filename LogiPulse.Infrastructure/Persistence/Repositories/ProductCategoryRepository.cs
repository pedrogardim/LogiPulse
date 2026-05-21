using LogiPulse.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.Persistence.Repositories;

public class ProductCategoryRepository(LogiPulseDbContext context) : IProductCategoryRepository
{
    public async Task<IReadOnlyList<ProductCategory>> ListAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        var query = context.ProductCategories.AsNoTracking();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(f =>
                f.Name.Contains(search) || f.ExternalId.Contains(search));

        var skipCount = (page - 1) * pageSize;
        query = query.Skip(skipCount).Take(pageSize);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ProductCategory productCategory, CancellationToken cancellationToken)
    {
        await context.ProductCategories.AddAsync(productCategory, cancellationToken);
    }

    public async Task<bool> ExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await context.ProductCategories.AnyAsync(d => d.ExternalId == externalId, cancellationToken);
    }

    public async Task<ProductCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ProductCategories.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public void Remove(ProductCategory productCategory)
    {
        context.ProductCategories.Remove(productCategory);
    }
}