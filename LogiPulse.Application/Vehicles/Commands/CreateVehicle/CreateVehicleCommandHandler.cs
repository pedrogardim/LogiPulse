using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler(
    IVehicleRepository vehicleRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateVehicleCommand, Guid>
{
    public async Task<Guid> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var exists = await vehicleRepository.ExistsByTenantIdAndExternalIdAsync(
            tenantId,
            request.ExternalId,
            cancellationToken
        );

        if (exists)
            throw new ConflictException("Vehicle already exists");

        var vehicle = Vehicle.Create(
            tenantId,
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