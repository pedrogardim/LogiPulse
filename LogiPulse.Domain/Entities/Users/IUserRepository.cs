using LogiPulse.Domain.Shared;

namespace LogiPulse.Domain.Entities.Users;

public interface IUserRepository
{
    public Task<bool> ExistsByIdAsync(Guid id);

    public Task<bool> ExistsByEmailAsync(Email email);
    public Task<bool> ExistsByEmailWithoutTenantFilterAsync(Email email);

    public Task AddAsync(User user);

    public Task<User?> ExistsByIdWithoutTenantFilterAsync(Guid entraId);

    public Task<User?> GetByEmailWithoutTenantFilterAsync(Email email);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public void Remove(User user);
}