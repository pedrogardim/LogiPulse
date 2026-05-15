using LogiPulse.Api.Extensions;
using LogiPulse.Application.Drivers.Commands;
using LogiPulse.Application.Drivers.DTOs;
using LogiPulse.Application.Vehicles.Commands;
using LogiPulse.Application.Vehicles.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVehicleAsync([FromBody] CreateVehicleRequest request)
    {
        var tenantId = HttpContext.GetTenantId();

        var command = new CreateVehicleCommand
        {
            TenantId = tenantId,
            ExternalId = request.ExternalId,
            Name = request.Name,
            LicensePlate = request.LicensePlate,
            VehicleType = request.VehicleType
        };

        var result = await mediator.Send(command);
        return Ok(result);
    }
}