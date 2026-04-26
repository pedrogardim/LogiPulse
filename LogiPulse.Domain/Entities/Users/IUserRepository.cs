using LogiPulse.Domain.Shared;

namespace LogiPulse.Domain.Entities.Users;

public interface IUserRepository
{
    public Task<User?>GetByEntraIdAsync(Guid entraId);
    public Task<bool> ExistsByEmailAsync(Email email);
    public Task<User?> GetByEmailAsync(Email email);
    public Task AddAsync(User user);
}