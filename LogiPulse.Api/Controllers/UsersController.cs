using LogiPulse.Application.Drivers.Commands.CreateDriver;
using LogiPulse.Application.Drivers.Commands.DeleteDriver;
using LogiPulse.Application.Users.Commands.DeleteUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUserAsync(Guid id)
    {
        await mediator.Send(new DeleteUserCommand(id));
        return NoContent();
    }
}