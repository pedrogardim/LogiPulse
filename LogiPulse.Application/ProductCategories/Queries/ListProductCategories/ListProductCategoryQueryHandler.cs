using LogiPulse.Domain.Entities.Products;
using MediatR;

namespace LogiPulse.Application.ProductCategories.Queries.ListProductCategories;

public class ListProductCategoriesQueryHandler(IProductCategoryRepository productCategoryRepository)
    : IRequestHandler<ListProductCategoriesQuery, IReadOnlyList<ListProductCategoriesItemResponse>>
{
    public async Task<IReadOnlyList<ListProductCategoriesItemResponse>> Handle(ListProductCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var facilities =
            await productCategoryRepository.ListAsync(
                query.Search,
                query.Page,
                query.PageSize,
                cancellationToken);

        return facilities.Select(f => new ListProductCategoriesItemResponse(
            f.Id,
            f.ExternalId,
            f.Name
        )).ToList();
    }
}