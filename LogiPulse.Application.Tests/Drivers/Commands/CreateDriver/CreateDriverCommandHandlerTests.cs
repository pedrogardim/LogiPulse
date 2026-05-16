using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Drivers.Commands.CreateDriver;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using NSubstitute;

namespace LogiPulse.Application.Tests.Drivers.Commands.CreateDriver;

public class CreateDriverCommandHandlerTests
{
    private readonly IDriverRepository _driverRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateDriverCommandHandler _handler;
    private readonly Guid _tenantId;

    public CreateDriverCommandHandlerTests()
    {
        _driverRepositoryMock = Substitute.For<IDriverRepository>();
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        _tenantId = Guid.CreateVersion7();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new CreateDriverCommandHandler(
            _driverRepositoryMock,
            _userRepositoryMock,
            userContextMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldCreateDriver()
    {
        var command = new CreateDriverCommand
        {
            ExternalId = "D-00001",
            UserId = Guid.CreateVersion7(),
            Name = "Pedro",
            Phone = "12345678",
            LicenseNumber = "8310XY",
            LicenseExpiryDate = DateOnly.MaxValue
        };

        Driver? capturedDriver = null;

        _userRepositoryMock
            .ExistsByIdAsync(command.UserId.Value)
            .Returns(true);

        _driverRepositoryMock
            .When(x => x.AddAsync(Arg.Any<Driver>(), CancellationToken.None))
            .Do(callInfo => capturedDriver = callInfo.Arg<Driver>());

        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().NotBeEmpty();

        capturedDriver!.TenantId.Should().Be(_tenantId);
        capturedDriver!.ExternalId.Should().Be(command.ExternalId);
        capturedDriver!.UserId.Should().Be(command.UserId);
        capturedDriver!.Name.Should().Be(command.Name);
        capturedDriver!.Phone.Should().Be(command.Phone);
        capturedDriver!.LicenseNumber.Should().Be(command.LicenseNumber);
        capturedDriver!.LicenseExpiryDate.Should().Be(command.LicenseExpiryDate);

        capturedDriver.Should().NotBeNull();

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUserIdIsNotProvided_ShouldCreateDriver()
    {
        var command = new CreateDriverCommand
        {
            ExternalId = "D-00001",
            Name = "Pedro",
            Phone = "12345678",
            LicenseNumber = "8310XY",
            LicenseExpiryDate = DateOnly.MaxValue
        };

        Driver? capturedDriver = null;

        _driverRepositoryMock
            .When(x => x.AddAsync(Arg.Any<Driver>(), CancellationToken.None))
            .Do(callInfo => capturedDriver = callInfo.Arg<Driver>());

        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().NotBeEmpty();

        capturedDriver!.TenantId.Should().Be(_tenantId);
        capturedDriver!.ExternalId.Should().Be(command.ExternalId);
        capturedDriver!.UserId.Should().BeNull();
        capturedDriver!.Name.Should().Be(command.Name);
        capturedDriver!.Phone.Should().Be(command.Phone);
        capturedDriver!.LicenseNumber.Should().Be(command.LicenseNumber);
        capturedDriver!.LicenseExpiryDate.Should().Be(command.LicenseExpiryDate);

        capturedDriver.Should().NotBeNull();

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrow()
    {
        var command = new CreateDriverCommand
        {
            ExternalId = "D-00001",
            UserId = Guid.CreateVersion7(),
            Name = "Pedro",
            Phone = "12345678",
            LicenseNumber = "8310XY",
            LicenseExpiryDate = DateOnly.MaxValue
        };

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*Given user doesn't exist*");
    }

    [Fact]
    public async Task Handle_WhenDriverAlreadyExists_ShouldThrow()
    {
        var command = new CreateDriverCommand
        {
            ExternalId = "D-00001",
            UserId = Guid.CreateVersion7(),
            Name = "Pedro",
            Phone = "12345678",
            LicenseNumber = "8310XY",
            LicenseExpiryDate = DateOnly.MaxValue
        };

        _driverRepositoryMock
            .ExistsByTenantIdAndUserIdAndExternalIdAsync(
                _tenantId,
                command.UserId,
                command.ExternalId,
                CancellationToken.None
            )
            .Returns(true);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("*Driver already exists*");
    }
}