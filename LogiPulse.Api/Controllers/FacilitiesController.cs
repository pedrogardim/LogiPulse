using LogiPulse.Api.Extensions;
using LogiPulse.Application.Drivers.Commands;
using LogiPulse.Application.Drivers.DTOs;
using LogiPulse.Application.Facilities.Commands.CreateFacility;
using LogiPulse.Application.Vehicles.Commands;
using LogiPulse.Application.Vehicles.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacilitiesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVehicleAsync([FromBody] CreateFacilityRequest request)
    {
        var tenantId = HttpContext.GetTenantId();

        var command = new CreateFacilityCommand
        {
            TenantId = tenantId,
            ExternalId = request.ExternalId,
            Name = request.Name,
            Code = request.Code,
            FacilityType = request.FacilityType,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Address = request.Address
        };

        var result = await mediator.Send(command);
        return Ok(result);
    }
}