using FluentAssertions;
using LogiPulse.Application.Facilities.Commands.CreateFacility;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using NSubstitute;

namespace LogiPulse.Application.Tests.Facilities.Commands.CreateFacility;

public class CreateFacilityCommandHandlerTests
{
    private readonly IFacilityRepository _facilityRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateFacilityCommandHandler _handler;
    private readonly CreateFacilityCommand _command;

    public CreateFacilityCommandHandlerTests()
    {
        _facilityRepositoryMock = Substitute.For<IFacilityRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _handler = new CreateFacilityCommandHandler(_facilityRepositoryMock, _unitOfWorkMock);

        var address = new Address("Rod. Hélio Smidt", "s/n", "Guarulhos", "SP", "07190-100", "Brazil", "Aeroporto");

        _command = new CreateFacilityCommand
        {
            TenantId = Guid.CreateVersion7(),
            ExternalId = "F-00001",
            Name = "Westroot Warehouse",
            Code = "0567",
            FacilityType = FacilityType.DistributionCenter,
            Latitude = 30,
            Longitude = 10,
            Address = address
        };
    }

    [Fact]
    public async Task Handle_ShouldCreateFacility()
    {
        Facility? capturedFacility = null;

        _facilityRepositoryMock
            .When(x => x.AddAsync(Arg.Any<Facility>(), CancellationToken.None))
            .Do(callInfo => capturedFacility = callInfo.Arg<Facility>());

        var result = await _handler.Handle(_command, CancellationToken.None);
        result.Should().NotBeEmpty();

        capturedFacility!.TenantId.Should().Be(_command.TenantId);
        capturedFacility!.ExternalId.Should().Be(_command.ExternalId);
        capturedFacility!.Name.Should().Be(_command.Name);
        capturedFacility!.Code.Should().Be(_command.Code);

        capturedFacility!.Type.Should().Be(_command.FacilityType);

        capturedFacility!.Location.X.Should().Be(_command.Longitude);
        capturedFacility!.Location.Y.Should().Be(_command.Latitude);

        capturedFacility!.Address.Should().Be(_command.Address);

        capturedFacility.Should().NotBeNull();

        await _facilityRepositoryMock.Received(1)
            .AddAsync(Arg.Any<Facility>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenFacilityAlreadyExistsWithSameExternalId_ShouldThrow()
    {
        _facilityRepositoryMock
            .ExistsByTenantIdAndExternalIdAsync(
                _command.TenantId,
                _command.ExternalId,
                CancellationToken.None
            )
            .Returns(true);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*A facility with that external id already exists*");
    }

    [Fact]
    public async Task Handle_WhenFacilityAlreadyExistsWithSameCode_ShouldThrow()
    {
        _facilityRepositoryMock
            .ExistsByTenantIdAndCodeAsync(
                _command.TenantId,
                _command.Code,
                CancellationToken.None
            )
            .Returns(true);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("*A facility with that code already exists*");
    }
}