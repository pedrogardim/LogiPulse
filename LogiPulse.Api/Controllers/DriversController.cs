using LogiPulse.Api.Extensions;
using LogiPulse.Application.Drivers.Commands;
using LogiPulse.Application.Drivers.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDriverAsync([FromBody] CreateDriverRequest request)
    {
        var tenantId = HttpContext.GetTenantId();

        var command = new CreateDriverCommand
        {
            TenantId = tenantId,
            ExternalId = request.ExternalId,
            UserId = request.UserId,
            Name = request.Name,
            Phone = request.Phone,
            LicenseNumber = request.LicenseNumber,
            LicenseExpiryDate = request.LicenseExpiryDate
        };

        var result = await mediator.Send(command);
        return Ok(result);
    }
}