using LogiPulse.Application.Common;
using LogiPulse.Domain.Shared;

namespace LogiPulse.Api.Context;

public class UserContext : IUserContext
{
    public Guid UserId { get; private set; }
    public Guid TenantId { get; private set; }
    public Email? Email { get; private set; } = null;
    public bool IsAuthenticated { get; private set; }
    public bool BypassTenantFilter => false;

    public void SetUser(Guid userId, Guid tenantId, string email)
    {
        UserId = userId;
        TenantId = tenantId;
        Email = Email.Create(email);
        IsAuthenticated = true;
    }
}