using LogiPulse.Api.Extensions;
using LogiPulse.Application.Interfaces;

namespace LogiPulse.Api.Middlewares;

public class UserTenantMiddleware(IUserService userService) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var userClaims = context.User;

        if (userClaims.Identity?.IsAuthenticated != true)
            throw new UnauthorizedAccessException();

        var entraId = userClaims.GetObjectId();
        var email = userClaims.GetEmail();

        var user = await userService.AuthAsync(entraId, email);

        // TODO: Create user
        // TODO: Put user on scoped context

        await next(context);
    }
}