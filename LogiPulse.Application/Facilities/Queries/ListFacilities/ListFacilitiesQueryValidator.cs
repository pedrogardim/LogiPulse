using FluentValidation;
using LogiPulse.Domain.Entities.Facilities;

namespace LogiPulse.Application.Facilities.Queries.ListFacilities;

public class ListFacilitiesQueryValidator : AbstractValidator<ListFacilitiesQuery>
{
    public ListFacilitiesQueryValidator()
    {
        RuleFor(x => x.FacilityType)
            .IsInEnum()
            .NotEqual(FacilityType.Unknown)
            .When(x => x.FacilityType.HasValue)
            .WithMessage("FacilityType is invalid");

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