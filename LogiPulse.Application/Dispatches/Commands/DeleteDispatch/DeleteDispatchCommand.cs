using MediatR;

namespace LogiPulse.Application.Dispatches.Commands.DeleteDispatch;

public record DeleteDispatchCommand(Guid Id) : IRequest;