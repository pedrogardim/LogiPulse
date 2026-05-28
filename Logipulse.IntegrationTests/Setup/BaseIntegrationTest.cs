using LogiPulse.Application.Common;
using LogiPulse.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Logipulse.IntegrationTests.Setup;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly LogiPulseDbContext DbContext;
    protected readonly IUserContext UserContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<LogiPulseDbContext>();
        UserContext = _scope.ServiceProvider.GetRequiredService<IUserContext>();

        UserContext.SetUser(factory.UserId, factory.TenantId, factory.UserEmail);
    }
}