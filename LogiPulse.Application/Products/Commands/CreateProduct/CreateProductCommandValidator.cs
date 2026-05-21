using FluentValidation;

namespace LogiPulse.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
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


        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is mandatory")
            .MaximumLength(50)
            .WithMessage("Code should have at most 50 characters");

        RuleFor(x => x.CategoryId)
            .Must(x => x != Guid.Empty)
            .When(x => x.CategoryId is not null)
            .WithMessage("CategoryId cannot be empty.");
    }
}