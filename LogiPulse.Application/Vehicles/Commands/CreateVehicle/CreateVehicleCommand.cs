using LogiPulse.Domain.Entities.Vehicles;
using MediatR;

namespace LogiPulse.Application.Vehicles.Commands.CreateVehicle;

public record CreateVehicleCommand : IRequest<Guid>
{
    public string ExternalId;
    public string Name;
    public string LicensePlate;
    public VehicleType VehicleType;
}