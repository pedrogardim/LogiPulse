using FluentAssertions;
using LogiPulse.Application.Facilities.Commands.CreateFacility;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.Facilities.Commands.CreateFacility;

public class CreateFacilityCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private readonly Address _address = new("X", "X", "X", "X", "X", "X", "X");

    [Fact]
    public async Task Handle_ShouldCreateFacility()
    {
        var command = new CreateFacilityCommand
        {
            ExternalId = "F-00001",
            Name = "Westroot Warehouse",
            Code = "0567",
            FacilityType = FacilityType.DistributionCenter,
            Latitude = 30,
            Longitude = 10,
            Address = _address
        };

        var facilityId = await Sender.Send(command, CancellationToken.None);
        facilityId.Should().NotBeEmpty();

        var facility = await DbContext.Facilities.FirstOrDefaultAsync(d => d.Id == facilityId);
        facility.Should().NotBeNull();

        facility!.TenantId.Should().Be(UserContext.TenantId);
        facility!.ExternalId.Should().Be(command.ExternalId);
        facility!.Name.Should().Be(command.Name);
        facility!.Code.Should().Be(command.Code);

        facility!.Type.Should().Be(command.FacilityType);

        facility!.Location.X.Should().Be(command.Longitude);
        facility!.Location.Y.Should().Be(command.Latitude);

        facility!.Address.Should().Be(command.Address);
    }

    [Fact]
    public async Task Handle_WhenFacilityAlreadyExistsWithSameExternalId_ShouldThrow()
    {
        var command = new CreateFacilityCommand
        {
            ExternalId = Guid.NewGuid().ToString()[0..20],
            Name = "Some Facility",
            Code = Guid.NewGuid().ToString()[0..10],
            FacilityType = FacilityType.DeliveryPoint,
            Latitude = 30,
            Longitude = 10,
            Address = _address
        };

        await Sender.Send(command);

        var command2 = new CreateFacilityCommand
        {
            ExternalId = command.ExternalId,
            Name = "Some Facility",
            Code = Guid.NewGuid().ToString()[0..10],
            FacilityType = FacilityType.DeliveryPoint,
            Latitude = 30,
            Longitude = 10,
            Address = _address
        };

        Func<Task> act = async () => await Sender.Send(command2);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*A facility with that external id already exists*");
    }

    [Fact]
    public async Task Handle_WhenFacilityAlreadyExistsWithSameCode_ShouldThrow()
    {
        var command = new CreateFacilityCommand
        {
            ExternalId = Guid.NewGuid().ToString()[0..20],
            Name = "Some Facility",
            Code = Guid.NewGuid().ToString()[0..10],
            FacilityType = FacilityType.DeliveryPoint,
            Latitude = 30,
            Longitude = 10,
            Address = _address
        };

        await Sender.Send(command);

        var command2 = new CreateFacilityCommand
        {
            ExternalId = Guid.NewGuid().ToString()[0..20],
            Name = "Some Facility",
            Code = command.Code,
            FacilityType = FacilityType.DeliveryPoint,
            Latitude = 30,
            Longitude = 10,
            Address = _address
        };

        Func<Task> act = async () => await Sender.Send(command2);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("*A facility with that code already exists*");
    }
}