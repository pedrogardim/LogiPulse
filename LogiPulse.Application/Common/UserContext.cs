namespace LogiPulse.Application.Common;

public class UserContext
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public string Email { get; set; } = string.Empty;
}