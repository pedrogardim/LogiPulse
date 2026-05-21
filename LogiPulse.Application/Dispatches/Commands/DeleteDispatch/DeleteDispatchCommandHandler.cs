using MediatR;

namespace LogiPulse.Application.Dispatches.Commands.DeleteDispatch;

public class DeleteDispatchCommandHandler : IRequestHandler<DeleteDispatchCommand>
{
    public async Task Handle(DeleteDispatchCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("DeleteDispatchCommand not implemented");
    }
}