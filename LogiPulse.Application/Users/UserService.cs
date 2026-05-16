using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;

namespace LogiPulse.Application.Users;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserService
{
    public async Task<User> AuthAsync(Guid entraId, string email)
    {
        // 1) Find by Azure Entra ID
        var entraUser = await userRepository.ExistsByIdWithoutTenantFilterAsync(entraId);

        if (entraUser is not null)
            return entraUser;

        // 2) Check if the user was invited (user added by admin)
        var user = await userRepository.GetByEmailWithoutTenantFilterAsync(Email.Create(email));

        if (user is null)
            throw new BusinessRuleException("User has not been invited");

        user.SetEntraId(entraId);
        await unitOfWork.CommitAsync();
        return user;
    }

    public async Task<User> InviteUser(Guid tenantId, string email, string fullName)
    {
        var user = User.Create(tenantId, Email.Create(email), fullName ?? email, null);
        await userRepository.AddAsync(user);
        await unitOfWork.CommitAsync();
        return user;
    }
}