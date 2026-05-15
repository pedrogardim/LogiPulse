using FluentValidation;
using LogiPulse.Domain.Entities.Vehicles;

namespace LogiPulse.Application.Vehicles.Commands;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotNull()
            .NotEqual(Guid.Empty)
            .WithMessage("TenantId is mandatory");

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