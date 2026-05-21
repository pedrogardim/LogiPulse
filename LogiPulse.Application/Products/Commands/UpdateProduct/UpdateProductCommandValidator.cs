using FluentValidation;

namespace LogiPulse.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
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

        RuleFor(x => x.ProductCategoryId)
            .Must(x => x != Guid.Empty)
            .When(x => x.ProductCategoryId is not null)
            .WithMessage("ProductCategoryId cannot be empty.");
    }

    private static bool HaveAtLeastOneChange(UpdateProductCommand command)
    {
        return command.Name is not null ||
               command.Code is not null ||
               command.ProductCategoryId.HasValue;
    }
}