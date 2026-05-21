using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Vehicles.Commands.DeleteVehicle;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;
using NSubstitute;

namespace LogiPulse.Application.Tests.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandlerTests
{
    private readonly IVehicleRepository _vehicleRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeleteVehicleCommandHandler _handler;
    private readonly Guid _tenantId;

    public DeleteVehicleCommandHandlerTests()
    {
        _vehicleRepositoryMock = Substitute.For<IVehicleRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        _tenantId = Guid.CreateVersion7();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new DeleteVehicleCommandHandler(
            _vehicleRepositoryMock,
            userContextMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteVehicleAndCommits()
    {
        var vehicle = Vehicle.Create(_tenantId, "D-01", "Vehicle", "123", VehicleType.BoxTruck);

        var command = new DeleteVehicleCommand(vehicle.Id);

        _vehicleRepositoryMock
            .GetByIdAsync(vehicle.Id, Arg.Any<CancellationToken>())
            .Returns(vehicle);

        await _handler.Handle(command, CancellationToken.None);

        _vehicleRepositoryMock.Received(1).Remove(vehicle);

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenVehicleDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        _vehicleRepositoryMock
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Vehicle?>(null));

        var act = async () => await _handler.Handle(new DeleteVehicleCommand(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Vehicle don't exist*");
    }
}