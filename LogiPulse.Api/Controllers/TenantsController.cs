using LogiPulse.Api.Attributes;
using LogiPulse.Api.Extensions;
using LogiPulse.Application.Tenants.Commands.RegisterTenant;
using Microsoft.AspNetCore.Mvc;
using LogiPulse.Infrastructure.Persistence;
using MediatR;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController(IMediator mediator, LogiPulseDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var dispatches = dbContext.Tenants.ToList();
        return Ok(dispatches);
    }

    [HttpPost("register")]
    [BypassUserValidation]
    public async Task<IActionResult> RegisterNewTenantAsync([FromBody] RegisterTenantRequest request)
    {
        var entraId = User.GetObjectId();
        var email = User.GetEmail();
        var name = User.GetName();

        var command = new RegisterTenantCommand
        {
            TaxCode = request.TaxCode,
            DisplayName = request.DisplayName,
            AdminUserEntraId = entraId,
            AdminUserEmail = email,
            AdminUserName = name
        };

        var result = await mediator.Send(command);
        return Ok(result);
    }
}