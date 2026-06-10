using FluentAssertions;
using LogiPulse.Application.Tenants.Commands.RegisterTenant;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.Tenants.Commands.RegisterTenant;

public class RegisterTenantCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private readonly RegisterTenantCommand _command = new()
    {
        TaxCode = Guid.NewGuid().ToString()[0..20],
        DisplayName = "LogiPulse",
        AdminUserEntraId = Guid.NewGuid(),
        AdminUserEmail = "registertest@mail.com",
        AdminUserName = "admin"
    };


    [Fact]
    public async Task Handle_ShouldCreateTenantAndUser()
    {
        var result = await Sender.Send(_command, CancellationToken.None);
        result.Should().NotBeEmpty();

        var tenant = await DbContext.Tenants.IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.TaxCode == _command.TaxCode);

        var user = await DbContext.Users.IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.EntraId == _command.AdminUserEntraId);

        tenant.Should().NotBeNull();
        tenant!.TaxCode.Should().Be(_command.TaxCode);
        tenant!.DisplayName.Should().Be(_command.DisplayName);

        user.Should().NotBeNull();
        user!.EntraId.Should().Be(_command.AdminUserEntraId);
        user!.Email.Should().Be(Email.Create(_command.AdminUserEmail));
        user!.FullName.Should().Be(_command.AdminUserName);
    }

    [Fact]
    public async Task Handle_ExistingTenant_ShouldThrow()
    {
        RegisterTenantCommand command = new()
        {
            TaxCode = Guid.NewGuid().ToString()[0..20],
            DisplayName = "LogiPulse",
            AdminUserEntraId = Guid.NewGuid(),
            AdminUserEmail = "Handle_ExistingTenant_ShouldThrow@mail.com",
            AdminUserName = "admin"
        };

        await Sender.Send(command);

        Func<Task> act = async () => await Sender.Send(command);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("*Tenant already exists*");
    }

    [Fact]
    public async Task Handle_ExistingUser_ShouldThrow()
    {
        RegisterTenantCommand command = new()
        {
            TaxCode = Guid.NewGuid().ToString()[0..20],
            DisplayName = "LogiPulse",
            AdminUserEntraId = Guid.NewGuid(),
            AdminUserEmail = "Handle_ExistingUser_ShouldThrow@mail.com",
            AdminUserName = "admin"
        };

        await Sender.Send(command);

        RegisterTenantCommand command2 = new()
        {
            TaxCode = Guid.NewGuid().ToString()[0..20],
            DisplayName = "LogiPulse",
            AdminUserEntraId = Guid.NewGuid(),
            AdminUserEmail = "Handle_ExistingUser_ShouldThrow@mail.com",
            AdminUserName = "admin"
        };


        Func<Task> act = async () => await Sender.Send(command2);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("*User already exists and belongs to a tenant*");
    }
}