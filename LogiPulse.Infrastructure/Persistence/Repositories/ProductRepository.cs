using LogiPulse.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.Persistence.Repositories;

public class ProductRepository(LogiPulseDbContext context) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> ListAsync(
        Guid? productCategoryId,
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        var query = context.Products.AsNoTracking();

        if (productCategoryId != null && productCategoryId != Guid.Empty)
            query = query.Where(p => p.CategoryId == productCategoryId);

        if (!string.IsNullOrEmpty(search))
            query =
                query.Where(p => p.Name.Contains(search) || p.Code.Contains(search) || p.ExternalId.Contains(search));

        var skipCount = (page - 1) * pageSize;
        query = query.Skip(skipCount).Take(pageSize);

        return await query
            .Include(p => p.Category)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await context.Products.AddAsync(product, cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products.AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await context.Products.AnyAsync(d => d.ExternalId == externalId, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await context.Products.AnyAsync(d => d.Code == code, cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public void Remove(Product product)
    {
        context.Products.Remove(product);
    }
}