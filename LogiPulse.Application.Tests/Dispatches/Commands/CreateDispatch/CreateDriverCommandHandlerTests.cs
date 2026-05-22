using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Dispatches.Commands.CreateDispatch;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;
using NSubstitute;

namespace LogiPulse.Application.Tests.Dispatches.Commands.CreateDispatch;

public class CreateDispatchCommandHandlerTests
{
    private readonly IDispatchRepository _dispatchRepositoryMock;
    private readonly IProductRepository _productRepositoryMock;
    private readonly IFacilityRepository _facilityRepositoryMock;
    private readonly IVehicleRepository _vehicleRepositoryMock;
    private readonly IDriverRepository _driverRepositoryMock;

    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateDispatchCommandHandler _handler;
    private readonly Guid _tenantId = Guid.CreateVersion7();

    private readonly CreateDispatchCommand _command = new()
    {
        ExternalId = "D-01",
        ProductId = Guid.CreateVersion7(),
        OriginFacilityId = Guid.CreateVersion7(),
        DestinationFacilityId = Guid.CreateVersion7(),
        VehicleId = Guid.CreateVersion7(),
        DriverId = Guid.CreateVersion7()
    };

    public CreateDispatchCommandHandlerTests()
    {
        _dispatchRepositoryMock = Substitute.For<IDispatchRepository>();
        _productRepositoryMock = Substitute.For<IProductRepository>();
        _facilityRepositoryMock = Substitute.For<IFacilityRepository>();
        _vehicleRepositoryMock = Substitute.For<IVehicleRepository>();
        _driverRepositoryMock = Substitute.For<IDriverRepository>();

        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new CreateDispatchCommandHandler(
            _dispatchRepositoryMock,
            _productRepositoryMock,
            _facilityRepositoryMock,
            _vehicleRepositoryMock,
            _driverRepositoryMock,
            userContextMock,
            _unitOfWorkMock
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateDispatch()
    {
        MockRepositoryData();

        Dispatch? dispatch = null;

        _dispatchRepositoryMock
            .When(x => x.AddAsync(Arg.Any<Dispatch>(), Arg.Any<CancellationToken>()))
            .Do(callInfo => dispatch = callInfo.Arg<Dispatch>());

        var result = await _handler.Handle(_command, CancellationToken.None);
        result.Should().NotBeEmpty();

        dispatch!.TenantId.Should().Be(_tenantId);
        dispatch!.ExternalId.Should().Be(_command.ExternalId);
        dispatch!.ProductId.Should().Be(_command.ProductId);
        dispatch!.OriginFacilityId.Should().Be(_command.OriginFacilityId);
        dispatch!.DestinationFacilityId.Should().Be(_command.DestinationFacilityId);
        dispatch!.VehicleId.Should().Be(_command.VehicleId);
        dispatch!.DriverId.Should().Be(_command.DriverId);

        dispatch.Should().NotBeNull();

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenDispatchAlreadyExists_ShouldThrow()
    {
        _dispatchRepositoryMock
            .ExistsByExternalIdAsync(
                _command.ExternalId,
                CancellationToken.None
            )
            .Returns(true);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*A dispatch with that external id already exists*");
    }

    [Fact]
    public async Task Handle_WhenVehicleIdAndDriverIdAreNotProvided_ShouldCreateDispatch()
    {
        MockRepositoryData();

        Dispatch? dispatch = null;

        var command = new CreateDispatchCommand
        {
            ExternalId = _command.ExternalId,
            ProductId = _command.ProductId,
            OriginFacilityId = _command.OriginFacilityId,
            DestinationFacilityId = _command.DestinationFacilityId
        };

        _dispatchRepositoryMock
            .When(x => x.AddAsync(Arg.Any<Dispatch>(), Arg.Any<CancellationToken>()))
            .Do(callInfo => dispatch = callInfo.Arg<Dispatch>());

        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().NotBeEmpty();

        dispatch!.TenantId.Should().Be(_tenantId);
        dispatch!.ExternalId.Should().Be(command.ExternalId);
        dispatch!.ProductId.Should().Be(command.ProductId);
        dispatch!.OriginFacilityId.Should().Be(command.OriginFacilityId);
        dispatch!.DestinationFacilityId.Should().Be(command.DestinationFacilityId);
        dispatch!.VehicleId.Should().Be(null);
        dispatch!.DriverId.Should().Be(null);

        dispatch.Should().NotBeNull();

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProductDontExist_ShouldThrow()
    {
        MockRepositoryData();

        _productRepositoryMock
            .ExistsByIdAsync(_command.ProductId, Arg.Any<CancellationToken>())
            .Returns(false);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*Product don't exist*");
    }

    [Fact]
    public async Task Handle_WhenOriginFacilityDontExist_ShouldThrow()
    {
        MockRepositoryData();

        _facilityRepositoryMock
            .ExistsByIdAsync(_command.OriginFacilityId, Arg.Any<CancellationToken>())
            .Returns(false);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*Origin facility don't exist*");
    }

    [Fact]
    public async Task Handle_WhenDestinationFacilityDontExist_ShouldThrow()
    {
        MockRepositoryData();

        _facilityRepositoryMock
            .ExistsByIdAsync(_command.DestinationFacilityId, Arg.Any<CancellationToken>())
            .Returns(false);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*Destination facility don't exist*");
    }

    [Fact]
    public async Task Handle_WhenVehicleDontExist_ShouldThrow()
    {
        MockRepositoryData();

        if (_command.VehicleId != null)
            _vehicleRepositoryMock
                .ExistsByIdAsync(_command.VehicleId.Value, Arg.Any<CancellationToken>())
                .Returns(false);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*Vehicle don't exist*");
    }

    [Fact]
    public async Task Handle_WhenDriverDontExist_ShouldThrow()
    {
        MockRepositoryData();

        if (_command.DriverId != null)
            _driverRepositoryMock
                .ExistsByIdAsync(_command.DriverId.Value, Arg.Any<CancellationToken>())
                .Returns(false);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*Driver don't exist*");
    }

    private void MockRepositoryData()
    {
        _productRepositoryMock
            .ExistsByIdAsync(_command.ProductId, Arg.Any<CancellationToken>())
            .Returns(true);

        _facilityRepositoryMock
            .ExistsByIdAsync(_command.OriginFacilityId, Arg.Any<CancellationToken>())
            .Returns(true);

        _facilityRepositoryMock
            .ExistsByIdAsync(_command.DestinationFacilityId, Arg.Any<CancellationToken>())
            .Returns(true);

        if (_command.VehicleId != null)
            _vehicleRepositoryMock
                .ExistsByIdAsync(_command.VehicleId.Value, Arg.Any<CancellationToken>())
                .Returns(true);

        if (_command.DriverId != null)
            _driverRepositoryMock
                .ExistsByIdAsync(_command.DriverId.Value, Arg.Any<CancellationToken>())
                .Returns(true);
    }
}