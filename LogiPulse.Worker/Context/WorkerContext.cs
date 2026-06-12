using LogiPulse.Application.Common;
using LogiPulse.Domain.Shared;

namespace LogiPulse.Worker.Context;

public class WorkerContext : IUserContext
{
    public Guid UserId { get; private set; }
    public Guid TenantId { get; private set; }
    public Email? Email { get; private set; } = null;
    public bool IsAuthenticated { get; private set; }
    public bool BypassTenantFilter => true;

    public void SetUser(Guid userId, Guid tenantId, string email)
    {
    }
}