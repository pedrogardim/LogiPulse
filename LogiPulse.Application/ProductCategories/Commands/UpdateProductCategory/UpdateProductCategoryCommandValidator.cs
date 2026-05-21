using FluentValidation;

namespace LogiPulse.Application.ProductCategories.Commands.UpdateProductCategory;

public class UpdateProductCategoryCommandValidator : AbstractValidator<UpdateProductCategoryCommand>
{
    public UpdateProductCategoryCommandValidator()
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
    }

    private static bool HaveAtLeastOneChange(UpdateProductCategoryCommand command)
    {
        return command.Name is not null;
    }
}