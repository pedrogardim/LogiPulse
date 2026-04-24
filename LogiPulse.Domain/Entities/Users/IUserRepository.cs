namespace LogiPulse.Domain.Entities.Users;

public interface IUserRepository
{
    public Task<User?>GetByEntraIdAsync(Guid entraId);
    public Task<bool> ExistsByEmailAsync(string email);
    public Task<User?> GetByEmailAsync(string email);
    public Task AddAsync(User user);
}