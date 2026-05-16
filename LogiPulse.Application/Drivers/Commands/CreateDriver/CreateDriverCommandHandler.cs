using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Drivers.Commands.CreateDriver;

public class CreateDriverCommandHandler(
    IDriverRepository driverRepository,
    IUserRepository userRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateDriverCommand, Guid>
{
    public async Task<Guid> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var exists = await driverRepository.ExistsByTenantIdAndUserIdAndExternalIdAsync(
            tenantId,
            request.UserId,
            request.ExternalId,
            cancellationToken
        );

        if (exists)
            throw new ConflictException("Driver already exists");

        if (request.UserId.HasValue)
        {
            var userExists = await userRepository.ExistsByIdAsync(request.UserId.Value);
            if (!userExists)
                throw new BusinessRuleException("Given user doesn't exist");
        }

        var driver = Driver.Create(
            tenantId,
            request.UserId,
            request.ExternalId,
            request.Name,
            request.Phone,
            request.LicenseNumber,
            request.LicenseExpiryDate
        );

        await driverRepository.AddAsync(driver, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return driver.Id;
    }
}