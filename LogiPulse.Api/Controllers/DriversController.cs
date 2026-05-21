using LogiPulse.Application.Drivers.Commands.CreateDriver;
using LogiPulse.Application.Drivers.Commands.DeleteDriver;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDriverAsync([FromBody] CreateDriverCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDriverAsync(Guid id)
    {
        await mediator.Send(new DeleteDriverCommand(id));
        return NoContent();
    }
}