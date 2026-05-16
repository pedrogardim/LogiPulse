using LogiPulse.Application.Vehicles.Commands.CreateVehicle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVehicleAsync([FromBody] CreateVehicleCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}