using MediatR;

namespace LogiPulse.Application.Products.Queries.ListProducts;

public record ListProductsQuery(
    Guid? ProductCategoryId,
    string? Search = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<IReadOnlyList<ListProductsItemResponse>>;