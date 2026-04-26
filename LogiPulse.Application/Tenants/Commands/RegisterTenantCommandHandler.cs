using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using MediatR;

namespace LogiPulse.Application.Tenants.Commands;

public class RegisterTenantCommandHandler(
    IUserRepository userRepository,
    ITenantRepository tenantRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<RegisterTenantCommand, Guid>
{
    public async Task<Guid> Handle(RegisterTenantCommand request, CancellationToken cancellationToken)
    {
        var tenantExists = await tenantRepository.ExistsByTaxCodeAsync(request.TaxCode);
        if (tenantExists)
            throw new ConflictException("Tenant already exists");

        var userExists = await userRepository.ExistsByEmailAsync(Email.Create(request.AdminUserEmail));
        if (userExists)
            throw new ConflictException("User already exists and belongs to a tenant");
            
        var tenant = Tenant.Create(request.DisplayName, request.TaxCode);
        var adminUser = User.Create(
            tenant.Id, 
            Email.Create(request.AdminUserEmail), 
            request.AdminUserName, 
            request.AdminUserEntraId
        );
        
        await tenantRepository.AddAsync(tenant);
        await userRepository.AddAsync(adminUser);

        await unitOfWork.CommitAsync(cancellationToken);

        return tenant.Id;
    }
}