using System.Net;
using System.Security.Claims;
using System.Web;
using LogiPulse.Api.Extensions;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Infrastructure.Persistance;

namespace LogiPulse.Api.Middlewares;

public class UserTenantMiddleware : IMiddleware
{
    private readonly LogiPulseDbContext _dbContext;
    
    public UserTenantMiddleware(LogiPulseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var userClaims = context.User;

        if (userClaims.Identity?.IsAuthenticated != true)
        {
            // TODO: Global error handler
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync("Unauthorized");
            return;
        }

        var entraIdStr = userClaims.GetObjectId();
        var email = userClaims.GetEmail();

        // TODO: Create user
        // TODO: Put user on scoped context
        
        await next(context);
    }
}