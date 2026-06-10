using FluentAssertions;
using LogiPulse.Application.Vehicles.Commands.DeleteVehicle;
using LogiPulse.Application.Vehicles.Commands.CreateVehicle;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteVehicleAndCommits()
    {
        var createCommand = new CreateVehicleCommand
        {
            ExternalId = "V-00001",
            Name = "Pedro",
            LicensePlate = "8310XY",
            VehicleType = VehicleType.SemiTruck
        };

        var vehicleId = await Sender.Send(createCommand);
        vehicleId.Should().NotBeEmpty();

        await Sender.Send(new DeleteVehicleCommand(vehicleId));

        var result = await DbContext.Vehicles.AnyAsync(v => v.Id == vehicleId);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenVehicleDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        var act = async () => await Sender.Send(new DeleteVehicleCommand(id));

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Vehicle don't exist*");
    }
}