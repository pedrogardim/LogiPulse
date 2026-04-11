using LogiPulse.Infrastructure.Persistance;
using LogiPulse.Infrastructure.Persistance.Interceptors;
using LogiPulse.Infrastructure.Persistance.Seeds;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("LogiPulseDatabase");

builder.Services.AddDbContext<LogiPulseDbContext>((sp, options) =>
{
    options.UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention()
        .AddInterceptors(new UpdateTimestampsInterceptor());
});

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
app.MapControllers();
app.Run();