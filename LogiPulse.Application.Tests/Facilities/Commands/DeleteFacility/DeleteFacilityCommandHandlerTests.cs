using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Facilities.Commands.DeleteFacility;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using NetTopologySuite.Geometries;
using NSubstitute;

namespace LogiPulse.Application.Tests.Facilities.Commands.DeleteFacility;

public class DeleteFacilityCommandHandlerTests
{
    private readonly IFacilityRepository _facilityRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeleteFacilityCommandHandler _handler;
    private readonly Guid _tenantId;

    public DeleteFacilityCommandHandlerTests()
    {
        _facilityRepositoryMock = Substitute.For<IFacilityRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        _tenantId = Guid.CreateVersion7();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new DeleteFacilityCommandHandler(
            _facilityRepositoryMock,
            userContextMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteFacilityAndCommits()
    {
        var facility = Facility.Create(_tenantId, "D-01", "Facility", "123", FacilityType.DeliveryPoint,
            new Address("", "", "", "", "", "", ""), new Point(0, 0));

        var command = new DeleteFacilityCommand(facility.Id);

        _facilityRepositoryMock
            .GetByIdAsync(facility.Id, Arg.Any<CancellationToken>())
            .Returns(facility);

        await _handler.Handle(command, CancellationToken.None);

        _facilityRepositoryMock.Received(1).Remove(facility);

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenFacilityDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        _facilityRepositoryMock
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Facility?>(null));

        var act = async () => await _handler.Handle(new DeleteFacilityCommand(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Facility don't exist*");
    }
}