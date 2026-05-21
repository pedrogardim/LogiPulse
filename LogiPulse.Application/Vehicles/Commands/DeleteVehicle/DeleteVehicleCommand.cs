using MediatR;

namespace LogiPulse.Application.Vehicles.Commands.DeleteVehicle;

public record DeleteVehicleCommand(Guid Id) : IRequest;