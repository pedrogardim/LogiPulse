using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Exceptions;
using MediatR;
using NetTopologySuite.Geometries;

namespace LogiPulse.Application.Facilities.Commands.UpdateFacility;

public class UpdateFacilityCommandHandler(
    IFacilityRepository facilityRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateFacilityCommand, Facility>
{
    public async Task<Facility> Handle(UpdateFacilityCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var facility = await facilityRepository.GetByIdAsync(request.Id, cancellationToken);
        if (facility is null)
            throw new NotFoundException("Facility not found");

        facility.Update(
            request.Name,
            request.Code,
            request.FacilityType,
            request.Latitude,
            request.Longitude,
            request.Address);

        await unitOfWork.CommitAsync(cancellationToken);

        return facility;
    }
}