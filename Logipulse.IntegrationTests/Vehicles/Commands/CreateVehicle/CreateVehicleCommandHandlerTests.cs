using FluentAssertions;
using LogiPulse.Application.Vehicles.Commands.CreateVehicle;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_ShouldCreateVehicle()
    {
        var command = new CreateVehicleCommand
        {
            ExternalId = "V-00001",
            Name = "Pedro",
            LicensePlate = "8310XY",
            VehicleType = VehicleType.SemiTruck
        };

        var vehicleId = await Sender.Send(command);
        vehicleId.Should().NotBeEmpty();

        var vehicle = await DbContext.Vehicles.FirstOrDefaultAsync(d => d.Id == vehicleId);

        vehicle!.TenantId.Should().Be(UserContext.TenantId);
        vehicle!.ExternalId.Should().Be(command.ExternalId);
        vehicle!.Name.Should().Be(command.Name);
        vehicle!.LicensePlate.Should().Be(command.LicensePlate);
        vehicle!.Type.Should().Be(command.VehicleType);

        vehicle.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WhenVehicleAlreadyExists_ShouldThrow()
    {
        var command = new CreateVehicleCommand
        {
            ExternalId = "V-00002",
            Name = "Pedro",
            LicensePlate = "8310XY",
            VehicleType = VehicleType.SemiTruck
        };

        await Sender.Send(command);

        Func<Task> act = async () => await Sender.Send(command);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("*Vehicle already exists*");
    }
}