using LogiPulse.Domain.Shared;

namespace LogiPulse.Domain.Entities.Users;

public interface IUserRepository
{
    public Task<bool> ExistsByEmailAsync(Email email);
    public Task<bool> ExistsByIdAsync(Guid id);

    public Task AddAsync(User user);

    public Task<User?> ExistsByIdWithoutTenantFilterAsync(Guid entraId);
    public Task<User?> GetByEmailWithoutTenantFilterAsync(Email email);
}