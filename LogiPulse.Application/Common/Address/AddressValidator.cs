using FluentValidation;

namespace LogiPulse.Application.Common.Address;

public class AddressValidator : AbstractValidator<Domain.Shared.Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Number)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Complement)
            .MaximumLength(100);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.State)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);
    }
}