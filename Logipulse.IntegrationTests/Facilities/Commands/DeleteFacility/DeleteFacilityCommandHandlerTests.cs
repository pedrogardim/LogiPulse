using FluentAssertions;
using LogiPulse.Application.Facilities.Commands.DeleteFacility;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace LogiPulse.IntegrationTests.Facilities.Commands.DeleteFacility;

public class DeleteFacilityCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteFacilityAndCommits()
    {
        var facility = Facility.Create(UserContext.TenantId, "D-01", "Facility", "123", FacilityType.DeliveryPoint,
            new Address("", "", "", "", "", "", ""), new Point(0, 0));

        await DbContext.Facilities.AddAsync(facility);
        await DbContext.SaveChangesAsync();

        var command = new DeleteFacilityCommand(facility.Id);
        await Sender.Send(command);

        var result = await DbContext.Facilities.FirstOrDefaultAsync(p => p.Id == facility.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenFacilityDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        var act = async () => await Sender.Send(new DeleteFacilityCommand(id));

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Facility don't exist*");
    }
}