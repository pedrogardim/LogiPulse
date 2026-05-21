using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Facilities.Commands.DeleteFacility;

public class DeleteFacilityCommandHandler(
    IFacilityRepository facilityRepository,
    IUserContext userContext,
    IUnitOfWork uow
) : IRequestHandler<DeleteFacilityCommand>
{
    public async Task Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var facility = await facilityRepository.GetByIdAsync(request.Id, cancellationToken);

        if (facility is null)
            throw new NotFoundException("Facility don't exist");

        facilityRepository.Remove(facility);

        await uow.CommitAsync(cancellationToken);
    }
}