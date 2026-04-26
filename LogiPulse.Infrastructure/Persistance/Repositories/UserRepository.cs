using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.Persistance.Repositories;

public class UserRepository(LogiPulseDbContext context) : IUserRepository
{
    public async Task<User?> GetByEntraIdAsync(Guid entraId)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.EntraId == entraId);
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
    
    public async Task<User?> GetByEmailAsync(Email email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<bool> ExistsByEmailAsync(Email email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
    }
}