using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Infrastructure.Persistence;
using LogiPulse.Infrastructure.Persistence.Interceptors;
using LogiPulse.Infrastructure.Persistence.Repositories;
using LogiPulse.Infrastructure.Persistence.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddLogiPulseInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LogiPulseDatabase");

        services.AddDbContext<LogiPulseDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions => { npgsqlOptions.UseNetTopologySuite(); })
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(new UpdateTimestampsInterceptor());
        });

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IFacilityRepository, FacilityRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IDispatchRepository, DispatchRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}