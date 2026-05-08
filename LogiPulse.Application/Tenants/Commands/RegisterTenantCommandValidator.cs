using FluentValidation;

namespace LogiPulse.Application.Tenants.Commands;

public class RegisterTenantCommandValidator : AbstractValidator<RegisterTenantCommand>
{
    public RegisterTenantCommandValidator()
    {
        RuleFor(x => x.TaxCode)
            .NotEmpty()
            .WithMessage("TaxCode is mandatory");

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .WithMessage("DisplayName is mandatory");

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