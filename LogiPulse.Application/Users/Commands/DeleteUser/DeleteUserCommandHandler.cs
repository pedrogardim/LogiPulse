using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using MediatR;

namespace LogiPulse.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IUserContext userContext,
    IUnitOfWork uow
) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var tenantId = userContext.TenantId;

        if (tenantId == Guid.Empty)
            throw new UnauthorizedBusinessException("Tenant context is required.");

        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
            throw new NotFoundException("User don't exist");

        userRepository.Remove(user);

        await uow.CommitAsync(cancellationToken);
    }
}