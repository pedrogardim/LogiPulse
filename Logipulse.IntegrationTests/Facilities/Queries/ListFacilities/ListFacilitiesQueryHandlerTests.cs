using FluentAssertions;
using LogiPulse.Application.Facilities.Queries.ListFacilities;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Shared;
using Logipulse.IntegrationTests.Setup;
using NetTopologySuite.Geometries;

namespace LogiPulse.IntegrationTests.Facilities.Queries.ListFacilities;

public class ListFacilitiesQueryHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_ShouldReturnFacilityItems()
    {
        var facility = Facility.Create(
            UserContext.TenantId,
            "D-01",
            Guid.NewGuid().ToString()[0..20],
            Guid.NewGuid().ToString()[0..8],
            FacilityType.DeliveryPoint,
            new Address("", "", "", "", "", "", ""),
            new Point(0, 0));

        await DbContext.Facilities.AddAsync(facility);
        await DbContext.SaveChangesAsync();

        var query = new ListFacilitiesQuery(
            FacilityType.DeliveryPoint,
            facility.Name
        );

        var result = await Sender.Send(query, CancellationToken.None);

        result.Should().BeOfType<List<ListFacilitiesItemResponse>>();

        result.Should().HaveCount(1);

        result[0].Id.Should().Be(facility.Id);
        result[0].ExternalId.Should().Be(facility.ExternalId);
        result[0].Name.Should().Be(facility.Name);
        result[0].Code.Should().Be(facility.Code);
        result[0].Type.Should().Be(facility.Type);
        result[0].City.Should().Be(facility.Address.City);
        result[0].State.Should().Be(facility.Address.State);
    }
}