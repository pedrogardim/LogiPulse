using FluentAssertions;
using LogiPulse.Application.Dispatches.Commands.CreateDispatch;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace LogiPulse.IntegrationTests.Dispatches.Commands.CreateDispatch;

public record DispatchEntities
{
    public required Product Product;
    public required Facility OriginFacility;
    public required Facility DestinationFacility;
    public required Vehicle Vehicle;
    public required Driver Driver;
}

public class CreateDispatchCommandHandlerTests : BaseIntegrationTest
{
    private readonly CreateDispatchCommand _command;

    public CreateDispatchCommandHandlerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        var entities = MockRepositoryData();
        _command = new CreateDispatchCommand
        {
            ExternalId = Guid.NewGuid().ToString()[0..8],
            ProductId = entities.Product.Id,
            OriginFacilityId = entities.OriginFacility.Id,
            DestinationFacilityId = entities.DestinationFacility.Id,
            VehicleId = entities.Vehicle.Id,
            DriverId = entities.Driver.Id
        };
    }

    [Fact]
    public async Task Handle_ShouldCreateDispatch()
    {
        var dispatchId = await Sender.Send(_command);
        dispatchId.Should().NotBeEmpty();

        var dispatch = await DbContext.Dispatches.FirstOrDefaultAsync(d => d.Id == dispatchId);

        dispatch.Should().NotBeNull();
        dispatch!.TenantId.Should().Be(UserContext.TenantId);
        dispatch!.ExternalId.Should().Be(_command.ExternalId);
        dispatch!.ProductId.Should().Be(_command.ProductId);
        dispatch!.OriginFacilityId.Should().Be(_command.OriginFacilityId);
        dispatch!.DestinationFacilityId.Should().Be(_command.DestinationFacilityId);
        dispatch!.VehicleId.Should().Be(_command.VehicleId);
        dispatch!.DriverId.Should().Be(_command.DriverId);
    }

    [Fact]
    public async Task Handle_WhenDispatchAlreadyExists_ShouldThrow()
    {
        await Sender.Send(_command);

        Func<Task> act = async () => await Sender.Send(_command);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*A dispatch with that external id already exists*");
    }

    [Fact]
    public async Task Handle_WhenVehicleIdAndDriverIdAreNotProvided_ShouldCreateDispatch()
    {
        _command.VehicleId = null;
        _command.DriverId = null;

        var dispatchId = await Sender.Send(_command);
        dispatchId.Should().NotBeEmpty();

        var dispatch = await DbContext.Dispatches.FirstOrDefaultAsync(d => d.Id == dispatchId);

        dispatch.Should().NotBeNull();
        dispatch!.TenantId.Should().Be(UserContext.TenantId);
        dispatch!.ExternalId.Should().Be(_command.ExternalId);
        dispatch!.ProductId.Should().Be(_command.ProductId);
        dispatch!.OriginFacilityId.Should().Be(_command.OriginFacilityId);
        dispatch!.DestinationFacilityId.Should().Be(_command.DestinationFacilityId);
        dispatch!.VehicleId.Should().BeNull();
        dispatch!.DriverId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenProductDontExist_ShouldThrow()
    {
        await DbContext.Products.Where(p => p.Id == _command.ProductId).ExecuteDeleteAsync();

        Func<Task> act = async () => await Sender.Send(_command);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*Product don't exist*");
    }

    [Fact]
    public async Task Handle_WhenOriginFacilityDontExist_ShouldThrow()
    {
        await DbContext.Facilities.Where(p => p.Id == _command.OriginFacilityId).ExecuteDeleteAsync();

        Func<Task> act = async () => await Sender.Send(_command);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*Origin facility don't exist*");
    }

    [Fact]
    public async Task Handle_WhenDestinationFacilityDontExist_ShouldThrow()
    {
        await DbContext.Facilities.Where(p => p.Id == _command.DestinationFacilityId).ExecuteDeleteAsync();

        Func<Task> act = async () => await Sender.Send(_command);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*Destination facility don't exist*");
    }

    [Fact]
    public async Task Handle_WhenVehicleDontExist_ShouldThrow()
    {
        await DbContext.Vehicles.Where(p => p.Id == _command.VehicleId).ExecuteDeleteAsync();

        Func<Task> act = async () => await Sender.Send(_command);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*Vehicle don't exist*");
    }

    [Fact]
    public async Task Handle_WhenDriverDontExist_ShouldThrow()
    {
        await DbContext.Drivers.Where(p => p.Id == _command.DriverId).ExecuteDeleteAsync();

        Func<Task> act = async () => await Sender.Send(_command);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*Driver don't exist*");
    }

    private DispatchEntities MockRepositoryData()
    {
        var product = Product.Create(
            UserContext.TenantId,
            Guid.NewGuid().ToString()[0..8],
            Guid.NewGuid().ToString()[0..8],
            "Product");

        var originFacility = Facility.Create(
            UserContext.TenantId,
            Guid.NewGuid().ToString()[0..8],
            "Facility X",
            Guid.NewGuid().ToString()[0..8],
            FacilityType.DeliveryPoint,
            new Address("X", "X", "X", "X", "X", "X", "X"),
            new Point(0, 0)
        );

        var destinationFacility = Facility.Create(
            UserContext.TenantId,
            Guid.NewGuid().ToString()[0..8],
            "Facility X",
            Guid.NewGuid().ToString()[0..8],
            FacilityType.DeliveryPoint,
            new Address("X", "X", "X", "X", "X", "X", "X"),
            new Point(0, 0)
        );

        var vehicle = Vehicle.Create(
            UserContext.TenantId,
            Guid.NewGuid().ToString()[0..8],
            "Test Vehicle",
            Guid.NewGuid().ToString()[0..8],
            VehicleType.BoxTruck
        );

        var driver = Driver.Create(
            UserContext.TenantId,
            null,
            Guid.NewGuid().ToString()[0..8],
            "Test Driver",
            "123",
            "1234",
            DateOnly.MaxValue
        );

        DbContext.Products.Add(product);
        DbContext.Facilities.Add(originFacility);
        DbContext.Facilities.Add(destinationFacility);
        DbContext.Vehicles.Add(vehicle);
        DbContext.Drivers.Add(driver);
        DbContext.SaveChanges();

        var entitiesIds = new DispatchEntities
        {
            Product = product,
            OriginFacility = originFacility,
            DestinationFacility = destinationFacility,
            Vehicle = vehicle,
            Driver = driver
        };

        return entitiesIds;
    }
}