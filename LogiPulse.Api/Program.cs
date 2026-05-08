using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;
using FluentValidation;
using LogiPulse.Api.Middlewares;
using LogiPulse.Application.Interfaces;
using LogiPulse.Application.Tenants;
using LogiPulse.Application.Users;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Infrastructure.Persistence;
using LogiPulse.Infrastructure.Persistence.Interceptors;
using LogiPulse.Infrastructure.Persistence.Repositories;
using LogiPulse.Infrastructure.Persistence.Seeds;
using LogiPulse.Infrastructure.Persistence.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

var connectionString = builder.Configuration.GetConnectionString("LogiPulseDatabase");

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

builder.Services.AddValidatorsFromAssembly(typeof(IUnitOfWork).Assembly);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly)
);

builder.Services.AddDbContext<LogiPulseDbContext>((sp, options) =>
{
    options.UseNpgsql(connectionString, npgsqlOptions => { npgsqlOptions.UseNetTopologySuite(); })
        .UseSnakeCaseNamingConvention()
        .AddInterceptors(new UpdateTimestampsInterceptor());
});

builder.Services.AddScoped<UserTenantMiddleware>();
builder.Services.AddScoped<ErrorCatcherMiddleware>();

builder.Services.AddScoped<ITenantRepository, TenantRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LogiPulseDbContext>();

    await context.Database.MigrateAsync();
    await DbInitializer.SeedAsync(context);
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorCatcherMiddleware>();
app.UseMiddleware<UserTenantMiddleware>();
app.MapControllers();
app.Run();