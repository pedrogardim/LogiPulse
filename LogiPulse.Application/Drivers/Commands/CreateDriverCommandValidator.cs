using FluentValidation;

namespace LogiPulse.Application.Drivers.Commands;

public class CreateDriverCommandValidator : AbstractValidator<CreateDriverCommand>
{
    public CreateDriverCommandValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("ExternalId is mandatory");

        RuleFor(x => x.UserId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("UserId must be null or a non-empty Guid");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is mandatory");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone is mandatory");

        RuleFor(x => x.LicenseNumber)
            .NotEmpty()
            .WithMessage("LicenseNumber is mandatory");

        RuleFor(x => x.LicenseExpiryDate)
            .NotEmpty()
            .WithMessage("LicenseExpiryDate is mandatory")
            .Must(x => x > DateOnly.FromDateTime(DateTime.UtcNow.Date))
            .WithMessage("Driver License has expired");
    }
}