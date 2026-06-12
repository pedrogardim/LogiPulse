using System.Text.Json.Serialization;
using LogiPulse.Api.Context;
using LogiPulse.Api.Middlewares;
using LogiPulse.Application.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace LogiPulse.Api.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddLogiPulseApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOpenApi();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));

        services.AddAuthorization();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.IncludeFields = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<UserTenantMiddleware>();
        services.AddScoped<ErrorCatcherMiddleware>();

        return services;
    }
}