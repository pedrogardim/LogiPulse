using LogiPulse.Api.Extensions;
using LogiPulse.Application.Facilities.Commands.CreateFacility;
using LogiPulse.Application.Facilities.Commands.UpdateFacility;
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
    public async Task<IActionResult> CreateAsync([FromBody] CreateFacilityCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateFacilityCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }
}