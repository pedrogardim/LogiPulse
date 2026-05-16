using LogiPulse.Application.Drivers.Commands.CreateDriver;
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
}