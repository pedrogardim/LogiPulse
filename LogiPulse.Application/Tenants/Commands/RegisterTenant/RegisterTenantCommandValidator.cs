using FluentValidation;

namespace LogiPulse.Application.Tenants.Commands.RegisterTenant;

public class RegisterTenantCommandValidator : AbstractValidator<RegisterTenantCommand>
{
    public RegisterTenantCommandValidator()
    {
        RuleFor(x => x.TaxCode)
            .NotEmpty()
            .WithMessage("TaxCode is mandatory")
            .Length(1, 30)
            .WithMessage("TaxCode length should be between 1 and 30");

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .WithMessage("DisplayName is mandatory")
            .Length(1, 30)
            .WithMessage("DisplayName length should be between 1 and 100");

        RuleFor(x => x.AdminUserEntraId)
            .NotEmpty()
            .NotNull()
            .WithMessage("AdminUserEntraId should be a valid Guid");

        RuleFor(x => x.AdminUserEmail)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("AdminUserEmail should be an valid email");

        RuleFor(x => x.AdminUserName)
            .NotEmpty()
            .WithMessage("AdminUserName is mandatory");
    }
}