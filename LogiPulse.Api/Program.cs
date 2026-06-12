using LogiPulse.Api.Middlewares;
using LogiPulse.Api.DependencyInjection;
using LogiPulse.Application.DependencyInjection;
using LogiPulse.Infrastructure.DependencyInjection;
using LogiPulse.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLogiPulseApplication()
    .AddLogiPulseInfrastructure(builder.Configuration)
    .AddLogiPulseApi(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LogiPulseDbContext>();

    await context.Database.MigrateAsync();
    // await DbInitializer.SeedAsync(context);
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorCatcherMiddleware>();
app.UseMiddleware<UserTenantMiddleware>();
app.MapControllers();
app.Run();