using MediatR;

namespace LogiPulse.Application.ProductCategories.Queries.ListProductCategories;

public record ListProductCategoriesQuery(
    string? Search = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<IReadOnlyList<ListProductCategoriesItemResponse>>;