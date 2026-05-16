using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Exceptions;
using MediatR;
using NetTopologySuite.Geometries;

namespace LogiPulse.Application.Facilities.Commands.CreateFacility;

public class CreateFacilityCommandHandler(
    IFacilityRepository facilityRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateFacilityCommand, Guid>
{
    public async Task<Guid> Handle(CreateFacilityCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        var existsByTenantId = await facilityRepository.ExistsByTenantIdAndExternalIdAsync(
            tenantId,
            request.ExternalId,
            cancellationToken
        );
        if (existsByTenantId)
            throw new ConflictException("A facility with that external id already exists");


        var existsByCode = await facilityRepository.ExistsByTenantIdAndCodeAsync(
            tenantId,
            request.Code,
            cancellationToken
        );

        if (existsByCode)
            throw new ConflictException("A facility with that code already exists");

        var point = new Point(request.Longitude, request.Latitude) { SRID = 4326 };

        var facility = Facility.Create(
            tenantId,
            request.ExternalId,
            request.Name,
            request.Code,
            request.FacilityType,
            request.Address,
            point
        );

        await facilityRepository.AddAsync(facility, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return facility.Id;
    }
}