using LogiPulse.Domain.Shared;

namespace LogiPulse.Application.Common;

public record UserContext
{
    public Guid UserId;
    public Guid TenantId;
    public required Email Email;
}