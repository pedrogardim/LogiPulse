using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Vehicles.Commands;

public class CreateVehicleCommandHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateVehicleCommand, Guid>
{
    public async Task<Guid> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var exists = await vehicleRepository.ExistsByTenantIdAndExternalIdAsync(
            request.TenantId,
            request.ExternalId,
            cancellationToken
        );

        if (exists)
            throw new ConflictException("Vehicle already exists");

        var vehicle = Vehicle.Create(
            request.TenantId,
            request.ExternalId,
            request.Name,
            request.LicensePlate,
            request.VehicleType
        );

        await vehicleRepository.AddAsync(vehicle, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return vehicle.Id;
    }
}