using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Dispatches.Commands.CreateDispatch;

public class CreateDispatchCommandHandler(
    IDispatchRepository dispatchRepository,
    IProductRepository productRepository,
    IFacilityRepository facilityRepository,
    IVehicleRepository vehicleRepository,
    IDriverRepository driverRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateDispatchCommand, Guid>
{
    public async Task<Guid> Handle(CreateDispatchCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var existsByTenantId = await dispatchRepository.ExistsByExternalIdAsync(request.ExternalId, cancellationToken);
        if (existsByTenantId)
            throw new ConflictException("A dispatch with that external id already exists");

        var productExists = await productRepository.ExistsByIdAsync(request.ProductId, cancellationToken);
        if (!productExists)
            throw new BusinessRuleException("Product don't exist");

        var originExists = await facilityRepository.ExistsByIdAsync(request.OriginFacilityId, cancellationToken);
        if (!originExists)
            throw new BusinessRuleException("Origin facility don't exist");

        var destinationExists =
            await facilityRepository.ExistsByIdAsync(request.DestinationFacilityId, cancellationToken);
        if (!destinationExists)
            throw new BusinessRuleException("Destination facility don't exist");

        if (request.VehicleId != null)
        {
            var vehicleExists = await vehicleRepository.ExistsByIdAsync(request.VehicleId.Value, cancellationToken);
            if (!vehicleExists)
                throw new BusinessRuleException("Vehicle don't exist");
        }

        if (request.DriverId != null)
        {
            var driverExists = await driverRepository.ExistsByIdAsync(request.DriverId.Value, cancellationToken);
            if (!driverExists)
                throw new BusinessRuleException("Driver don't exist");
        }


        var dispatch = Dispatch.Create(
            tenantId,
            request.ExternalId,
            request.ProductId,
            request.OriginFacilityId,
            request.DestinationFacilityId
        );

        if (request.DriverId != null)
            dispatch.AssignDriver(request.DriverId.Value);

        if (request.VehicleId != null)
            dispatch.AssignVehicle(request.VehicleId.Value);

        await dispatchRepository.AddAsync(dispatch, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return dispatch.Id;
    }
}