using FluentValidation;

namespace LogiPulse.Application.ProductCategories.Commands.CreateProductCategory;

public class CreateProductCategoryCommandValidator : AbstractValidator<CreateProductCategoryCommand>
{
    public CreateProductCategoryCommandValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("ExternalId is mandatory")
            .MaximumLength(100)
            .WithMessage("ExternalId should have at most 100 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is mandatory")
            .MaximumLength(200)
            .WithMessage("Name should have at most 200 characters");
    }
}