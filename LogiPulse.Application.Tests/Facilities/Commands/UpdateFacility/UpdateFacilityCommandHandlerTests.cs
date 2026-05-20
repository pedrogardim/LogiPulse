using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Facilities.Commands.CreateFacility;
using LogiPulse.Application.Facilities.Commands.UpdateFacility;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using NetTopologySuite.Geometries;
using NSubstitute;

namespace LogiPulse.Application.Tests.Facilities.Commands.UpdateFacility;

public class UpdateFacilityCommandHandlerTests
{
    private readonly IFacilityRepository _facilityRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateFacilityCommandHandler _handler;
    private readonly UpdateFacilityCommand _command;
    private readonly Guid _tenantId;

    public UpdateFacilityCommandHandlerTests()
    {
        _facilityRepositoryMock = Substitute.For<IFacilityRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        _tenantId = Guid.CreateVersion7();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new UpdateFacilityCommandHandler(_facilityRepositoryMock, userContextMock, _unitOfWorkMock);

        var address = new Address("Rod. Hélio Smidt", "s/n", "Guarulhos", "SP", "07190-100", "Brazil", "Aeroporto");

        _command = new UpdateFacilityCommand
        {
            Name = "Westroot Warehouse",
            Code = "0567",
            FacilityType = FacilityType.DistributionCenter,
            Latitude = 30,
            Longitude = 10,
            Address = address
        };
    }

    [Fact]
    public async Task Handle_WhenValid_UpdatesFacilityAndCommits()
    {
        var address = new Address("X", "X", "X", "X", "X", "X", "X");

        var facility = Facility.Create(
            _tenantId,
            "ID-0001",
            "Old Name",
            "OLD",
            FacilityType.MaintenanceHub,
            address,
            new Point(-23.5, -46.6));

        _command.Id = facility.Id;

        _facilityRepositoryMock
            .GetByIdAsync(facility.Id, Arg.Any<CancellationToken>())
            .Returns(facility);

        var result = await _handler.Handle(_command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(_command.Name);
        result.Code.Should().Be(_command.Code);
        result.Type.Should().Be(_command.FacilityType);
        result.Location.Should()
            .BeEquivalentTo(new Point(_command.Longitude.Value, _command.Latitude.Value),
                options => options.WithStrictOrdering());
        result.Address.Should().Be(_command.Address);

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }


    [Fact]
    public async Task Handle_WhenFacilityDontExist_ShouldThrow()
    {
        _facilityRepositoryMock
            .GetByIdAsync(_command.Id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Facility?>(null));

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Facility not found*");
    }
}