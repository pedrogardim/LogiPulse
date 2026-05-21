using System.Runtime.InteropServices.JavaScript;
using FluentAssertions;
using LogiPulse.Application.Facilities.Queries.ListFacilities;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Shared;
using NetTopologySuite.Geometries;
using NSubstitute;

namespace LogiPulse.Application.Tests.Facilities.Queries.ListFacilities;

public class ListFacilitiesQueryHandlerTests
{
    private readonly IFacilityRepository _facilityRepositoryMock;
    private readonly ListFacilitiesQueryHandler _handler;
    private readonly ListFacilitiesQuery _query;

    public ListFacilitiesQueryHandlerTests()
    {
        _facilityRepositoryMock = Substitute.For<IFacilityRepository>();

        _handler = new ListFacilitiesQueryHandler(_facilityRepositoryMock);

        _query = new ListFacilitiesQuery(
            FacilityType.DeliveryPoint,
            "Some Facility",
            1,
            20
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnFacilityItems()
    {
        var facility = Facility.Create(Guid.CreateVersion7(), "D-01", "Facility", "123", FacilityType.DeliveryPoint,
            new Address("", "", "", "", "", "", ""), new Point(0, 0));

        IReadOnlyList<Facility> facilities = [facility];

        _facilityRepositoryMock.ListAsync(
                _query.FacilityType,
                _query.Search,
                _query.Page,
                _query.PageSize,
                Arg.Any<CancellationToken>()
            )
            .Returns(facilities);

        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().BeOfType<List<ListFacilitiesItemResponse>>();

        result.Should().HaveCount(1);

        result[0].Id.Should().Be(facility.Id);
        result[0].ExternalId.Should().Be(facility.ExternalId);
        result[0].Name.Should().Be(facility.Name);
        result[0].Code.Should().Be(facility.Code);
        result[0].Type.Should().Be(facility.Type);
        result[0].City.Should().Be(facility.Address.City);
        result[0].State.Should().Be(facility.Address.State);

        await _facilityRepositoryMock.Received(1).ListAsync(
            _query.FacilityType,
            _query.Search,
            _query.Page,
            _query.PageSize,
            Arg.Any<CancellationToken>()
        );
    }
}