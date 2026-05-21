using LogiPulse.Domain.Entities.Products;

namespace LogiPulse.Application.Products.Queries.ListProducts;

public record ListProductsItemResponse(
    Guid Id,
    string ExternalId,
    string Name,
    string Code,
    ProductCategory? ProductCategory
);