using FluentValidation;

namespace LogiPulse.Application.Dispatches.Commands.CreateDispatch;

public class CreateDispatchCommandValidator : AbstractValidator<CreateDispatchCommand>
{
    public CreateDispatchCommandValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("ExternalId is mandatory")
            .MaximumLength(100)
            .WithMessage("ExternalId should have at most 100 characters");

        RuleFor(x => x.ProductId)
            .Must(id => id != Guid.Empty)
            .WithMessage("ProductId must be null or a non-empty Guid");

        RuleFor(x => x.OriginFacilityId)
            .Must(x => x != Guid.Empty)
            .WithMessage("OriginFacilityId cannot be empty.");

        RuleFor(x => x.OriginFacilityId)
            .Must(x => x != Guid.Empty)
            .WithMessage("OriginFacilityId cannot be empty.");

        RuleFor(x => x.DestinationFacilityId)
            .Must(x => x != Guid.Empty)
            .WithMessage("DestinationFacilityId cannot be empty.");

        RuleFor(x => x.VehicleId)
            .Must(x => x != Guid.Empty)
            .When(x => x.VehicleId is not null)
            .WithMessage("CategoryId cannot be empty.");

        RuleFor(x => x.DriverId)
            .Must(x => x != Guid.Empty)
            .When(x => x.DriverId is not null)
            .WithMessage("CategoryId cannot be empty.");
    }
}