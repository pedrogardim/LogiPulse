using MediatR;

namespace LogiPulse.Application.Facilities.Commands.DeleteFacility;

public record DeleteFacilityCommand(Guid Id) : IRequest;