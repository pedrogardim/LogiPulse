using LogiPulse.Domain.Entities.Dispatches;
using MediatR;

namespace LogiPulse.Application.Dispatches.Commands.UpdateDispatch;

public class UpdateDispatchCommandHandler : IRequestHandler<UpdateDispatchCommand, Dispatch>
{
    public async Task<Dispatch> Handle(UpdateDispatchCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("UpdateDispatchCommand not implemented");
    }
}