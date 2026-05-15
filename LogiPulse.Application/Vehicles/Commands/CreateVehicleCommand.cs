using LogiPulse.Domain.Entities.Vehicles;
using MediatR;

namespace LogiPulse.Application.Vehicles.Commands;

public record CreateVehicleCommand : IRequest<Guid>
{
    public Guid TenantId;
    public string ExternalId;
    public string Name;
    public string LicensePlate;
    public VehicleType VehicleType;
}