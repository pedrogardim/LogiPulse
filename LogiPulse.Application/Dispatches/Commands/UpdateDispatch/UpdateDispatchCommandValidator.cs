using FluentValidation;

namespace LogiPulse.Application.Dispatches.Commands.UpdateDispatch;

public class UpdateDispatchCommandValidator : AbstractValidator<UpdateDispatchCommand>
{
    public UpdateDispatchCommandValidator()
    {
        throw new NotImplementedException("UpdateDispatchCommandValidator not implemented");

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is mandatory");

        RuleFor(x => x)
            .Must(HaveAtLeastOneChange)
            .WithMessage("At least one field must be informed for update.");
    }

    private static bool HaveAtLeastOneChange(UpdateDispatchCommand command)
    {
        return false;
    }
}