using LogiPulse.Infrastructure.Persistence;
using LogiPulse.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Shared;

namespace Logipulse.IntegrationTests.Setup;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder("postgis/postgis:15-3.3").Build();

    public Guid UserId;
    public Guid TenantId;
    public string UserEmail;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<LogiPulseDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<LogiPulseDbContext>((container, options) =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString(),
                        npgsqlOptions => { npgsqlOptions.UseNetTopologySuite(); })
                    .UseSnakeCaseNamingConvention()
                    .AddInterceptors(new UpdateTimestampsInterceptor());
            });
        });

        builder.UseEnvironment("Development");
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await InitializeDatabase();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }

    private async Task InitializeDatabase()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<LogiPulseDbContext>();

        await dbContext.Database.MigrateAsync();

        var tenant = Tenant.Create("Tenant", "123");
        var adminUser = User.Create(
            tenant.Id,
            Email.Create("user@mail.com"),
            "User",
            Guid.CreateVersion7()
        );

        await dbContext.Tenants.AddAsync(tenant);
        await dbContext.Users.AddAsync(adminUser);
        await dbContext.SaveChangesAsync();

        UserId = adminUser.Id;
        TenantId = tenant.Id;
        UserEmail = "test@user.com";
    }
}