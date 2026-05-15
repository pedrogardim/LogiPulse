using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Drivers.Commands;

public class CreateDriverCommandHandler(
    IDriverRepository driverRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateDriverCommand, Guid>
{
    public async Task<Guid> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var exists = await driverRepository.ExistsByTenantIdAndUserIdAndExternalIdAsync(
            request.TenantId,
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
            request.TenantId,
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