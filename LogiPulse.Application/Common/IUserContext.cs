using LogiPulse.Domain.Shared;

namespace LogiPulse.Application.Common;

public interface IUserContext
{
    Guid UserId { get; }
    Guid TenantId { get; }
    Email Email { get; }
    bool IsAuthenticated { get; }

    public void SetUser(Guid userId, Guid tenantId, string email);
}