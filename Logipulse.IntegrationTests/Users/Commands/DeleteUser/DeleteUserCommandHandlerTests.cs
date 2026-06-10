using FluentAssertions;
using LogiPulse.Application.Users.Commands.DeleteUser;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Exceptions;
using LogiPulse.Domain.Shared;
using Logipulse.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace LogiPulse.IntegrationTests.Users.Commands.DeleteUser;

public class DeleteUserCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_WhenValid_ShouldDeleteUserAndCommits()
    {
        var user = User.Create(UserContext.TenantId, Email.Create("123@mail.com"), "User");

        await DbContext.Users.AddAsync(user);
        await DbContext.SaveChangesAsync();

        await Sender.Send(new DeleteUserCommand(user.Id));

        var result = await DbContext.Vehicles.AnyAsync(v => v.Id == user.Id);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenUserDontExist_ShouldThrow()
    {
        var id = Guid.CreateVersion7();

        var act = async () => await Sender.Send(new DeleteUserCommand(id));

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*User don't exist*");
    }
}