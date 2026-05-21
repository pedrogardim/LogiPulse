using FluentValidation;

namespace LogiPulse.Application.Products.Queries.ListProducts;

public class ListProductsQueryValidator : AbstractValidator<ListProductsQuery>
{
    public ListProductsQueryValidator()
    {
        RuleFor(x => x.ProductCategoryId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("ProductCategoryId must be null or a non-empty Guid");

        RuleFor(x => x.Search)
            .MaximumLength(200)
            .When(x => x.Search is not null)
            .WithMessage("Search should have at most 200 characters");

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page should be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize should be between 1 and 50");
    }
}