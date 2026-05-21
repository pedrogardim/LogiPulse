using FluentValidation;

namespace LogiPulse.Application.ProductCategories.Queries.ListProductCategories;

public class ListProductCategoriesQueryValidator : AbstractValidator<ListProductCategoriesQuery>
{
    public ListProductCategoriesQueryValidator()
    {
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