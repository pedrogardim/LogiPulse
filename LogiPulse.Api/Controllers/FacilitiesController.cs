using LogiPulse.Api.Extensions;
using LogiPulse.Application.Facilities.Commands.CreateFacility;
using LogiPulse.Application.Facilities.Queries.ListFacilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacilitiesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] ListFacilitiesQuery query)
    {
        var command = new ListFacilitiesQuery(query.FacilityType, query.Search, query.Page, query.PageSize);

        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateFacilityRequest request)
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