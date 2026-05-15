using LogiPulse.Application.Common;

namespace LogiPulse.Api.Extensions;

public static class HttpContextExtensions
{
    public static UserContext GetUserContext(this HttpContext context)
    {
        return context?.Items["UserContext"] as UserContext;
    }

    public static Guid GetTenantId(this HttpContext context)
    {
        return context.GetUserContext()?.TenantId ?? Guid.Empty;
    }
}