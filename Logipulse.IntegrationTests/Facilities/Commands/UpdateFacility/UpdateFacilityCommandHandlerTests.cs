using FluentAssertions;
using LogiPulse.Application.Facilities.Commands.UpdateFacility;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace LogiPulse.IntegrationTests.Facilities.Commands.UpdateFacility;

public class UpdateFacilityCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_WhenValid_UpdatesFacilityAndCommits()
    {
        var oldAddress = new Address("X", "X", "X", "X", "X", "X", "X");
        var newAddress = new Address("Rod. Hélio Smidt", "s/n", "Guarulhos", "SP", "07190-100", "Brazil", "Aeroporto");


        var facility = Facility.Create(
            UserContext.TenantId,
            Guid.NewGuid().ToString()[0..8],
            "Old Name",
            Guid.NewGuid().ToString()[0..8],
            FacilityType.MaintenanceHub,
            oldAddress,
            new Point(-23.5, -46.6));

        await DbContext.Facilities.AddAsync(facility);
        await DbContext.SaveChangesAsync();

        var command = new UpdateFacilityCommand
        {
            Id = facility.Id,
            Name = "New Name",
            Code = Guid.NewGuid().ToString()[0..8],
            FacilityType = FacilityType.DistributionCenter,
            Latitude = 30,
            Longitude = 10,
            Address = newAddress
        };

        await Sender.Send(command);

        var retrievedFacility = await DbContext.Facilities.FirstOrDefaultAsync(p => p.Id == facility.Id);


        retrievedFacility.Should().NotBeNull();
        retrievedFacility!.Name.Should().Be(command.Name);
        retrievedFacility!.Code.Should().Be(command.Code);
        retrievedFacility!.Type.Should().Be(command.FacilityType);
        retrievedFacility!.Location.Should()
            .BeEquivalentTo(new Point(command.Longitude.Value, command.Latitude.Value),
                options => options.WithStrictOrdering());
        retrievedFacility!.Address.Should().Be(command.Address);
    }

    [Fact]
    public async Task Handle_WhenFacilityDontExist_ShouldThrow()
    {
        var command = new UpdateFacilityCommand
        {
            Id = Guid.NewGuid(),
            Name = "New Name"
        };

        Func<Task> act = async () => await Sender.Send(command);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Facility not found*");
    }
}