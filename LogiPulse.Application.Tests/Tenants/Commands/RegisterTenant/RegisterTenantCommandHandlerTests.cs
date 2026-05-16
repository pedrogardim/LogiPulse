using FluentAssertions;
using LogiPulse.Application.Interfaces;
using LogiPulse.Application.Tenants.Commands.RegisterTenant;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using NSubstitute;

namespace LogiPulse.Application.Tests.Tenants.Commands.RegisterTenant;

public class RegisterTenantCommandHandlerTests
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly ITenantRepository _tenantRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly RegisterTenantCommandHandler _handler;
    private readonly RegisterTenantCommand _command;

    public RegisterTenantCommandHandlerTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _tenantRepositoryMock = Substitute.For<ITenantRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _handler = new RegisterTenantCommandHandler(
            _userRepositoryMock,
            _tenantRepositoryMock,
            _unitOfWorkMock
        );

        _command = new RegisterTenantCommand
        {
            TaxCode = "1234",
            DisplayName = "LogiPulse",
            AdminUserEntraId = Guid.NewGuid(),
            AdminUserEmail = "admin@mail.com",
            AdminUserName = "admin"
        };
    }

    [Fact]
    public async Task Handle_ShouldCreateTenantAndUser()
    {
        Tenant? capturedTenant = null;
        User? capturedUser = null;

        _tenantRepositoryMock
            .When(x => x.AddAsync(Arg.Any<Tenant>()))
            .Do(callInfo => capturedTenant = callInfo.Arg<Tenant>());

        _userRepositoryMock
            .When(x => x.AddAsync(Arg.Any<User>()))
            .Do(callInfo => capturedUser = callInfo.Arg<User>());

        var result = await _handler.Handle(_command, CancellationToken.None);
        result.Should().NotBeEmpty();

        capturedTenant.Should().NotBeNull();
        capturedTenant!.TaxCode.Should().Be(_command.TaxCode);
        capturedTenant!.DisplayName.Should().Be(_command.DisplayName);

        capturedUser.Should().NotBeNull();
        capturedUser!.EntraId.Should().Be(_command.AdminUserEntraId);
        capturedUser!.Email.Should().Be(Email.Create(_command.AdminUserEmail));
        capturedUser!.FullName.Should().Be(_command.AdminUserName);

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ExistingTenant_ShouldThrow()
    {
        _tenantRepositoryMock
            .ExistsByTaxCodeAsync(_command.TaxCode)
            .Returns(true);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("Tenant already exists");
    }

    [Fact]
    public async Task Handle_ExistingUser_ShouldThrow()
    {
        _userRepositoryMock
            .ExistsByEmailAsync(Email.Create(_command.AdminUserEmail))
            .Returns(true);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("User already exists and belongs to a tenant");
    }
}