using FluentValidation;
using LogiPulse.Application.Common.Address;
using LogiPulse.Domain.Entities.Facilities;

namespace LogiPulse.Application.Facilities.Commands.CreateFacility;

public class CreateFacilityCommandValidator : AbstractValidator<CreateFacilityCommand>
{
    public CreateFacilityCommandValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("ExternalId is mandatory");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is mandatory");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is mandatory");

        RuleFor(x => x.FacilityType)
            .IsInEnum()
            .NotEqual(FacilityType.Unknown)
            .WithMessage("FacilityType is invalid");

        RuleFor(x => x.Latitude)
            .NotNull()
            .WithMessage("Latitude is mandatory")
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude should be between -90 and 90");

        RuleFor(x => x.Longitude)
            .NotNull()
            .WithMessage("Longitude is mandatory")
            .InclusiveBetween(-90, 90)
            .WithMessage("Longitude should be between -90 and 90");

        RuleFor(x => x.Address)
            .NotNull()
            .SetValidator(new AddressValidator());
    }
}