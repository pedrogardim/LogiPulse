using FluentAssertions;
using LogiPulse.Application.Common;
using LogiPulse.Application.Users.Commands.DeleteUser;
using LogiPulse.Application.Interfaces;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using NSubstitute;

namespace LogiPulse.Application.Tests.Users.Commands.DeleteUser;

public class DeleteUserCommandHandlerTests
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeleteUserCommandHandler _handler;
    private readonly Guid _tenantId;

    public DeleteUserCommandHandlerTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        var userContextMock = Substitute.For<IUserContext>();
        _tenantId = Guid.CreateVersion7();
        userContextMock.TenantId.Returns(_tenantId);

        _handler = new DeleteUserCommandHandler(
            _userRepositoryMock,
            userContextMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteUserAndCommits()
    {
        var user = User.Create(_tenantId, Email.Create("test@mail.com"), "User");

        var command = new DeleteUserCommand(user.Id);

        _userRepositoryMock
            .GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Received(1).Remove(user);

        await _unitOfWorkMock.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUserDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        _userRepositoryMock
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<User?>(null));

        var act = async () => await _handler.Handle(new DeleteUserCommand(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*User don't exist*");
    }
}