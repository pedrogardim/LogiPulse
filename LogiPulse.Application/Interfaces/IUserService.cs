using LogiPulse.Domain.Entities.Users;

namespace LogiPulse.Application.Interfaces;

public interface IUserService
{
    Task<User> AuthAsync(Guid entraId, string email);
}