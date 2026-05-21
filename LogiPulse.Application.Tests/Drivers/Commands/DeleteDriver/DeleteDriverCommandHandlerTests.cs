using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Drivers.Commands.DeleteDriver;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Exceptions;
using NSubstitute;

namespace LogiPulse.Application.Tests.Drivers.Commands.DeleteDriver;

public class DeleteDriverCommandHandlerTests
{
    private readonly IDriverRepository _driverRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeleteDriverCommandHandler _handler;
    private readonly Guid _tenantId;

    public DeleteDriverCommandHandlerTests()
    {
        _driverRepositoryMock = Substitute.For<IDriverRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        _tenantId = Guid.CreateVersion7();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new DeleteDriverCommandHandler(
            _driverRepositoryMock,
            userContextMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteDriverAndCommits()
    {
        var driver = Driver.Create(_tenantId, Guid.CreateVersion7(), "D-01", "Driver", "123", "345", DateOnly.MaxValue);

        var command = new DeleteDriverCommand(driver.Id);

        _driverRepositoryMock
            .GetByIdAsync(driver.Id, Arg.Any<CancellationToken>())
            .Returns(driver);

        await _handler.Handle(command, CancellationToken.None);

        _driverRepositoryMock.Received(1).Remove(driver);

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenFacilityDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        _driverRepositoryMock
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Driver?>(null));

        var act = async () => await _handler.Handle(new DeleteDriverCommand(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Driver don't exist*");
    }
}