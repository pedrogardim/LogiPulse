using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Drivers.Commands.DeleteDriver;

public class DeleteDriverCommandHandler(
    IDriverRepository driverRepository,
    IUserContext userContext,
    IUnitOfWork uow
) : IRequestHandler<DeleteDriverCommand>
{
    public async Task Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var driver = await driverRepository.GetByIdAsync(request.Id, cancellationToken);

        if (driver is null)
            throw new NotFoundException("Driver don't exist");

        driverRepository.Remove(driver);

        await uow.CommitAsync(cancellationToken);
    }
}