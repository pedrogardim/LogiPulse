using FluentAssertions;
using LogiPulse.Application.Drivers.Commands.DeleteDriver;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Exceptions;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.Drivers.Commands.DeleteDriver;

public class DeleteDriverCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteDriverAndCommits()
    {
        var driver = Driver.Create(
            UserContext.TenantId,
            UserContext.UserId,
            "D-01",
            "Driver",
            "123",
            "345",
            DateOnly.MaxValue);

        await DbContext.Drivers.AddAsync(driver);
        await DbContext.SaveChangesAsync();

        var command = new DeleteDriverCommand(driver.Id);

        await Sender.Send(command, CancellationToken.None);

        var result = await DbContext.Drivers.FirstOrDefaultAsync(p => p.Id == driver.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenDriverDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        var act = async () => await Sender.Send(new DeleteDriverCommand(id));

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Driver don't exist*");
    }
}