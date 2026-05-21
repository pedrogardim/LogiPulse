using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.Persistence.Repositories;

public class UserRepository(LogiPulseDbContext context) : IUserRepository
{
    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> ExistsByEmailAsync(Email email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
    }

    public async Task<User?> ExistsByIdWithoutTenantFilterAsync(Guid entraId)
    {
        return await context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.EntraId == entraId);
    }

    public async Task<User?> GetByEmailWithoutTenantFilterAsync(Email email)
    {
        return await context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Users.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public void Remove(User user)
    {
        context.Users.Remove(user);
    }
}