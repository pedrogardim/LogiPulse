using FluentValidation;
using LogiPulse.Application.Interfaces;
using LogiPulse.Application.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LogiPulse.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddLogiPulseApplication(this IServiceCollection services)
    {
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(IUnitOfWork).Assembly);

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly)
        );

        services.AddScoped<IUserService, UserService>();

        return services;
    }
}