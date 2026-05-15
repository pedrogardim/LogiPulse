using LogiPulse.Api.Attributes;
using LogiPulse.Api.Extensions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Shared;

namespace LogiPulse.Api.Middlewares;

public class UserTenantMiddleware(IUserService userService) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var userClaims = context.User;

        var endpoint = context.GetEndpoint();

        var route = context.Request.Path.Value ?? string.Empty;

        if (route.StartsWith("/openapi"))
        {
            await next(context);
            return;
        }

        if (userClaims.Identity?.IsAuthenticated != true)
            throw new UnauthorizedAccessException();

        var bypassValidation = endpoint?.Metadata?.GetMetadata<BypassUserValidationAttribute>();

        if (bypassValidation is not null)
        {
            await next(context);
            return;
        }

        var entraId = userClaims.GetObjectId();
        var email = userClaims.GetEmail();

        var user = await userService.AuthAsync(entraId, email);

        var userContext = new UserContext
        {
            UserId = user.Id,
            TenantId = user.TenantId,
            Email = Email.Create(email)
        };

        context.Items["UserContext"] = userContext;

        await next(context);
    }
}