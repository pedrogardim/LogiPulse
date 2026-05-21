using MediatR;

namespace LogiPulse.Application.Drivers.Commands.DeleteDriver;

public record DeleteDriverCommand(Guid Id) : IRequest;