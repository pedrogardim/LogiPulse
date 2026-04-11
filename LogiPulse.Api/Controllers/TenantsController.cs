using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Tenants;

namespace LogiPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    // An action method that handles HTTP GET requests
    [HttpGet]
    public IActionResult Get()
    {
        var externalId = RandomNumberGenerator.GetHexString(16);
        return Ok(Tenant.Create("LogiPulse Logistics", "Y777888999")); // Returns an HTTP 200 OK status with a message
    }
}