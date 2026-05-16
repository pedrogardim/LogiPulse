using FluentValidation;
using LogiPulse.Domain.Entities.Vehicles;

namespace LogiPulse.Application.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("ExternalId is mandatory");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is mandatory");

        RuleFor(x => x.LicensePlate)
            .NotEmpty()
            .WithMessage("LicensePlate is mandatory");

        RuleFor(x => x.VehicleType)
            .IsInEnum()
            .NotEqual(VehicleType.Unknown)
            .WithMessage("VehicleType is invalid");
    }
}