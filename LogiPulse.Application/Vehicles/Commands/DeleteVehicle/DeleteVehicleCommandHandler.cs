using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler(
    IVehicleRepository vehicleRepository,
    IUserContext userContext,
    IUnitOfWork uow
) : IRequestHandler<DeleteVehicleCommand>
{
    public async Task Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var vehicle = await vehicleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (vehicle is null)
            throw new NotFoundException("Vehicle don't exist");

        vehicleRepository.Remove(vehicle);

        await uow.CommitAsync(cancellationToken);
    }
}