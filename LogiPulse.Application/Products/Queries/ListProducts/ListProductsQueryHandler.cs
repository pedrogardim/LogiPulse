using LogiPulse.Domain.Entities.Products;
using MediatR;

namespace LogiPulse.Application.Products.Queries.ListProducts;

public class ListProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<ListProductsQuery, IReadOnlyList<ListProductsItemResponse>>
{
    public async Task<IReadOnlyList<ListProductsItemResponse>> Handle(ListProductsQuery query,
        CancellationToken cancellationToken)
    {
        var facilities =
            await productRepository.ListAsync(
                query.ProductCategoryId,
                query.Search,
                query.Page,
                query.PageSize,
                cancellationToken);

        return facilities.Select(p => new ListProductsItemResponse(
            p.Id,
            p.ExternalId,
            p.Name,
            p.Code,
            p.Category
        )).ToList();
    }
}