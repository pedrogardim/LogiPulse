using FluentValidation;
using LogiPulse.Application.Common.Address;
using LogiPulse.Domain.Entities.Facilities;

namespace LogiPulse.Application.Facilities.Commands.UpdateFacility;

public class UpdateFacilityCommandValidator : AbstractValidator<UpdateFacilityCommand>
{
    public UpdateFacilityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is mandatory");

        RuleFor(x => x)
            .Must(HaveAtLeastOneChange)
            .WithMessage("At least one field must be informed for update.");

        RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .When(x => x.Name is not null)
            .WithMessage("Name cannot be empty.")
            .MaximumLength(200)
            .WithMessage("Name should have at most 200 characters");

        RuleFor(x => x.Code)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .When(x => x.Code is not null)
            .WithMessage("Code cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Code should have at most 50 characters");

        RuleFor(x => x.FacilityType)
            .IsInEnum()
            .NotEqual(FacilityType.Unknown)
            .When(x => x.FacilityType.HasValue)
            .WithMessage("FacilityType is invalid");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .When(x => x.Latitude.HasValue)
            .WithMessage("Latitude should be between -90 and 90");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .When(x => x.Longitude.HasValue)
            .WithMessage("Longitude should be between -180 and 180");


        When(x => x.Address is not null, () =>
        {
            RuleFor(x => x.Address!)
                .SetValidator(new AddressValidator());
        });
    }

    private static bool HaveAtLeastOneChange(UpdateFacilityCommand command)
    {
        return command.Name is not null ||
               command.Code is not null ||
               command.FacilityType.HasValue ||
               command.Latitude.HasValue ||
               command.Longitude.HasValue ||
               command.Address is not null;
    }
}