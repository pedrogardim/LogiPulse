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
            .WithMessage("ExternalId is mandatory")
            .MaximumLength(100)
            .WithMessage("ExternalId should have at most 100 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is mandatory")
            .MaximumLength(200)
            .WithMessage("Name should have at most 200 characters");


        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is mandatory")
            .MaximumLength(50)
            .WithMessage("Code should have at most 50 characters");

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
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude should be between -180 and 180");

        RuleFor(x => x.Address)
            .NotNull()
            .SetValidator(new AddressValidator());
    }
}