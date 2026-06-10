using FluentAssertions;
using LogiPulse.Application.Drivers.Commands.CreateDriver;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Exceptions;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace LogiPulse.IntegrationTests.Drivers.Commands.CreateDriver;

public class CreateDriverCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_ShouldCreateDriver()
    {
        var randomUser = await DbContext.Users.FirstAsync();
        var command = new CreateDriverCommand
        {
            ExternalId = Guid.NewGuid().ToString()[0..8],
            UserId = randomUser.Id,
            Name = "Pedro",
            Phone = "12345678",
            LicenseNumber = Guid.NewGuid().ToString()[0..8],
            LicenseExpiryDate = DateOnly.MaxValue
        };

        var driverId = await Sender.Send(command);
        driverId.Should().NotBeEmpty();

        var driver = await DbContext.Drivers.FirstOrDefaultAsync(d => d.Id == driverId);
        driver.Should().NotBeNull();

        driver!.TenantId.Should().Be(UserContext.TenantId);
        driver!.ExternalId.Should().Be(command.ExternalId);
        driver!.UserId.Should().Be(command.UserId);
        driver!.Name.Should().Be(command.Name);
        driver!.Phone.Should().Be(command.Phone);
        driver!.LicenseNumber.Should().Be(command.LicenseNumber);
        driver!.LicenseExpiryDate.Should().Be(command.LicenseExpiryDate);
    }

    [Fact]
    public async Task Handle_WhenUserIdIsNotProvided_ShouldCreateDriver()
    {
        var command = new CreateDriverCommand
        {
            ExternalId = Guid.NewGuid().ToString()[0..8],
            Name = "Pedro",
            Phone = "12345678",
            LicenseNumber = Guid.NewGuid().ToString()[0..8],
            LicenseExpiryDate = DateOnly.MaxValue
        };

        var driverId = await Sender.Send(command);
        driverId.Should().NotBeEmpty();

        var driver = await DbContext.Drivers.FirstOrDefaultAsync(d => d.Id == driverId);
        driver.Should().NotBeNull();

        driver!.TenantId.Should().Be(UserContext.TenantId);
        driver!.ExternalId.Should().Be(command.ExternalId);
        driver!.UserId.Should().BeNull();
        driver!.Name.Should().Be(command.Name);
        driver!.Phone.Should().Be(command.Phone);
        driver!.LicenseNumber.Should().Be(command.LicenseNumber);
        driver!.LicenseExpiryDate.Should().Be(command.LicenseExpiryDate);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrow()
    {
        var command = new CreateDriverCommand
        {
            ExternalId = Guid.NewGuid().ToString()[0..8],
            UserId = Guid.CreateVersion7(),
            Name = "Pedro",
            Phone = "12345678",
            LicenseNumber = Guid.NewGuid().ToString()[0..8],
            LicenseExpiryDate = DateOnly.MaxValue
        };

        Func<Task> act = async () => await Sender.Send(command);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*Given user doesn't exist*");
    }

    [Fact]
    public async Task Handle_WhenDriverAlreadyExists_ShouldThrow()
    {
        var command = new CreateDriverCommand
        {
            ExternalId = Guid.NewGuid().ToString()[0..8],
            Name = "Pedro",
            Phone = "12345678",
            LicenseNumber = Guid.NewGuid().ToString()[0..8],
            LicenseExpiryDate = DateOnly.MaxValue
        };

        await Sender.Send(command);

        Func<Task> act = async () => await Sender.Send(command);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("*Driver already exists*");
    }
}