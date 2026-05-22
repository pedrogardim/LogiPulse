using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Interfaces;
using LogiPulse.Application.Vehicles.Commands.CreateVehicle;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;
using NSubstitute;

namespace LogiPulse.IntegrationTests.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandlerTests
{
    private readonly IVehicleRepository _vehicleRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateVehicleCommandHandler _handler;
    private readonly Guid _tenantId;

    public CreateVehicleCommandHandlerTests()
    {
        _vehicleRepositoryMock = Substitute.For<IVehicleRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        _tenantId = Guid.CreateVersion7();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new CreateVehicleCommandHandler(_vehicleRepositoryMock, userContextMock, _unitOfWorkMock);
    }

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

        Vehicle? capturedVehicle = null;

        _vehicleRepositoryMock
            .When(x => x.AddAsync(Arg.Any<Vehicle>(), CancellationToken.None))
            .Do(callInfo => capturedVehicle = callInfo.Arg<Vehicle>());

        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().NotBeEmpty();

        capturedVehicle!.TenantId.Should().Be(_tenantId);
        capturedVehicle!.ExternalId.Should().Be(command.ExternalId);
        capturedVehicle!.Name.Should().Be(command.Name);
        capturedVehicle!.LicensePlate.Should().Be(command.LicensePlate);
        capturedVehicle!.Type.Should().Be(command.VehicleType);

        capturedVehicle.Should().NotBeNull();

        await _vehicleRepositoryMock.Received(1)
            .AddAsync(Arg.Any<Vehicle>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenVehicleAlreadyExists_ShouldThrow()
    {
        var command = new CreateVehicleCommand
        {
            ExternalId = "V-00001",
            Name = "Pedro",
            LicensePlate = "8310XY",
            VehicleType = VehicleType.SemiTruck
        };

        _vehicleRepositoryMock
            .ExistsByTenantIdAndExternalIdAsync(
                _tenantId,
                command.ExternalId,
                CancellationToken.None
            )
            .Returns(true);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("*Vehicle already exists*");
    }
}